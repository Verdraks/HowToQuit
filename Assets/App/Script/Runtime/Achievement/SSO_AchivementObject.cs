using BT.ScriptablesObject;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class SSO_AchivementObject<T> : SSO_Achievement
{
    [Header("Settings")] 
    [SerializeField] protected T valueTarget;
    
    [Header("Input")] 
    [SerializeField] protected RuntimeScriptableObject<T> rso;

    public override void BindEventCheck()
    {
        rso.OnChanged += OnEventCheckAchievement;
    }

    public override void UnbindEventCheck()
    {
        rso.OnChanged -= OnEventCheckAchievement;
    }
}