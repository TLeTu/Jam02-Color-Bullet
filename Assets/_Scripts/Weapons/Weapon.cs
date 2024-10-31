using UnityEngine;
using UnityEngine.Pool;
using Utilities;

public abstract class Weapon : PoolerBase<Bullet>
{
    [SerializeField] protected float _fireRate = 1f; // 1 bullet per second
    [SerializeField] protected float _damage = 1f;

    protected CountdownTimer _fireTimer;

    public float Damage => _damage;

    protected override void Awake()
    {
        base.Awake();

        _fireTimer = new CountdownTimer(1f / _fireRate);

        if (_prefab == null) throw new System.InvalidOperationException("Prefab is not set.");
        if (_holder == null)
        {
            //find a holder object name "Bullet Holder"
            _holder = GameObject.Find("BulletHolder");
        }

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
