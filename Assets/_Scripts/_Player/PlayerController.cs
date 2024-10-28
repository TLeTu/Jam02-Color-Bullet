using UnityEngine;

public class PlayerController : UnitController
{

    #region COMPONENTS
    [SerializeField] private PlayerAnimator _playerAnimator;
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
