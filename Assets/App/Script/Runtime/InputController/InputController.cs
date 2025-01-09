using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [Header("Output")]
    [SerializeField] private RSE_InputLook rseInputLook;
    [SerializeField] private RSE_InputAbility rseInputAbility;
    [SerializeField] private RSE_InputMove rseInputMove;
    [SerializeField] private RSE_InputSwapController rseInputSwapController;
    [SerializeField] private RSE_InputJump rseInputJump;

    private InputActionController _inputActionController;

    private void Awake() => _inputActionController = new InputActionController();
    
    private void OnEnable()
    {
        _inputActionController.Enable();
        _inputActionController.Player.Ability.performed += OnInputAbilityPerformed;
        _inputActionController.Player.SwapController.performed += OnInputSwapControllerPerformed;
        _inputActionController.Player.Jump.performed += OnInputJumpPerformed;
        _inputActionController.Player.Look.performed += OnInputLookPerformed;
    }

    private void OnDisable()
    {
        _inputActionController.Player.Ability.performed -= OnInputAbilityPerformed;
        _inputActionController.Player.SwapController.performed -= OnInputSwapControllerPerformed;
        _inputActionController.Player.Jump.performed -= OnInputJumpPerformed;
        _inputActionController.Player.Look.performed -= OnInputLookPerformed;
        _inputActionController.Disable();
    }

    private void Update()
    {
        OnInputMovePerformed();
    }

    private void OnInputLookPerformed(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        if (value == Vector2.zero) return;
        rseInputLook.Call(value);
    }
    
    private void OnInputAbilityPerformed(InputAction.CallbackContext context)
    {
        rseInputAbility.Call();
    }

    private void OnInputJumpPerformed(InputAction.CallbackContext context)
    {
        rseInputJump.Call();
    }

    private void OnInputMovePerformed()
    {
        var value = _inputActionController.Player.Move.ReadValue<Vector2>();
        rseInputMove.Call(value);
    }

    private void OnInputSwapControllerPerformed(InputAction.CallbackContext context)
    {
        var value = (int)context.ReadValue<float>();
        rseInputSwapController.Call(value);
    }
    
}