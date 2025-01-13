using System;
using System.Collections.Generic;
using BT.ScriptablesObject;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.Impl;
using Task = System.Threading.Tasks.Task;

public class AchievementManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private SSO_Achievement[] achievements;
    [Header("References")]
    [SerializeField] private RSO_ContentSaved rsoContentSaved;
    [SerializeField] private RSO_AchievementRuntimeData rsoAchievementRuntimeData;
    [Header("Output")]
    [SerializeField] private RSE_TriggerAchievement rseTriggerAchievement;
    
    private async void Start()
    {
        rsoAchievementRuntimeData.Value = new AchievementRuntimeData
        {
            achievements = achievements,
            achievementsCompleted = new bool[achievements.Length],
            lastAchievementCompletedID = rsoContentSaved.Value.lastAchievementCompletedId
        };
        
        for (var index = 0; index < achievements.Length; index++)
        {
            if (rsoContentSaved.Value.achievementsIdCompleted.Count == 0)
            {
                ConnectAchievementNotifiers(index);
                rsoAchievementRuntimeData.Value.achievementsCompleted[index] = false;
                continue;
            }
            bool find = false;
            foreach (var id in rsoContentSaved.Value.achievementsIdCompleted)
            {
                if (id == achievements[index].AchievementId)
                {
                    find = true;
                    break;
                }
            }
            if (!find) ConnectAchievementNotifiers(index);
            rsoAchievementRuntimeData.Value.achievementsCompleted[index] = find;
        }

        await Task.Delay(100);
        
        if (rsoAchievementRuntimeData.Value.lastAchievementCompletedID != "")
        {
            rseTriggerAchievement.Call();
            rsoAchievementRuntimeData.Value.lastAchievementCompletedID = "";
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
        
        rsoAchievementRuntimeData.Value.lastAchievementCompletedID = achievement.AchievementId;
        
        if (!achievement.achievementOnReaload) rseTriggerAchievement.Call();
        achievement.UnbindEventCheck();
        achievement.OnAchievementComplete -= OnAchievementCompleted;
    }

    private void OnDestroy()
    {
        rsoContentSaved.Value.lastAchievementCompletedId = rsoAchievementRuntimeData.Value.lastAchievementCompletedID;
        rsoAchievementRuntimeData.Value = null;
    }
}