using System;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class PhysicController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpCooldown = 0.5f;
    [SerializeField] private float coyoteeTime = 0.2f;
    [SerializeField] private float inputBufferTime = 0.2f;
    [SerializeField] private float fallAcceleration = 2f;
    [SerializeField] private float smoothStopTime = 0.2f;
    [SerializeField] private float rotationSpeed = 360f;

    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private RSO_CameraTransform rsoCameraTransform;

    [Header("Input")]
    [SerializeField] private RSE_InputMove rseInputMove;
    [SerializeField] private RSE_InputJump rseInputJump;
    [SerializeField] private RSE_InputSprint rseInputSprint;

    private bool _isGrounded;
    private bool _canJump = true;
    private bool _bufferTimerRunning;
    private bool _coyoteeTimerRunning;
    private Vector3 _moveDirection;
    private Vector3 _velocity;
    private Vector3 _currentSmoothVelocity;
    private static readonly float ZeroF = 0f;
    

    private void OnEnable()
    {
        rseInputMove.action += OnInputMove;
        rseInputJump.action += OnJumpInput;
        rseInputSprint.action += OnInputSprint;
    }

    private void OnDisable()
    {
        rseInputMove.action -= OnInputMove;
        rseInputJump.action -= OnJumpInput;
        rseInputSprint.action -= OnInputSprint;
    }

    private void Update()
    {
        CheckGrounded();
        HandleJumpBuffer();
    }

    private void FixedUpdate()
    {
        Move();
        ApplyGravity();
    }

    private void OnInputMove(Vector2 input)
    {
        _moveDirection = new Vector3(input.x, 0, input.y).normalized;
    }

    private void OnJumpInput()
    {
        _coyoteeTimerRunning = true;
        StartCoroutine(Utils.Delay(coyoteeTime,()=> _coyoteeTimerRunning = false));
    }
    
    private void OnInputSprint(bool isSprinting)
    {
        speed *= isSprinting ? sprintMultiplier : 1f / sprintMultiplier;
    }    
    
    private void CheckGrounded()
    {
        RaycastHit hit;
        var wasGrounded = _isGrounded;
        _isGrounded = Physics.RaycastNonAlloc(new Ray(transform.position, Vector3.down), new RaycastHit[1], 0.1f) > 0;

        if (!_isGrounded && wasGrounded)
        {
            _bufferTimerRunning = true;
            StartCoroutine(Utils.Delay(inputBufferTime,()=> _bufferTimerRunning = false));
        }
    }
    

    private void Move()
    {
        var adjustedDirection = Quaternion.AngleAxis(rsoCameraTransform.Value.Rotation.eulerAngles.y, Vector3.up) * _moveDirection;

        if (adjustedDirection.magnitude > ZeroF)
        {
            HandleRotation(adjustedDirection);
            Vector3 targetVelocity = adjustedDirection * speed;
            rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
        }
        else
        {
            rb.velocity = new Vector3(ZeroF, rb.velocity.y, ZeroF);
        }
    }

    private void HandleRotation(Vector3 adjustedDirection)
    {
        var targetRotation = Quaternion.LookRotation(adjustedDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, smoothStopTime * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (!_isGrounded)
        {
            rb.velocity += Vector3.down * (fallAcceleration * Time.fixedDeltaTime);
        }
    }

    
    private void HandleJumpBuffer()
    {
        if (!_canJump) return;
        if (_isGrounded) Jump();
        else if (!_isGrounded && _coyoteeTimerRunning) Jump();
        
        // if (Time.time - _lastJumpInputTime <= inputBufferTime &&
        //     Time.time - _lastGroundedTime <= coyoteeTime &&
        //     !_canJump)
        // {
        //     Jump();
        // }
    }
    
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        _canJump = false;
        StartCoroutine(Utils.Delay(jumpCooldown, () => _canJump = true));
    }

    protected abstract void OnInputAbility();
    
    public void Teleport(Vector3 position, Quaternion rotation)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.position = position;
        transform.position = position;
        transform.rotation = rotation;
    }

}