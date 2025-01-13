using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class FlyerController : PhysicController
{
    [Header("Settings")] 
    [SerializeField] private float abilityDuration = 5f;
    [SerializeField] private float maxDistanceCheckHit;
    [SerializeField] private float flySpeed;
    
    [Header("Ouput")]
    [SerializeField] UnityEvent onAbilityFlyStart;
    [SerializeField] UnityEvent onAbilityFlyEnd;
    
    private bool _isFlying;
    
    protected override void OnInputSprint(bool isSprinting)
    {
        if (_isFlying) return;
        base.OnInputSprint(isSprinting);
    }

    protected override void OnJumpInput()
    {
        if (_isFlying) return;
        base.OnJumpInput();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StopCoroutine(ActivateFlyingMode());
    }

    protected override void OnInputAbility()
    {
        if (!_isFlying && !_isGrounded && _coyoteeTimerRunning) return;
        
        if (!_isFlying)
        {
            playerOnAir.Invoke();
            StartCoroutine(ActivateFlyingMode());
        }
        else
        {
            DisableFlyingMode();
        }
    }

    private IEnumerator ActivateFlyingMode()
    {
        EnableFlyingMode();

        yield return new WaitForSeconds(abilityDuration); // Attendre la fin de la durée de l'abilité

        if (_isFlying) // Désactiver automatiquement si le vol est encore actif
        {
            DisableFlyingMode();
        }
    }
    
    private void EnableFlyingMode()
    {
        _isFlying = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        onAbilityFlyStart.Invoke();
    }

    private void DisableFlyingMode()
    {
        _isFlying = false;
        rb.velocity = Vector3.zero;
        rb.useGravity = true;
        onAbilityFlyEnd.Invoke();
    }

    protected override void Update()
    {
        if (!_isFlying) base.Update();
    }
    
    protected override void FixedUpdate()
    {
        if (_isFlying)
        {
            Fly();
            CheckCollision();
        }
        else
        {
            base.FixedUpdate();
        }
    }

    private void Fly()
    {
        var cameraForward = rsoCameraTransform.Value.Rotation * Vector3.forward;
        cameraForward.Normalize();

        if (cameraForward.sqrMagnitude > 0.01f)
        {
            HandleRotation(cameraForward);
        }

        rb.velocity = cameraForward * flySpeed;
    }

    private void CheckCollision()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxDistanceCheckHit))
        {
            Debug.LogWarning("Collision detected, calling Death()");
        }
    }
}