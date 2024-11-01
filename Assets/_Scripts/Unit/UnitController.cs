using System;
using TMPro;
using UnityEngine;
using Utilities;

public class UnitController : MonoBehaviour
{
    [Header("Unit Base")]
    [SerializeField] protected Rigidbody2D _rb;
    [SerializeField] private float _forceTime;

    [Header("Movement Setting")]
    [SerializeField] private float _runMaxSpeed = 5f;
    [SerializeField] private float _runAccelAmount = 50f;
    [SerializeField] private float _runDeccelAmount = 50f;

    private CountdownTimer _forceTimer;
    private CountdownTimer _lockTimer;

    protected bool _lockForce;

    protected bool _lockMotion;

    public void ChangeRunMaxSpeed(float speed)
    {
        _runMaxSpeed = speed;

    }

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        if (_rb == null)
        {
            //add a rigidbody2d component
            _rb = gameObject.AddComponent<Rigidbody2D>();
        }

    }

    protected virtual void Start()
    {
        _forceTimer = new CountdownTimer(_forceTime);
        _forceTimer.OnTimerStop += ResetForce;
        _forceTimer.Stop();

        _lockTimer = new CountdownTimer(1);
        _lockTimer.Start();
        _lockTimer.Stop();

        _lockForce = false;
        _lockMotion = false;
    }

    protected virtual void Update()
    {
        if (_rb.linearVelocity != Vector2.zero && !_forceTimer.IsRunning)
        {
            _forceTimer.Start();
        }

        _forceTimer.Tick(Time.deltaTime);
        _lockTimer.Tick(Time.deltaTime);

        if (_lockTimer.IsFinished)
        {
            _lockMotion = false;
            _lockTimer.Reset();
            _lockTimer.Stop();
        }

    }

    protected virtual void FixedUpdate()
    {
    }

    public virtual void LockMotion(float time)
    {
        _lockTimer.Reset(time);
        _lockTimer.Start();
        _lockMotion = true;
    }


    public virtual void ResetForce()
    {
        _rb.linearVelocity = Vector2.zero;

        _forceTimer.Reset(_forceTime);

        _forceTimer.Stop();

        UnlockForce();
    }

    public void LockForce() => _lockForce = true;
    public void UnlockForce() => _lockForce = false;

    public virtual void AddForce(float force, Vector2 direction)
    {
        _rb.AddForce(direction * force, ForceMode2D.Impulse);
        _forceTimer.Start();
    }

    public virtual void OverideForce(float force, Vector2 direction, float timer = -1)
    {
        _rb.linearVelocity = Vector2.zero;

        if (timer > 0)
        {
            _forceTimer.Reset(timer);
        }

        AddForce(force, direction);
        LockForce();
    }

    protected void Locomotion(Vector2 movement)
    {
        if (_lockForce) return;
        if (_lockMotion) return;

        #region NO TOUCHING
        float targetSpeedX = movement.x * _runMaxSpeed;
        float targetSpeedY = movement.y * _runMaxSpeed;

        targetSpeedX = Mathf.Lerp(_rb.linearVelocity.x, targetSpeedX, 1);
        targetSpeedY = Mathf.Lerp(_rb.linearVelocity.y, targetSpeedY, 1);

        float accelRateX;
        float accelRateY;

        accelRateX = (Mathf.Abs(targetSpeedX) > 0.01f) ? _runAccelAmount : _runDeccelAmount;
        accelRateY = (Mathf.Abs(targetSpeedY) > 0.01f) ? _runAccelAmount : _runDeccelAmount;

        float speedDifX = targetSpeedX - _rb.linearVelocity.x;
        float speedDifY = targetSpeedY - _rb.linearVelocity.y;

        float movementX = speedDifX * accelRateX;
        float movementY = speedDifY * accelRateY;
        #endregion

        _rb.AddForce(new Vector2(movementX, movementY), ForceMode2D.Force);

    }

}
