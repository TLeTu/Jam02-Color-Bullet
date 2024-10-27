using System;
using UnityEngine;
using Utilities;

public abstract class Bullet : MonoBehaviour
{
    [SerializeField] private float _despawnTime;

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

    public abstract void Initialize(Vector2 position);
}
