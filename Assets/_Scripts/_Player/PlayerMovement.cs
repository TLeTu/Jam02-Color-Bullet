using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region COMPONENTS
    public Rigidbody2D _rb { get; private set; }
    #endregion

    #region STATE PARAMETERS
    private Vector2 _moveInput;
    #endregion

    #region DATA
    public float _runMaxSpeed = 9.5f;
    public float _runAccelAmount = 50f;
    public float _runDeccelAmount = 50f;
    private Vector3 _mousePositon;
    private Vector2 _direction;
    #endregion


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        #region INPUT HANDLER
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");
        _moveInput = _moveInput.normalized;
        _mousePositon = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        #endregion

        #region MODEL MOVEMENT
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

        #endregion
    }

    private void FixedUpdate()
    {
        Run();
    }

    #region MOVEMENT METHODS

    private void Run()
    {
        #region NO TOUCHING
        float targetSpeedX = _moveInput.x * _runMaxSpeed;
        float targetSpeedY = _moveInput.y * _runMaxSpeed;

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

    #endregion
}
