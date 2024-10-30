using UnityEngine;

public class Sniper : Weapon
{
    public override void Fire(Vector2 aimPoint, UnitController source = null)
    {
        if (!CanFire()) return;

        SniperBullet bullet = GetBullet() as SniperBullet;

        bullet.Initialize(this);

        bullet.transform.rotation = transform.rotation;

        bullet.Firing(aimPoint);
    }
}
