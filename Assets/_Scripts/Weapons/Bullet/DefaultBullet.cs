using UnityEngine;

public class DefaultBullet : Bullet
{
    [SerializeField] private Animator _animator;
    [SerializeField] private DefaultBulletDmgDealer _dealer;

    [SerializeField] private AnimationCurve _animationCurve;


    private Vector2 _startPoint;
    private Vector2 _aimPoint;
    private Vector2 _direction;
    private bool _isFlying;
    private float _flyTime;

    private float _currentCurve;

    private static readonly int FlyAnimation = Animator.StringToHash("Fly");


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

    public override void Initialize(Weapon sourceWeapon)
    {
        transform.position = sourceWeapon.transform.position;
        transform.rotation = sourceWeapon.transform.rotation;

        _animator.CrossFade(FlyAnimation, 0, 0);


    }

    public void Firing(Vector2 aimPoint)
    {
        _startPoint = (Vector2)transform.position;
        _isFlying = true;
        _direction = (aimPoint - _startPoint.normalized) ;
        _aimPoint = GetPointOutsideCamera(_startPoint, aimPoint);
        _flyTime = Vector2.Distance(_startPoint, _aimPoint) / _flySpeed;

        RotateToAimPoint(_direction);
    }

    private void RotateToAimPoint(Vector2 direction)
    {
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
            _isFlying = false;
            StartDespawnTimer();
        }

    }
    public Vector2 GetPointOutsideCamera(Vector2 startPoint, Vector2 aimPoint)
    {
        Camera camera = Camera.main;

        Vector2 direction = (aimPoint - startPoint).normalized;

        Vector3 screenBottomLeft = camera.ScreenToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        Vector3 screenTopRight = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, camera.nearClipPlane));
        float maxScreenDistance = Vector2.Distance(screenBottomLeft, screenTopRight) / 2;

        Vector2 aimPoint2 = aimPoint + direction * maxScreenDistance;

        return aimPoint2;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out DamageReceiver receiver))
        {
            _dealer.DealOneShotDamage(_damage, receiver);
            _isFlying = false;
            StartDespawnTimer();
        }
    }
}
