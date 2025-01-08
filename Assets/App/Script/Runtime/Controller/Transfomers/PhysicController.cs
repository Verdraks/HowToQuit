using UnityEngine;
public abstract class PhysicController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected float walkSpeed = 2.0f;
    
    [Header("References")]
    [SerializeField] protected Rigidbody2D rb;
    [Header("Input")]
    [SerializeField] protected RSE_InputMove rseInputMove;
    
    protected  virtual void OnEnable()
    {
        rseInputMove.action += OnInputMove;
    }

    protected virtual void OnDisable()
    {
        rseInputMove.action -= OnInputMove;
    }

    protected virtual void OnInputMove(Vector2 value)
    {
        value.y = 0;
        rb.AddForce(value * walkSpeed);
    }
    
}