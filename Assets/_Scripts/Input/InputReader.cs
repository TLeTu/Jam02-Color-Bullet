using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : Singleton<InputReader>
{
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _aimAction;
    [SerializeField] private InputActionReference _fireAction;
    [SerializeField] private InputActionReference _changeWeaponsAction;

    public Vector2  Direction { get; private set; }
    public Vector2 AimPoint { get; private set; }
    public bool IsFiring { get; private set; }

    public Action ChangeWeaponAction;
    public Action FireAction;

    private void OnEnable()
    {
        ActiveInput();
    }

    private void OnDisable()
    {
        DeactiveInput();
    }

    public void ActiveInput()
    {
        _moveAction.action.Enable();
        _aimAction.action.Enable();
        _fireAction.action.Enable();
        _changeWeaponsAction.action.Enable();
    }

    public void DeactiveInput()
    {
        _moveAction.action.Disable();
        _aimAction.action.Disable();
        _fireAction.action.Disable();
        _changeWeaponsAction.action.Disable();
    }

    private void Start()
    {
        _moveAction.action.performed += OnMotion;
        _moveAction.action.canceled += _ => Direction = Vector2.zero;   

        _aimAction.action.performed += OnAim;

        _fireAction.action.started += _ => IsFiring = true;
        _fireAction.action.canceled += _ => IsFiring = false;

        _changeWeaponsAction.action.performed += ChangeWeapon;
    }

    private void OnMotion(InputAction.CallbackContext context)
    {
        Direction = context.ReadValue<Vector2>();
    }

    private void ChangeWeapon(InputAction.CallbackContext context)
    {
        ChangeWeaponAction?.Invoke();
    }

    private void OnAim(InputAction.CallbackContext context)
    {
        AimPoint = context.ReadValue<Vector2>();
        //change to world point
        AimPoint = Camera.main.ScreenToWorldPoint(AimPoint);
    }

}
