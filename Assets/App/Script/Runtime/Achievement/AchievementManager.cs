using System;
using System.Collections.Generic;
using UnityEngine;
public class AchievementManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<SSO_Achievement> achievements;
    
    [Header("Output")]
    [SerializeField] private RSE_AchievementComplete rseAchievementComplete;

    private bool[] _achievementAchieved;

    private void Awake()
    {
        _achievementAchieved = new bool[achievements.Count];
    }

    public void Update()
    {
        for (int i = 0; i < _achievementAchieved.Length; i++)
        {
            if (!_achievementAchieved[i])
            {
                if (achievements[i].rsfAchievementCondition.AchievementAchieved()) OnAchievementCompleted(i);
            }
        }
    }

    private void OnAchievementCompleted(int i)
    {
        print("Achieved: " + achievements[i].achievementID);
        _achievementAchieved[i] = true;
        rseAchievementComplete.Call(achievements[i]);
    }
}