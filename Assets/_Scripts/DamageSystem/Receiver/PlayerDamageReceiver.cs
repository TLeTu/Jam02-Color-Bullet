using System;
using UnityEngine;

public class PlayerDamageReceiver : DamageReceiver
{
    public Action DeathAction;
    [SerializeField] UIController _uiController;

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);
        _uiController.SetHealth((int)_currentHP);
        if (_currentHP <= 0)
        {
            DeathAction?.Invoke();
        }
    }
}
