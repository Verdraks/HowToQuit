using BT.ScriptablesObject;
using UnityEngine;

[CreateAssetMenu(fileName = "SSO_Achievement_", menuName = "ScriptableObject/Achievement/SSO_AchievementEventString")]
public class SSO_AchievementEventString : SSO_AchievementEvent<string>
{
    protected override bool CheckCondition()
    {
        return valueTarget.Equals(_value);
    }
}