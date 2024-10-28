using TMPro;
using UnityEngine;
using Utilities;

public class UnitController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _forceTime;

    private CountdownTimer _forceTimer;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        _forceTimer = new CountdownTimer(_forceTime);
        _forceTimer.OnTimerStop += ResetForce;
        _forceTimer.Stop();

    }

    protected virtual void Update()
    {
        if (_rb.linearVelocity != Vector2.zero && !_forceTimer.IsRunning)
        {
            _forceTimer.Start();
        }

        _forceTimer.Tick(Time.deltaTime);
    }

    protected virtual void ResetForce()
    {
        _rb.linearVelocity = Vector2.zero;
        _forceTimer.Stop();
    }

    public virtual void AddForce(float force, Vector2 direction)
    {
        _rb.AddForce(direction * force, ForceMode2D.Impulse);
        _forceTimer.Start();
        
    }

}
