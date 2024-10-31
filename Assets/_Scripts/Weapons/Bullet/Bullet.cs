using System;
using UnityEngine;
using Utilities;

public abstract class Bullet : MonoBehaviour
{
    [Header("Bullet Base")]
    [SerializeField] protected float _despawnTime;
    [SerializeField] protected float _damage;
    [SerializeField] protected float _flySpeed;


    public Action<Bullet> Despawn;

    private CountdownTimer _despawnTimer;


    protected virtual void Update()
    {
        if (_despawnTimer != null)
            _despawnTimer.Tick(Time.deltaTime);
    }

    protected void StartDespawnTimer()
    {
        _despawnTimer = new CountdownTimer(_despawnTime);
        _despawnTimer.OnTimerStop += () => Despawn?.Invoke(this);
        _despawnTimer.Start();
    }

    public abstract void Initialize(Weapon sourceWeapon);
}
