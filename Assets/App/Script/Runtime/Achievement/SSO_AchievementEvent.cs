using System.Collections;
using System.Collections.Generic;
using BT.ScriptablesObject;
using UnityEngine;

[CreateAssetMenu(fileName = "SSO_Achievement_", menuName = "ScriptableObject/Achievement/SSO_AchievementEvent")]
public class SSO_AchievementEvent : SSO_Achievement
{
    [Header("Input")] 
    [SerializeField] protected RuntimeScriptableEvent rse;
    
    public override void BindEventCheck()
    {
        rse.action += OnEventCheckAchievement;
    }

    public override void UnbindEventCheck()
    {
        rse.action -= OnEventCheckAchievement;
    }

    protected override bool CheckCondition()
    {
        return true;
    }
}

public abstract class SSO_AchievementEvent<T> : SSO_Achievement
{
    [Header("Settings")] 
    [SerializeField] protected T valueTarget;
    
    [Header("Input")] 
    [SerializeField] protected RuntimeScriptableEvent<T> rse;
    protected T _value;

    public override void BindEventCheck()
    {
        rse.action += CallEventCheck;
    }

    public override void UnbindEventCheck()
    {
        rse.action -= CallEventCheck;
    }

    private void CallEventCheck(T value)
    {
        _value = value;
        OnEventCheckAchievement();
    }
}
