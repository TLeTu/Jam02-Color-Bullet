using System;
using UnityEngine;

public class GrenadeLauncherBullet : Bullet
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GrenadeLauncherBulletDmgDealer _dealer;

    [SerializeField] private float _flySpeed = 1;
    [SerializeField] private AnimationCurve _animationCurve;

    [SerializeField] private float _damage;

    private Vector2 _startPoint;
    private Vector2 _aimPoint;
    private bool _isFlying;
    private float _flyTime;

    private float _currentCurve;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _isFlying = false;
        _currentCurve = 0;
    }

    protected override void Update()
    {
        base.Update();

        if (_isFlying)
        {
            HandleFlying();
        }
    }

    public override void Initialize(Vector2 position)
    {
        transform.position = position;
    }

    public void Firing(Vector2 aimPoint)
    {
        _startPoint = (Vector2)transform.position;
        _aimPoint = aimPoint;
        _isFlying = true;
        _flyTime = Vector2.Distance(_startPoint, _aimPoint) / _flySpeed;

        RotateToAimPoint(aimPoint);
    }

    private void RotateToAimPoint(Vector2 aimPoint)
    {
        Vector2 direction = aimPoint - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void HandleFlying()
    {
        _currentCurve += Mathf.MoveTowards(0, 1, Time.deltaTime / _flyTime);

        Vector2 point = Vector2.Lerp(_startPoint, _aimPoint, _animationCurve.Evaluate(_currentCurve));

        transform.position = (Vector3)point;

        if (_currentCurve >= 1)
        {
            Explode();
            _isFlying = false;
        }

    }

    private void Explode()
    {
        _animator.SetTrigger("Explode");
        _dealer.DealOneShotDamage(_damage);

        StartDespawnTimer();
    }


}
