using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RSE_InputFire rseInputFire;
    [SerializeField] private RSE_InputGrab rseInputGrab;
    [SerializeField] private RSE_InputMove rseInputMove;
    [SerializeField] private RSE_InputTransformer rseInputTransformer;
    

    private InputAction_Controller _inputActionController;

    private void Awake()
    {
        _inputActionController = new InputAction_Controller();
    }

    private void OnEnable()
    {
        _inputActionController.Enable();
        _inputActionController.Player.Fire.performed += OnFirePerformed;
        _inputActionController.Player.Grab.performed += OnGrabPerformed;
        _inputActionController.Player.Transformer.performed += OnTransformerPerformed;
    }

    

    private void OnDisable()
    {
        _inputActionController.Player.Fire.performed -= OnFirePerformed;
        _inputActionController.Player.Grab.performed -= OnGrabPerformed;
        _inputActionController.Player.Transformer.performed -= OnTransformerPerformed;
        _inputActionController.Disable();
    }

    private void Update()
    {
        OnMovePerformed();
    }

    private void OnMovePerformed()
    {
        var value =_inputActionController.Player.Move.ReadValue<Vector2>();
        if (value == Vector2.zero) return;
        rseInputMove.Call(value);
    }

    private void OnFirePerformed(InputAction.CallbackContext callbackContext)
    {
        rseInputFire.Call();
    }

    private void OnGrabPerformed(InputAction.CallbackContext callbackContext)
    {
        rseInputGrab.Call();
    }
    
    private void OnTransformerPerformed(InputAction.CallbackContext obj)
    {
        var value = (int)obj.ReadValue<float>();
        if (value == 0) return;
        rseInputTransformer.Call(value);
    }

}