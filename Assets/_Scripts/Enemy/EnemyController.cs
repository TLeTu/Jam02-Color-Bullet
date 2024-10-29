using UnityEngine;
using StateMachineBehaviour;
using Utilities;

public class EnemyController : UnitController
{
    [Header("Enemy Setting")]
    [SerializeField] private EnemyType _type;
    [SerializeField] private Animator _animator;
    
    [Header("Attack Setting")]
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackCooldown;

    [Header("Movement")]
    [SerializeField] private float _runMaxSpeed = 5f;
    [SerializeField] private float _runAccelAmount = 50f;
    [SerializeField] private float _runDeccelAmount = 50f;

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

    public void FollowTarget()
    {
        if (_lockForce) return;

        Vector2 direction = (_target.transform.position - transform.position).normalized;

        #region NO TOUCHING
        float targetSpeedX = direction.x * _runMaxSpeed;
        float targetSpeedY = direction.y * _runMaxSpeed;

        targetSpeedX = Mathf.Lerp(_rb.linearVelocity.x, targetSpeedX, 1);
        targetSpeedY = Mathf.Lerp(_rb.linearVelocity.y, targetSpeedY, 1);

        float accelRateX;
        float accelRateY;

        accelRateX = (Mathf.Abs(targetSpeedX) > 0.01f) ? _runAccelAmount : _runDeccelAmount;
        accelRateY = (Mathf.Abs(targetSpeedY) > 0.01f) ? _runAccelAmount : _runDeccelAmount;

        float speedDifX = targetSpeedX - _rb.linearVelocity.x;
        float speedDifY = targetSpeedY - _rb.linearVelocity.y;

        float movementX = speedDifX * accelRateX;
        float movementY = speedDifY * accelRateY;
        #endregion

        _rb.AddForce(new Vector2(movementX, movementY), ForceMode2D.Force);
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