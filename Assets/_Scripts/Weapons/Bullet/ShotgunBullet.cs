using System;
using UnityEditor.Tilemaps;
using UnityEngine;

public class ShotgunBullet : Bullet
{
    [SerializeField] private ShotgunBulletDmgDealer _dealer;
    [SerializeField] private float offset;

    public override void Initialize(Weapon weapon)
    {
        transform.position = weapon.transform.position;
        transform.rotation = weapon.transform.rotation;
    }

    public void Firing(Vector2 direction)
    {
        RotateToDirection(transform, direction);

        //offset the bullet's position so it doesn't collide with the player
        transform.position += (Vector3)direction * offset;

        _dealer.DealOneShotDamage(_damage);

        StartDespawnTimer();
    }

    private void RotateToDirection(Transform transform, Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
