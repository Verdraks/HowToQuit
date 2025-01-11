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
    
    private void Awake()
    {
        for (var index = 0; index < achievements.Length; index++)
        {
            foreach (var id in rsoContentSaved.Value.achievementsIdCompleted)
            {
                if (id != achievements[index].AchievementId)
                {
                    ConnectAchievementNotifiers(index);
                }
            }
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
        achievement.UnbindEventCheck();
        achievement.OnAchievementComplete -= OnAchievementCompleted;
    }
}