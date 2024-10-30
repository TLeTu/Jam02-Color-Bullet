using UnityEngine;
using UnityEngine.Pool;
using Utilities;

public abstract class Weapon : PoolerBase<Bullet>
{
    [SerializeField] protected float _fireRate = 1f; // 1 bullet per second


    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private GameObject _bulletHolder;

    protected CountdownTimer _fireTimer;

    protected override void Awake()
    {
        base.Awake();

        _fireTimer = new CountdownTimer(1f / _fireRate);
    }

    protected virtual void Update()
    {
        _fireTimer.Tick(Time.deltaTime);
    }


    protected virtual Bullet GetBullet()
    {
        return Get();
    }

    protected void DespawnBullet(Bullet bullet)
    {
        Return(bullet);
    }

    protected override void Initialize(Bullet obj)
    {
        obj.Initialize(this);
    }

    protected override void GetSetup(Bullet obj)
    {
        obj.Despawn += DespawnBullet;
    }

    protected virtual bool CanFire()
    {
        if (!_fireTimer.IsFinished && _fireTimer.IsRunning)
        {
            return false;
        }

        _fireTimer.Reset();
        _fireTimer.Start();

        return true;
    }


    // This method is called when the player presses the fire button
    public abstract void Fire(Vector2 aimPoint, UnitController source = null);
}
