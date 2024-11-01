using System;
using System.Threading;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using Utilities;

public class PlayerController : UnitController
{
    #region Reference
    [Header("Reference")]
    [SerializeField] private PlayerAnimator _playerAnimator;
    [SerializeField] private PlayerDamageReceiver _playerDamageReceiver;
    [SerializeField] private WeaponController _weaponController;
    [SerializeField] private UIController _uiController;
    #endregion
    [Header("Data")]
    [SerializeField] private float _weaponExistDuration;
    [SerializeField] private float _health;
    #region DATA
    private PlayerColor _playerColor;

    private Vector2 _moveInput;
    private Vector3 _mousePositon;
    private Vector2 _direction;

    private CountdownTimer _weaponExistTimer;
    #endregion
    protected override void Awake()
    {
        base.Awake();
        _playerColor = PlayerColor.White;

        _playerDamageReceiver.DeathAction += HandleDeath;

        _weaponExistTimer = new CountdownTimer(_weaponExistDuration);
        _weaponExistTimer.Stop();
    }

    protected override void Start()
    {
        base.Start();
        InputReader.Instance.ChangeWeaponAction += _weaponController.ChangeNextWeapon;

        _playerDamageReceiver.SetMaxHP(_health);
        _uiController.SetMaxHealth((int)_health);
    }

    protected override void Update()
    {
        base.Update();
        _playerAnimator.Render(_playerColor);

        if (InputReader.Instance.IsFiring)
        {
            _weaponController.FireCurrentWeapon(this);
        }

        _moveInput = InputReader.Instance.Direction;
        _mousePositon = InputReader.Instance.AimPoint;

        FaceToDirection();

        _weaponExistTimer.Tick(Time.deltaTime);
/*        _uiController.SetAmmo((float)_weaponExistTimer);*/

        if (_weaponExistTimer.IsFinished)
        {
            _weaponExistTimer.Reset();
            _weaponExistTimer.Stop();

            ChangeWeapon(PlayerColor.White);
        }
    }

    protected override void FixedUpdate()
    {
        Locomotion(_moveInput);
    }

    public void ChangeWeapon(PlayerColor color)
    {
        _playerColor = color;
        _playerAnimator.Render(_playerColor);
        _weaponController.ChangeWeapon(color);

        _weaponExistTimer.Start();
    }

    private void FaceToDirection()
    {
        _direction = (_mousePositon - transform.position).normalized;
        if (_direction.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.right = _direction;
        }
        else if (_direction.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.right = -_direction;
        }
    }

    public override void AddForce(float force, Vector2 direction)
    {
        LockForce();
        base.AddForce(force, direction);
    }

    public override void OverideForce(float force, Vector2 direction, float timer = -1)
    {
        base.OverideForce(force, direction, timer);
    }

    public override void ResetForce()
    {
        UnlockForce();
        base.ResetForce();
    }

    public void HandleDeath()
    {
        Time.timeScale = 0;
    }
}
[Serializable]
public enum PlayerColor
{
    Red,
    Blue,
    Green,
    Yellow,
    Orange,
    Purple,
    White
}
