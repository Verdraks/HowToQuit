using UnityEngine;
using UnityEngine.Events;

public class BreakableDeath : MonoBehaviour, IBreakable
{
    [Header("Output")] 
    [SerializeField] private UnityEvent OnBreak;
    
    public void Break()
    {
        OnBreak.Invoke();
        Destroy(gameObject);
    }
}