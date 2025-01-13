using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class SSO_Achievement : ScriptableObject
{
    [Header("Settings")]
    public string achievementName;
    public string achievementDescription;
    public Sprite achievementIcon;
    public string AchievementId { get => achievementId; private set => achievementId = value; }
    private string achievementId;
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
    
    # if UNITY_EDITOR
    [ContextMenu("SetIDAchievement")] 
    public void SetIDAchievement()
    {
        AchievementId = Guid.NewGuid().ToString();
        ValidateUniqueID();
    }
    
    private void ValidateUniqueID()
    {
        var allAchievements = UnityEditor.AssetDatabase.FindAssets("t:SSO_Achievement")
            .Select(guid => UnityEditor.AssetDatabase.LoadAssetAtPath<SSO_Achievement>(
                UnityEditor.AssetDatabase.GUIDToAssetPath(guid)))
            .Where(achievement => achievement != null);

        if (allAchievements.Any(a => a != this && a.AchievementId == this.AchievementId))
        {
            Debug.LogError($"Duplicate Achievement ID detected: {AchievementId} in {name}. Regenerating a new ID...");
            SetIDAchievement();
        }
    }
    #endif
    
}