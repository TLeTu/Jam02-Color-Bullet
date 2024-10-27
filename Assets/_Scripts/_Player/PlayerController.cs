using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region DATA
    private PlayerColor _playerColor;
    #endregion
    #region COMPONENTS
    public PlayerAnimator _playerAnimator;
    #endregion

    private void Awake()
    {
        _playerColor = PlayerColor.White;
    }
    private void Update()
    {
        _playerAnimator.Render(_playerColor);
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
