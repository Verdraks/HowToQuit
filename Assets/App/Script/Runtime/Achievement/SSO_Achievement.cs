using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class SSO_Achievement : ScriptableObject
{
    [Header("Settings")]
    public string achievementName;
    public string achievementDescription;
    public Sprite achievementIcon;
    public string achievementId;
    public bool achievementOnReaload;

    public Action<SSO_Achievement> OnAchievementComplete;
    
    public abstract void BindEventCheck();
    public abstract void UnbindEventCheck();

    public void OnEventCheckAchievement()
    {
        if (CheckCondition())
        {
            UnlockAchievement();
        }
    }
    protected abstract bool CheckCondition();
    
    private void UnlockAchievement()
    {
        OnAchievementComplete?.Invoke(this);
    }
}