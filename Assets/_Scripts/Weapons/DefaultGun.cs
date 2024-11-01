using UnityEngine;

public class DefaultGun : Weapon
{
    [SerializeField] AudioClip _fire;

    public override void Fire(Vector2 aimPoint, UnitController source = null)
    {
        if (!CanFire()) return;

        DefaultBullet bullet = GetBullet() as DefaultBullet;

        bullet.Initialize(this);

        bullet.transform.rotation = transform.rotation;

        bullet.Firing(aimPoint);

        AudioManager.Instance.PlaySound(_fire, transform.position);
    }
}
