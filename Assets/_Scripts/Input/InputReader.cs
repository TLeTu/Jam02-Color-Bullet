using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : Singleton<InputReader>
{
    [SerializeField] private InputActionReference _aimAction;
    [SerializeField] private InputActionReference _fireAction;

    public Vector2 AimPoint { get; private set; }
    public bool IsFiring { get; private set; }

    private void OnEnable()
    {
        _aimAction.action.Enable();
        _fireAction.action.Enable();
    }

    private void OnDisable()
    {
        _aimAction.action.Disable();
        _fireAction.action.Disable();
    }

    private void Start()
    {
        _aimAction.action.performed += OnAim;

        _fireAction.action.started += _ => IsFiring = true;
        _fireAction.action.canceled += _ => IsFiring = false;
    }

    private void OnAim(InputAction.CallbackContext context)
    {
        AimPoint = context.ReadValue<Vector2>();
        //change to world point
        AimPoint = Camera.main.ScreenToWorldPoint(AimPoint);
    }

}
