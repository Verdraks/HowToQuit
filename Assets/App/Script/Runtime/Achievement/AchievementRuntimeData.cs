using UnityEngine;

[System.Serializable]
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
            if (achievements[i].achievementId == lastAchievementCompletedID) return achievements[i];
        }
        return null;
    }
}