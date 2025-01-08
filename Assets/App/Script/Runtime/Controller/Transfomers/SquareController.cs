using Unity.VisualScripting;
using UnityEngine;
public class SquareController : PhysicController
{
    [Header("Settings")] 
    [SerializeField] private float sizeMin = 0.5f;
    [SerializeField] private float sizeMax = 1.5f;
    [SerializeField] private float scaleIncrement = 0.5f;

    [Header("References")] 
    [SerializeField] private Transform root;
    [Header("Input")]
    [SerializeField] private RSE_InputResize rseInputResize;

    protected override void OnEnable()
    {
        base.OnEnable();
        rseInputResize.action += OnInputResize;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        rseInputResize.action -= OnInputResize;
    }
    
    private void OnInputResize(int value)
    {
        if (value != 0)
        {
            float scaleFactor = Mathf.Clamp(root.localScale.x + scaleIncrement * value, sizeMin, sizeMax);
            print(scaleFactor);
            Vector3 newLocalScale = new(scaleFactor, scaleFactor,1);
            root.localScale = newLocalScale;
        }
    }
    
}