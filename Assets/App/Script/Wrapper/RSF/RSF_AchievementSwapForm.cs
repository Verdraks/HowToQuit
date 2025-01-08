using UnityEngine;

[CreateAssetMenu(fileName = "RSF_AchievementSwapForm", menuName = "RSF/RSF_AchievementSwapForm")]
public class RSF_AchievementSwapForm : RSF_AchievementCondition
{
    
    
    public override bool AchievementAchieved()
    {
        return true;
    }
}