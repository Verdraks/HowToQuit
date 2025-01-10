using System;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class PhysicController : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private float speed = 5f;
    [SerializeField] private float speedSprinting = 0.5f;
    
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpCooldown = 0.5f;
    [SerializeField] private float distanceCheckGround = 0.2f;
    [SerializeField] private Vector3 offsetCheckGround;
    [SerializeField] private LayerMask groundLayer;
    
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private float gravityFallingMultiplier = 2f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private RSO_CameraTransform rsoCameraTransform;

    [Header("Input")]
    [SerializeField] private RSE_InputMove rseInputMove;
    [SerializeField] private RSE_InputJump rseInputJump;
    [SerializeField] private RSE_InputAbility rseInputAbility;
    [SerializeField] private RSE_InputSprint rseInputSprint;

    private bool _canJump = true;
    private Vector3 _movement;
    private Quaternion _currentRotationVelocity;
    private float _currentSpeed;
    private float _velocity;
    private bool _isSprinting;
    private bool _isGrounded;

    private RaycastHit[] _hit = new RaycastHit[1];
    private Ray _ray = new(Vector3.zero, Vector3.down);
    private float _jumpVelocity;
    
    private const float ZeroF = 0.0f;
    
    
    protected virtual void OnEnable()
    {
        rseInputMove.action += OnInputMove;
        rseInputAbility.action += OnInputAbility;
        rseInputJump.action += OnInputJump;
        rseInputSprint.action += OnInputSprint;
    }


    protected virtual void OnDisable()
    {
        rseInputMove.action -= OnInputMove;
        rseInputAbility.action -= OnInputAbility;
        rseInputJump.action -= OnInputJump;
        rseInputSprint.action -= OnInputSprint;
    }
    
    private void OnInputSprint(bool value)
    {
        _isSprinting = value;
    }

    protected virtual void OnInputMove(Vector2 value)
    {
        _movement = new Vector3(value.x, 0f, value.y);
    }

    protected void FixedUpdate()
    {
        CheckTouchGround();
        HandleMovement();
    }

    private void CheckTouchGround()
    {
        _ray.origin = rb.position + offsetCheckGround;
        _isGrounded = Physics.RaycastNonAlloc(_ray, _hit, distanceCheckGround, groundLayer) > 0;
    }

    void HandleMovement()
    {
        if (!_isGrounded) HandleFallingGravity();
        
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

    private void HandleFallingGravity()
    {
        _jumpVelocity += Physics.gravity.y * gravityFallingMultiplier * Time.fixedDeltaTime;
        rb.velocity = new Vector3(rb.velocity.x, _jumpVelocity, rb.velocity.z);
    }

    void HandleHorizontalMovement(Vector3 adjustedDirection) {
        // Move the player
        Vector3 velocity = adjustedDirection * ((_isSprinting ? speedSprinting : speed ) * Time.fixedDeltaTime);
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
        if (!_isGrounded || !_canJump) return;
        _jumpVelocity = jumpForce;
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

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_ray.origin, _ray.origin + _ray.direction * distanceCheckGround);
    }
}