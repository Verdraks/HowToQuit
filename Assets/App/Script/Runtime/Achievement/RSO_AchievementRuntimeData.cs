using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[CreateAssetMenu(fileName = "RSO_AchievementComplete", menuName = "RSO/RSO_AchievementComplete")]
public class RSO_AchievementRuntimeData : BT.ScriptablesObject.RuntimeScriptableObject<AchievementRuntimeData>{}

[SerializeField]
public class AchievementRuntimeData
{
    public SSO_Achievement[] achievements;
    public bool[] achievementsCompleted;
    
    public string lastAchievementCompletedID;

    public SSO_Achievement GetLastAchievementCompleted()
    {
        if (lastAchievementCompletedID == "") return null;
        for (int i = 0; i < achievements.Length; i++)
        {
            if (achievements[i].AchievementId == lastAchievementCompletedID) return achievements[i];
        }
        return null;
    }
}