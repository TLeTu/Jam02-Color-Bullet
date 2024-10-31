using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    protected float _maxHP;
    [SerializeField] protected float _currentHP;

    protected virtual void OnEnable()
    {
        _currentHP = _maxHP;
    }

    public void SetMaxHP(float maxHP)
    {
        _maxHP = maxHP;
        _currentHP = _maxHP;
    }

    public virtual void TakeDamage(float dmg)
    {
        _currentHP -= dmg;

        if ( _currentHP < 0 )
            _currentHP = 0;
    }
}
