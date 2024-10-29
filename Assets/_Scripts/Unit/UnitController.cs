using TMPro;
using UnityEngine;
using Utilities;

public class UnitController : MonoBehaviour
{
    [Header("Unit Setting")]
    [SerializeField] protected Rigidbody2D _rb;
    [SerializeField] private float _forceTime;

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

}
