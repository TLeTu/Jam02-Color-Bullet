using UnityEngine;
public class PlayerAnimator : MonoBehaviour
{
    #region COMPONENTS
    public Animator _animator { get; private set; }
    public SpriteRenderer _spriteRenderer { get; private set; }
    #endregion

    #region DATA
    private float _lockedTill;
    #endregion

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    #region ANIMATE METHODS
    public void Render(PlayerColor playerColor)
    {
        var state = GetState(playerColor);

        _animator.CrossFade(state, 0, 0);
    }
    private int GetState(PlayerColor playerColor)
    {
        return playerColor switch
        {
            PlayerColor.White => IdleWhite,
            PlayerColor.Red => IdleRed,
            PlayerColor.Blue => IdleBlue,
            PlayerColor.Green => IdleGreen,
            PlayerColor.Yellow => IdleYellow,
            PlayerColor.Orange => IdleOrange,
            PlayerColor.Purple => IdlePurple,
            _ => IdleWhite,
        };
    }
    #endregion

    #region STATE
    private static readonly int IdleWhite = Animator.StringToHash("Idle-White");
    private static readonly int IdleRed = Animator.StringToHash("Idle-Red");
    private static readonly int IdleBlue = Animator.StringToHash("Idle-Blue");
    private static readonly int IdleGreen = Animator.StringToHash("Idle-Green");
    private static readonly int IdleYellow = Animator.StringToHash("Idle-Yellow");
    private static readonly int IdleOrange = Animator.StringToHash("Idle-Orange");
    private static readonly int IdlePurple = Animator.StringToHash("Idle-Purple");
    #endregion
}
