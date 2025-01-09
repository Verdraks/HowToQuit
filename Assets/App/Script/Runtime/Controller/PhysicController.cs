using System;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class PhysicController : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private float jumpCooldown = 0.5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private RSO_CameraTransform rsoCameraTransform;

    [Header("Input")]
    [SerializeField] private RSE_InputMove rseInputMove;
    [SerializeField] private RSE_InputJump rseInputJump;
    [SerializeField] private RSE_InputAbility rseInputAbility;

    private bool _canJump = true;
    private Vector3 _movement;
    private Quaternion _currentRotationVelocity;
    private float _currentSpeed;
    private float _velocity;

    private const float ZeroF = 0.0f;
    
    
    protected virtual void OnEnable()
    {
        rseInputMove.action += OnInputMove;
        rseInputAbility.action += OnInputAbility;
        rseInputJump.action += OnInputJump;
    }

    protected virtual void OnDisable()
    {
        rseInputMove.action -= OnInputMove;
        rseInputAbility.action -= OnInputAbility;
        rseInputJump.action -= OnInputJump;
    }

    protected virtual void OnInputMove(Vector2 value)
    {
        _movement = new Vector3(value.x, 0f, value.y);
    }

    protected void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        // Rotate movement direction to match camera rotation
        var adjustedDirection = Quaternion.AngleAxis(rsoCameraTransform.Value.Rotation.eulerAngles.y, Vector3.up) * _movement;
            
        if (adjustedDirection.magnitude > ZeroF) {
            HandleRotation(adjustedDirection);
            HandleHorizontalMovement(adjustedDirection);
            SmoothSpeed(adjustedDirection.magnitude);
        } else {
            SmoothSpeed(ZeroF);
                
            // Reset horizontal velocity for a snappy stop
            rb.velocity = new Vector3(ZeroF, rb.velocity.y, ZeroF);
        }
    }

    void HandleHorizontalMovement(Vector3 adjustedDirection) {
        // Move the player
        Vector3 velocity = adjustedDirection * (speed * Time.fixedDeltaTime);
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
    }

    void HandleRotation(Vector3 adjustedDirection) {
        // Adjust rotation to match movement direction
        var targetRotation = Quaternion.LookRotation(adjustedDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void SmoothSpeed(float value) {
        _currentSpeed = Mathf.SmoothDamp(_currentSpeed, value, ref _velocity, smoothTime);
    }
    
    protected virtual void OnInputJump()
    {
        if (!_canJump) return;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        _canJump = false;
        StartCoroutine(Utils.Delay(jumpCooldown, () => _canJump = true));
    }
    
    protected abstract void OnInputAbility();
    
    public void Teleport(Vector3 position, Quaternion quaternion)
    {
        transform.position = position;
        rb.MovePosition(position);
        rb.MoveRotation(quaternion);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}