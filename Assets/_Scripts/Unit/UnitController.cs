using System;
using TMPro;
using UnityEngine;
using Utilities;

public class UnitController : MonoBehaviour
{
    [Header("Unit Setting")]
    [SerializeField] protected Rigidbody2D _rb;
    [SerializeField] private float _forceTime;

    [Header("Movement")]
    [SerializeField] private float _runMaxSpeed = 5f;
    [SerializeField] private float _runAccelAmount = 50f;
    [SerializeField] private float _runDeccelAmount = 50f;

    private CountdownTimer _forceTimer;

    protected bool _lockForce;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        _forceTimer = new CountdownTimer(_forceTime);
        _forceTimer.OnTimerStop += ResetForce;
        _forceTimer.Stop();

        _lockForce = false;
    }

    protected virtual void Update()
    {
        if (_rb.linearVelocity != Vector2.zero && !_forceTimer.IsRunning)
        {
            _forceTimer.Start();
        }

        _forceTimer.Tick(Time.deltaTime);
    }

    protected virtual void FixedUpdate()
    {
    }


    protected virtual void ResetForce()
    {
        _rb.linearVelocity = Vector2.zero;
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

    public virtual void OverideForce(float force, Vector2 direction)
    {
        _rb.linearVelocity = Vector2.zero;
        AddForce(force, direction);

        LockForce();
    }

    protected void Locomotion(Vector2 movement)
    {
        if (_lockForce) return;

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
