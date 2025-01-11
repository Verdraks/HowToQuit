using UnityEngine;

public abstract class PhysicController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private SSO_ControllerStat ssoControllerStat;
    
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private RSO_CameraTransform rsoCameraTransform;

    [Header("Input")]
    [SerializeField] private RSE_InputMove rseInputMove;
    [SerializeField] private RSE_InputJump rseInputJump;
    [SerializeField] private RSE_InputSprint rseInputSprint;


    private float currentSpeed;
    private bool _isGrounded;
    private bool _canJump = true;
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
        if (!_canJump) return;
        if (_isGrounded) Jump();
        else if (_coyoteeTimerRunning) Jump();
        
    }
    
    private void OnInputSprint(bool isSprinting)
    {
        currentSpeed = isSprinting ? ssoControllerStat.speed * ssoControllerStat.sprintMultiplier : ssoControllerStat.speed;
    }    
    
    private void CheckGrounded()
    {
        RaycastHit hit;
        var wasGrounded = _isGrounded;
        _isGrounded = Physics.RaycastNonAlloc(new Ray(transform.position, Vector3.down), new RaycastHit[1], ssoControllerStat.distanceCheck) > 0;

        if (!_isGrounded && wasGrounded)
        {
            _coyoteeTimerRunning = true;
            StartCoroutine(Utils.Delay(ssoControllerStat.coyoteeTime,()=> _coyoteeTimerRunning = false));
        }
    }
    

    private void Move()
    {
        var adjustedDirection = Quaternion.AngleAxis(rsoCameraTransform.Value.Rotation.eulerAngles.y, Vector3.up) * _moveDirection;

        if (adjustedDirection.magnitude > ZeroF)
        {
            HandleRotation(adjustedDirection);
            Vector3 targetVelocity = adjustedDirection * currentSpeed;
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
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, ssoControllerStat.smoothStopTime * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (!_isGrounded)
        {
            rb.velocity += Vector3.down * (ssoControllerStat.fallAcceleration * Time.fixedDeltaTime);
        }
    }
    
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, ssoControllerStat.jumpForce, rb.velocity.z);
        _canJump = false;
        StartCoroutine(Utils.Delay(ssoControllerStat.jumpCooldown, () => _canJump = true));
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