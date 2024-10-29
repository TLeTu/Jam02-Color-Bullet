using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] private float _knockbackForce;

    public override void Fire(Vector2 aimPoint, UnitController source = null)
    {
        if (!CanFire()) return;

        Vector2 direction = (aimPoint - (Vector2)transform.position).normalized;

        ShotgunBullet bullet = GetBullet() as ShotgunBullet;

        bullet.Firing(direction);

        if (source != null)
        {
            source.OverideForce(_knockbackForce, -direction);
            //Debug.Log(source.name + " Knock " + _knockbackForce + " - " + direction);
        }

    }
}
