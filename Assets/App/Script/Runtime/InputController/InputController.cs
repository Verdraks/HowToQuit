using System;
using BT.ScriptablesObject;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InputController : MonoBehaviour
{
    [Header("Output")]
    [SerializeField] private RSE_InputLook rseInputLook;
    [SerializeField] private RSE_InputAbility rseInputAbility;
    [SerializeField] private RSE_InputMove rseInputMove;
    [SerializeField] private RSE_InputSwapController rseInputSwapController;
    [SerializeField] private RSE_InputJump rseInputJump;
    [SerializeField] private RSE_InputSprint rseInputSprint;
    [SerializeField] private RSE_InputAchievement rseInputAchievement;
    [Space(10)] 
    [SerializeField] private RSE_AltF4 rseAltF4;

    private InputActionController _inputActionController;

    private void Awake() => _inputActionController = new InputActionController();

    private bool _controllerInputEnabled = true;
    
    private void OnEnable()
    {
        _inputActionController.Enable();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _inputActionController.Player.Ability.performed += OnInputAbilityPerformed;
        _inputActionController.Player.SwapController.performed += OnInputSwapControllerPerformed;
        _inputActionController.Player.Jump.performed += OnInputJumpPerformed;
        _inputActionController.Player.Look.performed += OnInputLookPerformed;
        _inputActionController.Menu.Achievement.performed += OnInputAchievementPerformed;
        
        _inputActionController.Player.Sprint.started += OnInputSprintCall;
        _inputActionController.Player.Sprint.canceled += OnInputSprintCall;
    }

    private void OnDisable()
    {
        _inputActionController.Player.Ability.performed -= OnInputAbilityPerformed;
        _inputActionController.Player.SwapController.performed -= OnInputSwapControllerPerformed;
        _inputActionController.Player.Jump.performed -= OnInputJumpPerformed;
        _inputActionController.Player.Look.performed -= OnInputLookPerformed;
        
        _inputActionController.Player.Sprint.started -= OnInputSprintCall;
        _inputActionController.Player.Sprint.canceled -= OnInputSprintCall;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _inputActionController.Disable();
    }

    private void Update()
    {
        OnInputMovePerformed();
        CheckSecretAchievement();
    }

    private void CheckSecretAchievement()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.F4))
            {
                rseAltF4.Call();
            }
        }
    }

    private void OnInputSprintCall(InputAction.CallbackContext context)
    {
        rseInputSprint.Call(context.started);
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

    private void OnInputAchievementPerformed(InputAction.CallbackContext context)
    {
        if (_controllerInputEnabled) _inputActionController.Player.Disable();
        else _inputActionController.Player.Enable();
        _controllerInputEnabled  = !_controllerInputEnabled;
        Cursor.visible = !_controllerInputEnabled;
        Cursor.lockState = _controllerInputEnabled? CursorLockMode.Locked : CursorLockMode.Confined;
        rseInputAchievement.Call();
    }

    private void OnInputSwapControllerPerformed(InputAction.CallbackContext context)
    {
        var value = (int)context.ReadValue<float>();
        rseInputSwapController.Call(value);
    }
    
}