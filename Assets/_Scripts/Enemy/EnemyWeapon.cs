using System;
using UnityEngine;
using Utilities;
using static UnityEngine.GraphicsBuffer;

public class EnemyWeapon : Weapon
{
    [Header("Attack Setting")]
    [SerializeField] private float _attackDamage;

    public void SetupWeapon(float attackDamage)
    {
        _attackDamage = attackDamage;
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
        throw new NotImplementedException();
    }
}
