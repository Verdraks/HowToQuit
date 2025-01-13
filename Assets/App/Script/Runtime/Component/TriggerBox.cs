using BT.ScriptablesObject;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class TriggerBox : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string triggerTag;
    
    [Header("Output")] 
    [SerializeField] private UnityEvent onTriggerEnter;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(triggerTag)) onTriggerEnter.Invoke();
    }
    
}