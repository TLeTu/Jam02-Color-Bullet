using UnityEngine;
using Utilities;

/// <summary>
/// RED
/// </summary>
public class GrenadeLauncher : Weapon
{
    [SerializeField] private float _fireRate = 1f; // 1 bullet per second

    private CountdownTimer _fireTimer;

    protected override void Awake()
    {
        base.Awake();
        _fireTimer = new CountdownTimer(1f/_fireRate);
    }

    private void Update()
    {
        _fireTimer.Tick(Time.deltaTime);
    }

    public override void Fire(Vector2 aimPoint)
    {
        if (!_fireTimer.IsFinished && _fireTimer.IsRunning)
        {
            return;
        }

        _fireTimer.Reset();
        _fireTimer.Start();

        GrenadeLauncherBullet bullet = GetBullet() as GrenadeLauncherBullet;

        bullet.Firing(aimPoint);
    }
}
