using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region COMPONENTS
    public Rigidbody2D RB { get; private set; }
    #endregion

    #region STATE PARAMETERS
    private Vector2 _moveInput;
    #endregion

    #region DATA
    public float runMaxSpeed = 9.5f;
    public float runAccelAmount = 50f;
    private Vector3 mousePositon;
    private Vector2 direction;
    #endregion


    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        #region INPUT HANDLER
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");
        _moveInput = _moveInput.normalized;
        mousePositon = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        #endregion

        #region MODEL MOVEMENT
        direction = (mousePositon - transform.position).normalized;
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.right = direction;
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.right = -direction;
        }
        

        // the character is a square, i want to rotate the left side of the square to face the mouse like transform.up = direction;
        

        #endregion
    }

    private void FixedUpdate()
    {
        Run();
    }

    #region MOVEMENT METHODS
    private void Run()
    {
        float targetSpeedX = _moveInput.x * runMaxSpeed;
        float targetSpeedY = _moveInput.y * runMaxSpeed;

        Vector2 targetVelocity = new Vector2(targetSpeedX, targetSpeedY);

        RB.linearVelocity = Vector2.Lerp(RB.linearVelocity, targetVelocity, runAccelAmount * Time.fixedDeltaTime);
    }

    #endregion
}
