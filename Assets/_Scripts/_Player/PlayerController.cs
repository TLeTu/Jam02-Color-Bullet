using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region COMPONENTS
    [SerializeField] private PlayerAnimator _playerAnimator;
    [SerializeField] private WeaponController _weaponController;
    #endregion
    #region DATA
    private PlayerColor _playerColor;
    #endregion
    private void Awake()
    {
        _playerColor = PlayerColor.White;
    }
    private void Update()
    {
        _playerAnimator.Render(_playerColor);

        if (InputReader.Instance.IsFiring)
        {
            _weaponController.FireCurrentWeapon();
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
