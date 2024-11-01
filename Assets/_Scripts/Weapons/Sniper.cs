using UnityEngine;
using Utilities;
using static Unity.VisualScripting.Member;

public class Sniper : Weapon
{
    [SerializeField] AudioClip _fire;

    [SerializeField] private float _specificLockForceTime = 0.5f;

    private CountdownTimer _lockTimer;

    private Vector2 aimPoint;

    protected override void Awake()
    {
        base.Awake();

        _lockTimer = new CountdownTimer(_specificLockForceTime);
        _lockTimer.Start();
        _lockTimer.Stop();
        Debug.Log("Sniper Awake: " + _lockTimer.Progress + " - " + _lockTimer.IsFinished);
    }

    protected override void Update()
    {
        base.Update();

        _lockTimer.Tick(Time.deltaTime);

        if (_lockTimer.IsFinished)
        {
            Firing();
            _lockTimer.Reset();
            _lockTimer.Stop();
        }
    }

    public override void Fire(Vector2 aimPoint, UnitController source = null)
    {
        if (!CanFire()) return;

        source.LockMotion(_specificLockForceTime);

        this.aimPoint = aimPoint;
        _lockTimer.Start();

    }

    private void Firing()
    {
        SniperBullet bullet = GetBullet() as SniperBullet;

        bullet.Initialize(this);

        bullet.transform.rotation = transform.rotation;

        bullet.Firing(aimPoint);

        AudioManager.Instance.PlaySound(_fire, transform.position);
    }

}
