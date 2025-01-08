using UnityEngine;

[CreateAssetMenu(fileName = "SSO_Achievement", menuName = "ScriptableObject/SSO_Achievement")]
public class SSO_Achievement : ScriptableObject
{
    [Header("Settings")]
    public string achievementID;
    public string achievementTitle;
    [TextArea] public string achievementDescription;
    public Sprite icon;
    
    [Header("References")]
    public RSF_AchievementCondition rsfAchievementCondition;
}