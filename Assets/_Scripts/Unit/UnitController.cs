using UnityEngine;
using Utilities;

public class UnitController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _forceTime;

    private CountdownTimer _forceTimer;

    private void Start()
    {
        _forceTimer = new CountdownTimer(_forceTime);
        _forceTimer.OnTimerStop += ResetForce;
        _forceTimer.Stop();

    }

    private void Update()
    {
        _forceTimer.Tick(Time.deltaTime);

        Debug.Log(_rb.linearVelocity);

    }

    private void ResetForce()
    {
        _rb.linearVelocity = Vector2.zero;
        _forceTimer.Stop();
    }

    public void AddForce(float force, Vector2 direction)
    {
        _rb.AddForce(direction * force, ForceMode2D.Impulse);
        _forceTimer.Start();
        
    }

}
