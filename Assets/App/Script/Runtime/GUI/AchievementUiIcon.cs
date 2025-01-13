using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUiIcon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image imageComp;
    [SerializeField] private TMP_Text textTitleComp;
    [SerializeField] private TMP_Text textDescriptionComp;

    public void UpdateElement(bool isAchieved, SSO_Achievement achievementData)
    {
        imageComp.sprite = achievementData.achievementIcon;
        imageComp.color = isAchieved ? Color.white : Color.gray;
        textTitleComp.text = achievementData.achievementName;
        textDescriptionComp.text = isAchieved ? achievementData.achievementDescription : ".....";
    }
}