using UnityEngine;
using StateMachineBehaviour;
using Utilities;
using System;

public class EnemyController : UnitController
{
    [Header("Enemy Setting")]
    [SerializeField] private EnemyType _type;
    [SerializeField] private Animator _animator;
    
    [Header("Attack Setting")]
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackCooldown;

    [Header("Target")]
    [SerializeField] private UnitController _target;


    StateMachine _stateMachine;

    CountdownTimer _cooldownTimer;

    protected override void Start()
    {
        base.Start();

        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<UnitController>();

        _cooldownTimer = new CountdownTimer(_attackCooldown);
        _cooldownTimer.Stop();

        _stateMachine = new StateMachine();

        EnemyBaseState chaseState = new EnemyChaseState(this, _animator);
        EnemyBaseState attackState = new EnemyAttackState(this, _animator);

        At(chaseState, attackState, new FuncPredicate(() => IsInRange()));
        At(attackState, chaseState, new FuncPredicate(() => _cooldownTimer.IsFinished || !_cooldownTimer.IsRunning));

        _stateMachine.SetState(chaseState);


    }

    void At(IState from, IState to, IPredicate condition) => _stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);

    protected override void Update()
    {
        base.Update();
        _stateMachine.Update();
        _cooldownTimer.Tick(Time.deltaTime);
    }

    protected override void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    public void HandleMovement()
    {
        Vector2 direction = (_target.transform.position - transform.position).normalized;

        Locomotion(direction);
    }

    public bool IsInRange()
    {
        return Vector2.Distance(transform.position, _target.transform.position) <= _attackRange;
    }

    public void Attacking()
    {
        _cooldownTimer.Reset();
        _cooldownTimer.Start();

        switch(_type)
        {
            case EnemyType.CloseRange:
                CloseRangeAttack();
                break;
            case EnemyType.LongRange:
                LongRangeAttack();
                break;
        }
    }

    private void CloseRangeAttack()
    {

    }

    private void LongRangeAttack()
    {
        //fire a projectile

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }

}