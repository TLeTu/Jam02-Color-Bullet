using UnityEngine;
using UnityEngine.Pool;

public abstract class Weapon : PoolerBase<Bullet>
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private GameObject _bulletHolder;

    protected virtual void Awake()
    {
        //Init a new object pool
        InitPool(_bulletPrefab, _bulletHolder);
    }

    protected Bullet GetBullet()
    {
        return Get();
    }

    protected void DespawnBullet(Bullet bullet)
    {
        Release(bullet);
    }

    protected override void GetSetup(Bullet obj)
    {
        base.GetSetup(obj);
        obj.Initialize(transform.position);

        obj.Despawn += DespawnBullet;
    }

    // This method is called when the player presses the fire button
    public abstract void Fire(Vector2 aimPoint);
}
