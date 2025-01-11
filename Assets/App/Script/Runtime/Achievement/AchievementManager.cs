using System;
using System.Collections.Generic;
using BT.ScriptablesObject;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AchievementManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private SSO_Achievement[] achievements;
    private List<string> _achievementIdCompleted;
    
    private void Awake()
    {
        for (var index = 0; index < achievements.Length; index++)
        {
            foreach (var id in _achievementIdCompleted)
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
        _achievementIdCompleted.Add(achievement.AchievementId);
        achievement.UnbindEventCheck();
        achievement.OnAchievementComplete -= OnAchievementCompleted;
    }
}