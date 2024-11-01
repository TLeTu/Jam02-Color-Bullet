using UnityEngine;
using Utilities;

/// <summary>
/// RED
/// </summary>
public class GrenadeLauncher : Weapon
{
    [SerializeField] AudioClip _fire;

    public override void Fire(Vector2 aimPoint, UnitController source = null)
    {
        if (!CanFire()) return;

        GrenadeLauncherBullet bullet = GetBullet() as GrenadeLauncherBullet;

        bullet.Initialize(this);

        bullet.transform.rotation = transform.rotation;

        bullet.Firing(aimPoint);

        AudioManager.Instance.PlaySound(_fire, transform.position);
    }
}
