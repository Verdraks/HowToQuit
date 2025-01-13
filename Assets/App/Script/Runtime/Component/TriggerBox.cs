using BT.ScriptablesObject;
using UnityEngine;
using UnityEngine.Serialization;

public class TriggerBox : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string triggerTag;
    
    [Header("Output")] 
    [SerializeField] private RuntimeScriptableEvent rse;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(triggerTag)) rse.Call();
    }
    
}