using UnityEngine;
using UnityEngine.Events;

public abstract class PhysicController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected SSO_ControllerStat ssoControllerStat;
    
    [Header("References")]
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected RSO_CameraTransform rsoCameraTransform;

    [Header("Input")]
    [SerializeField] private RSE_InputMove rseInputMove;
    [SerializeField] private RSE_InputJump rseInputJump;
    [SerializeField] private RSE_InputSprint rseInputSprint;
    [SerializeField] private RSE_InputAbility rseInputAbility;

    [Header("Output")]
    [SerializeField] protected UnityEvent playerOnGround;
    [SerializeField] protected UnityEvent playerOnAir;
    
    private float _currentSpeed;
    protected bool _isGrounded;
    private bool _canJump = true;
    protected bool _coyoteeTimerRunning;
    private Vector3 _moveDirection = Vector3.one;
    private static readonly float ZeroF = 0f;
    

    protected virtual void Start() => _currentSpeed = ssoControllerStat.speed;
    
    protected virtual void OnEnable()
    {
        rseInputMove.action += OnInputMove;
        rseInputJump.action += OnJumpInput;
        rseInputSprint.action += OnInputSprint;
        rseInputAbility.action += OnInputAbility;
    }

    protected virtual void OnDisable()
    {
        rseInputMove.action -= OnInputMove;
        rseInputJump.action -= OnJumpInput;
        rseInputSprint.action -= OnInputSprint;
        rseInputAbility.action -= OnInputAbility;
    }

    protected virtual void Update()
    {
        CheckGrounded();
    }

    protected virtual void FixedUpdate()
    {
        Move();
        ApplyGravity();
    }

    protected virtual void OnInputMove(Vector2 input)
    {
        _moveDirection = new Vector3(input.x, 0, input.y).normalized;
    }

    protected virtual void OnJumpInput()
    {
        if (!_canJump) return;
        if (_isGrounded) Jump();
        else if (_coyoteeTimerRunning) Jump();
    }
    
    protected virtual void OnInputSprint(bool isSprinting)
    {
        _currentSpeed = isSprinting ? ssoControllerStat.speed * ssoControllerStat.sprintMultiplier : ssoControllerStat.speed;
    }    
    
    protected virtual void CheckGrounded()
    {
        RaycastHit hit;
        var wasGrounded = _isGrounded;
        _isGrounded = Physics.RaycastNonAlloc(new Ray(transform.position + ssoControllerStat.positionCheckOffset, Vector3.down), new RaycastHit[1], ssoControllerStat.distanceCheck) > 0;

        if (!_isGrounded && wasGrounded)
        {
            _coyoteeTimerRunning = true;
            StartCoroutine(Utils.Delay(ssoControllerStat.coyoteeTime,()=> _coyoteeTimerRunning = false));
        }
        if (!wasGrounded && _isGrounded) playerOnGround?.Invoke();
        else if(!_isGrounded && wasGrounded) playerOnAir?.Invoke();
    }
    

    protected virtual void Move()
    {
        var adjustedDirection = Quaternion.AngleAxis(rsoCameraTransform.Value.Rotation.eulerAngles.y, Vector3.up) * _moveDirection;

        if (adjustedDirection.magnitude > 0.1f)
        {
            HandleRotation(adjustedDirection);
            Vector3 targetVelocity = adjustedDirection * _currentSpeed;
            rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
        }
        else
        {
            rb.velocity = new Vector3(ZeroF, rb.velocity.y, ZeroF);
        }
    }

    protected virtual void HandleRotation(Vector3 adjustedDirection)
    {
        var targetRotation = Quaternion.LookRotation(adjustedDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, ssoControllerStat.rotationSpeed * Time.deltaTime);
    }

    protected virtual void ApplyGravity()
    {
        if (!_isGrounded)
        {
            rb.velocity += Vector3.down * (ssoControllerStat.fallAcceleration * Time.fixedDeltaTime);
        }
    }
    
    protected virtual void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, ssoControllerStat.jumpForce, rb.velocity.z);
        _canJump = false;
        StartCoroutine(Utils.Delay(ssoControllerStat.jumpCooldown, () => _canJump = true));
    }
 
    protected abstract void OnInputAbility();
    
    public virtual void Teleport(Vector3 position, Quaternion rotation)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.position = position;
        transform.position = position;
        transform.rotation = rotation;
    }

}