using UnityEngine;

public class DefaultGun : Weapon
{
    public override void Fire(Vector2 aimPoint, UnitController source = null)
    {
        if (!CanFire()) return;

        GrenadeLauncherBullet bullet = GetBullet() as GrenadeLauncherBullet;

        bullet.Initialize(this);

        bullet.transform.rotation = transform.rotation;

        bullet.Firing(aimPoint);
    }
}
