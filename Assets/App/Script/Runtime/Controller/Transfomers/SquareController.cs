using UnityEngine;
public class SquareController : PhysicController
{
    [Header("Parameters")] 
    [SerializeField] private float sizeMin = 0.5f;
    [SerializeField] private float sizeMax = 1.5f;
    [SerializeField] private float scaleIncrement = 0.5f;

    [Header("References")] 
    [SerializeField] private Transform root;

    protected override void OnInputMove(Vector2 value)
    {
        base.OnInputMove(value);
        if (value.x != 0)
        {
            float scaleFactor = Mathf.Clamp(transform.localScale.x + scaleIncrement * value.x, sizeMin, sizeMax);
            Vector3 newLocalScale = new(scaleFactor, scaleFactor,1);
            root.localScale = newLocalScale;
        }
    }
    
}