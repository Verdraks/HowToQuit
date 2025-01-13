using BT.ScriptablesObject;
using UnityEngine;
using UnityEngine.Events;


public class EventReceiver : MonoBehaviour
{
    [Header("Input")] [SerializeField] private RuntimeScriptableEvent rse;
    [Header("Output")] [SerializeField] private UnityEvent eventCallback;

    private void OnEnable() => rse.action += eventCallback.Invoke;
    private void OnDisable() => rse.action -= eventCallback.Invoke;
}

public abstract class EventReceiver<T> : MonoBehaviour
{
    [Header("Input")] [SerializeField] private RuntimeScriptableEvent<T> rse;
    [Header("Output")] [SerializeField] private UnityEvent<T> eventCallback;

    private void OnEnable() => rse.action += eventCallback.Invoke;
    private void OnDisable() => rse.action -= eventCallback.Invoke;
}