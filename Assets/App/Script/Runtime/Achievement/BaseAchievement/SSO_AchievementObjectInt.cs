using UnityEngine;

[CreateAssetMenu(fileName = "SSO_Achievement_", menuName = "ScriptableObject/Achievement/SSO_AchievementObjectInt")]
public class SSO_AchievementObjectInt : SSO_AchivementObject<int>
{
    protected override bool CheckCondition()
    {
        return rso.Value == valueTarget;
    }
}