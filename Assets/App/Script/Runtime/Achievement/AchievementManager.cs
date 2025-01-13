using System;
using System.Collections.Generic;
using BT.ScriptablesObject;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.Impl;

public class AchievementManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private SSO_Achievement[] achievements;
    [Header("References")]
    [SerializeField] private RSO_ContentSaved rsoContentSaved;
    [Header("Output")]
    [SerializeField] private RSE_TriggerAchievement rseTriggerAchievement;
    [SerializeField] private RSO_AchievementComplete rsoAchievementComplete;

    private void Start()
    {
        for (var index = 0; index < achievements.Length; index++)
        {
            if (rsoContentSaved.Value.achievementsIdCompleted.Count == 0)
            {
                ConnectAchievementNotifiers(index);
                continue;
            }
            bool find = false;
            foreach (var id in rsoContentSaved.Value.achievementsIdCompleted)
            {
                if (id == achievements[index].AchievementId){ find = true;}
                if (!find) ConnectAchievementNotifiers(index);
            }
        }
        
        if (rsoAchievementComplete.Value != null)
        {
            rseTriggerAchievement.Call();
            rsoAchievementComplete.Value = null;
        }
    }


    private void ConnectAchievementNotifiers(int i)
    {
        achievements[i].BindEventCheck();
        achievements[i].OnAchievementComplete += OnAchievementCompleted;
    }
    
    private void OnAchievementCompleted(SSO_Achievement achievement)
    {
        rsoContentSaved.Value.achievementsIdCompleted.Add(achievement.AchievementId);
        rsoAchievementComplete.Value = achievement;
        if (!achievement.achievementOnReaload) rseTriggerAchievement.Call();
        achievement.UnbindEventCheck();
        achievement.OnAchievementComplete -= OnAchievementCompleted;
    }

    private void OnDestroy()
    {
        if (rsoAchievementComplete.Value && !rsoAchievementComplete.Value.achievementOnReaload) rsoAchievementComplete.Value = null;
    }
}