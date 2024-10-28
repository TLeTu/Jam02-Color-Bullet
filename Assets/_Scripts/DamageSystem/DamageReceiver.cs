using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] private float _maxHP;
    [SerializeField] private float _currentHP;

    public void TakeDamage(float dmg)
    {
        _currentHP -= dmg;

        if ( _currentHP < 0 )
            _currentHP = 0;
    }
}
