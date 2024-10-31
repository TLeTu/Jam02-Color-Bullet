using System;
using UnityEngine;

public class PlayerDamageReceiver : DamageReceiver
{
    public Action DeathAction;

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);
        if (_currentHP <= 0)
        {
            DeathAction?.Invoke();
        }
    }
}
