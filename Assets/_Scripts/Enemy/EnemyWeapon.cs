using System;
using UnityEngine;
using Utilities;
using static UnityEngine.GraphicsBuffer;

public class EnemyWeapon : Weapon
{
    public void SetupWeapon(float attackDamage)
    {
        _damage = attackDamage;
    }

    public void CloseRangeAttack(UnitController target)
    {

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
