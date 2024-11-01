using UnityEngine;
using StateMachineBehaviour;
using Utilities;
using System;
using UnityEditor.Animations;
using static UnityEngine.GraphicsBuffer;

public class EnemyController : UnitController
{
    [Header("Enemy Setting")]
    [SerializeField] private EnemyType _type;
    [SerializeField] private Animator _animator;
    [SerializeField] private EnemyDamageReceiver _damageReceiver;

    [Header("Attack Setting")]
    [SerializeField] private EnemyWeapon _weapon;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackDuration;
    [SerializeField] private float _attackCooldown;


    [Header("Target")]
    [SerializeField] private UnitController _target;

    public Action<EnemyController> DespawnAction { get; set; }


    StateMachine _stateMachine;

    CountdownTimer _durationTimer;
    CountdownTimer _cooldownTimer;

    public EnemyType Type => _type;
    public bool OnCooldown => _cooldownTimer.IsRunning;

    protected override void Awake()
    {
        base.Awake();

        if (!TryGetComponent<Animator>(out _animator)) _animator = gameObject.AddComponent<Animator>();
        else _animator = GetComponent<Animator>();

        _damageReceiver.DeathAction += () => DespawnAction?.Invoke(this);

        _durationTimer = new CountdownTimer(0);
        _durationTimer.Stop();

        _cooldownTimer = new CountdownTimer(0);
        _cooldownTimer.Stop();
    }

    protected override void Start()
    {
        base.Start();

        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<UnitController>();

        _stateMachine = new StateMachine();

        EnemyBaseState chaseState = new EnemyChaseState(this, _animator);
        EnemyBaseState attackState = new EnemyAttackState(this, _animator);

        At(chaseState, attackState, new FuncPredicate(() => IsInRange()));
        At(attackState, chaseState, new FuncPredicate(() => !IsInRange() && (_durationTimer.IsFinished || !_durationTimer.IsRunning)));

        _stateMachine.SetState(chaseState);
    }

    void At(IState from, IState to, IPredicate condition) => _stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);

    protected override void Update()
    {
        base.Update();
        _stateMachine.Update();
        _durationTimer.Tick(Time.deltaTime);
        _cooldownTimer.Tick(Time.deltaTime);
    }

    protected override void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    public void Initialize (EnemyType type, EnemySpawner spawner)
    {
        _type = type;

        AnimatorController controller = spawner.RandomLongRangeAnimator;
        SetAnimatorController(controller);

        _damageReceiver.SetMaxHP(type == EnemyType.CloseRange ? spawner.CloseRangeHealth : spawner.LongRangeHealth);

        float dmg;
        float range;
        float duration;
        float cooldown;

        if (type == EnemyType.CloseRange)
        {
            dmg = spawner.CloseRangeDamage;
            range = spawner.CloseRangeAtkRange ;
            duration = spawner.CloseRangeAtkDuration;
            cooldown = spawner.CloseRangeAtkCooldown;
        }
        else
        {
            dmg = spawner.LongRangeDamage;
            range = spawner.LongRangeAtkRange;
            duration = spawner.LongRangeAtkDuration;
            cooldown = spawner.LongRangeAtkCooldown;
        }

        _durationTimer.Reset(duration);
        _cooldownTimer.Reset(cooldown);
        _attackRange = range;
        _weapon.SetupWeapon(dmg);
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
        _durationTimer.Reset();
        _durationTimer.Start();

        _cooldownTimer.Reset();
        _cooldownTimer.Start();

        switch(_type)
        {
            case EnemyType.CloseRange:
                _weapon.CloseRangeAttack(_attackRange);
                break;
            case EnemyType.LongRange:
                _weapon.LongRangeAttack(_target);
                break;
        }
    }
    public void SetAnimatorController(AnimatorController controller)
    {
        _animator.runtimeAnimatorController = controller;
    }
}