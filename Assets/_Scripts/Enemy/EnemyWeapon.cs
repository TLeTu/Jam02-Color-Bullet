using System;
using UnityEngine;
using Utilities;
using static UnityEngine.GraphicsBuffer;

public class EnemyWeapon : Weapon
{
    [SerializeField] private EnemyCloseRangeDamageDealer _closeRangeDamageDealer;

    public void SetupWeapon(float attackDamage)
    {
        _damage = attackDamage;
    }

    public void CloseRangeAttack(float range)
    {
        _closeRangeDamageDealer.DealOneShotDamage(_damage, range);

    }

    public void LongRangeAttack(UnitController target)
    {
        EnemyBullet bullet = GetBullet() as EnemyBullet;

        bullet.Initialize(this);

        bullet.Firing(target.transform.position);


    }

    public override void Fire(Vector2 aimPoint, UnitController source = null)
    {
    }
}
