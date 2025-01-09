using System;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class PhysicController : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpCooldown = 0.5f;

    [Header("References")]
    [SerializeField] private Rigidbody rb;

    [Header("Input")]
    [SerializeField] private RSE_InputMove rseInputMove;
    [SerializeField] private RSE_InputJump rseInputJump;
    [SerializeField] private RSE_InputAbility rseInputAbility;

    private bool _canJump = true;
    
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
        Vector3 input = new Vector3(value.x, 0, value.y);
        rb.AddForce(speed * input, ForceMode.Force);
    }

    protected virtual void OnInputJump()
    {
        if (!_canJump) return;
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        _canJump = false;
        StartCoroutine(Utils.Delay(jumpCooldown, () => _canJump = true));
    }
    
    public void Teleport(Vector3 position, Quaternion quaternion)
    {
        transform.position = position;
        rb.Move(position, quaternion);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    protected abstract void OnInputAbility();
}