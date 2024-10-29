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
    }

    public override void AddForce(float force, Vector2 direction)
    {
        _playerMovement.LockMovement();
        LockForce();
        base.AddForce(force, direction);
    }

    public override void OverideForce(float force, Vector2 direction)
    {
        base.OverideForce(force, direction);
    }

    protected override void ResetForce()
    {
        _playerMovement.UnlockMovement();
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
