using UnityEngine;

public class Sniper : Weapon
{
    [SerializeField] AudioClip _fire;

    [SerializeField] private float _specificLockForceTime = 0.5f;

    public override void Fire(Vector2 aimPoint, UnitController source = null)
    {
        if (!CanFire()) return;

        SniperBullet bullet = GetBullet() as SniperBullet;

        bullet.Initialize(this);

        bullet.transform.rotation = transform.rotation;

        bullet.Firing(aimPoint);

        if (source != null)
        {
            source.OverideForce(0, Vector2.zero, _specificLockForceTime);
        }
        AudioManager.Instance.PlaySound(_fire, transform.position);

    }
}
