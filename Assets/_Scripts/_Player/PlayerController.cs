using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerController : UnitController
{

    #region COMPONENTS
    [SerializeField] private PlayerAnimator _playerAnimator;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private WeaponController _weaponController;
    #endregion
    #region DATA
    private PlayerColor _playerColor;

    private Vector2 _moveInput;
    private Vector3 _mousePositon;
    private Vector2 _direction;
    #endregion
    protected override void Awake()
    {
        base.Awake();
        _playerColor = PlayerColor.White;
    }

    protected override void Start()
    {
        base.Start();
        InputReader.Instance.ChangeWeaponAction += _weaponController.ChangeNextWeapon;
    }

    protected override void Update()
    {
        base.Update();
        _playerAnimator.Render(_playerColor);

        if (InputReader.Instance.IsFiring)
        {
            _weaponController.FireCurrentWeapon(this);
        }

        _moveInput = InputReader.Instance.Direction;
        _mousePositon = InputReader.Instance.AimPoint;

        FaceToDirection();
    }

    protected override void FixedUpdate()
    {
        Locomotion(_moveInput);
    }

    private void FaceToDirection()
    {
        _direction = (_mousePositon - transform.position).normalized;
        if (_direction.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.right = _direction;
        }
        else if (_direction.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.right = -_direction;
        }
    }

    public override void AddForce(float force, Vector2 direction)
    {
        LockForce();
        base.AddForce(force, direction);
    }

    public override void OverideForce(float force, Vector2 direction, float timer = -1)
    {
        base.OverideForce(force, direction, timer);
    }

    public override void ResetForce()
    {
        UnlockForce();
        base.ResetForce();
    }

    #region COLLISION
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out ColorPool colorPool))
        {
            _playerColor = colorPool.PlayerColor;
        }
    }
    #endregion
}
public enum PlayerColor
{
    Red,
    Blue,
    Green,
    Yellow,
    Orange,
    Purple,
    White
}
