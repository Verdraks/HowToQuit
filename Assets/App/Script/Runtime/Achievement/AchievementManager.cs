using System;
using System.Collections.Generic;
using BT.ScriptablesObject;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Task = System.Threading.Tasks.Task;

public class AchievementManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private SSO_Achievement[] achievements;
    [Header("References")]
    [SerializeField] private RSO_ContentSaved rsoContentSaved;
    [SerializeField] private RSO_AchievementRuntimeData rsoAchievementRuntimeData;
    [Header("Input")]
    [SerializeField] private RSE_Death rseDeath;
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
                if (id == achievements[index].achievementId)
                {
                    find = true;
                    break;
                }
            }
            if (!find) ConnectAchievementNotifiers(index);
            rsoAchievementRuntimeData.Value.achievementsCompleted[index] = find;
        }

        await Task.Delay(100);
        
        if (!string.IsNullOrEmpty(rsoAchievementRuntimeData.Value.lastAchievementCompletedID))
        {
            rseTriggerAchievement.Call();
            rsoAchievementRuntimeData.Value.lastAchievementCompletedID = "";
            rsoContentSaved.Value.lastAchievementCompletedId = "";
        }
    }


    private void ConnectAchievementNotifiers(int i)
    {
        achievements[i].BindEventCheck();
        achievements[i].OnAchievementComplete += OnAchievementCompleted;
    }
    
    private void OnAchievementCompleted(SSO_Achievement achievement)
    {
        rsoContentSaved.Value.achievementsIdCompleted.Add(achievement.achievementId);
        
        rsoAchievementRuntimeData.Value.lastAchievementCompletedID = achievement.achievementId;
        
        if (!achievement.achievementOnReaload) rseTriggerAchievement.Call();
        achievement.UnbindEventCheck();
        achievement.OnAchievementComplete -= OnAchievementCompleted;
    }

    private void SaveLastAchievementCompleted()
    {
        rsoContentSaved.Value.lastAchievementCompletedId = rsoAchievementRuntimeData.Value.lastAchievementCompletedID;
        rsoAchievementRuntimeData.Value = null;
    }

    private void OnEnable() => rseDeath.action += SaveLastAchievementCompleted;
    private void OnDisable() => rseDeath.action -= SaveLastAchievementCompleted;

    private void OnApplicationQuit() => SaveLastAchievementCompleted();
}