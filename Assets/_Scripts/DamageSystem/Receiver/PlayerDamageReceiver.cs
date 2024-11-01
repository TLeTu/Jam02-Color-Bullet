using System;
using UnityEngine;
using Utilities;

public class PlayerDamageReceiver : DamageReceiver
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float unvanurableDuration;
    public Action DeathAction;

    private CountdownTimer _unvanurableTimer;

    public bool IsUnvanurable => _unvanurableTimer.IsRunning;

    private void Awake()
    {
        _unvanurableTimer = new CountdownTimer(unvanurableDuration);
        _unvanurableTimer.Stop();

    }

    private void Update()
    {
        _unvanurableTimer.Tick(Time.deltaTime);
        if (_unvanurableTimer.IsFinished)
        {
            _spriteRenderer.color = Color.white;
            _unvanurableTimer.Stop();
        }
        else
        {
            //make sprite color blink
            _spriteRenderer.color = new Color(1, 1, 1, Mathf.PingPong(Time.time * 10, 1));
        }

    }

    public override void TakeDamage(float dmg)
    {
        if (IsUnvanurable) return;

        base.TakeDamage(dmg);
        if (_currentHP <= 0)
        {
            DeathAction?.Invoke();
        }

        _unvanurableTimer.Start();

    }
}
