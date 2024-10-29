using UnityEngine;
using Utilities;

/// <summary>
/// RED
/// </summary>
public class GrenadeLauncher : Weapon
{
    public override void Fire(Vector2 aimPoint, UnitController source = null)
    {
        if (!CanFire()) return;

        GrenadeLauncherBullet bullet = GetBullet() as GrenadeLauncherBullet;

        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;

        bullet.Firing(aimPoint);
    }
}
