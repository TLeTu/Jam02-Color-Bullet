using UnityEngine;

public class EnemyBullet : Bullet
{
    [SerializeField] private Animator _animator;
    [SerializeField] private EnemyBulletDmgDealer _dealer;

    private Vector2 _direction;
    private bool _isFlying;

    private static readonly int FlyAnimation = Animator.StringToHash("Fly");


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _isFlying = false;
    }

    protected override void Update()
    {
        base.Update();

        if (_isFlying)
        {
            HandleFlying();
        }
    }

    public override void Initialize(Weapon sourceWeapon)
    {
        transform.position = sourceWeapon.transform.position;
        transform.rotation = sourceWeapon.transform.rotation;

        _animator.CrossFade(FlyAnimation, 0, 0);

        _sourceWeapon = sourceWeapon;

        _isFlying = false;
    }

    public void Firing(Vector2 aimPoint)
    {
        _direction = aimPoint - (Vector2)transform.position;
        RotateToAimPoint(_direction);

        _isFlying = true;

        StartDespawnTimer();
    }

    private void RotateToAimPoint(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void HandleFlying()
    {
        transform.position += (Vector3)(_direction.normalized * _flySpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out DamageReceiver receiver))
        {
            if (receiver is PlayerDamageReceiver)
            {
                receiver.TakeDamage(_sourceWeapon.Damage);

                Despawn?.Invoke(this);

                return;
            }
        }
    }
}
