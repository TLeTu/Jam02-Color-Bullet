using UnityEngine;

public class ColorPool : MonoBehaviour
{
    [SerializeField] private PlayerColor _playerColor;
    public PlayerColor PlayerColor => _playerColor;
}
