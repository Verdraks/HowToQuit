using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CircleController : PhysicController
{
    [Header("Settings")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float distanceDetectionMur = 0.5f;
    [SerializeField] private float speed = 3f;
    
    private bool _closeToWall;
    
    private void Update()
    {
        CheckCloseToWall();
    }

    private void CheckCloseToWall()
    {
        RaycastHit2D[] results = new RaycastHit2D[1];
        _closeToWall = Physics2D.RaycastNonAlloc(rb.position, Vector2.zero, results, distanceDetectionMur, layerMask) >=
                       1;
    }

    protected override void OnInputMove(Vector2 value)
    {
        base.OnInputMove(value);
        if (_closeToWall && value.y != 0)
        {
            rb.velocity = new (rb.velocity.x,speed* value.y);
        }
    }
    
}