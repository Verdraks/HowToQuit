using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AchievementUiScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image imageComp;
    [SerializeField] private TMP_Text textTitleComp;
    [SerializeField] private TMP_Text textDescriptionComp;
    [Space(10)]
    [SerializeField] private RSO_AchievementRuntimeData rsoAchievementRuntimeData;

    [Header("Input")]
    [SerializeField] private RSE_TriggerAchievement rseTriggerAchievement;


    private void OnEnable() => rseTriggerAchievement.action += UpdateElement;
    private void OnDisable() => rseTriggerAchievement.action -= UpdateElement;

    private void UpdateElement()
    {
        var achievement = rsoAchievementRuntimeData.Value.GetLastAchievementCompleted();
        imageComp.sprite = achievement.achievementIcon;
        textTitleComp.text = achievement.achievementName;
        textDescriptionComp.text = achievement.achievementDescription;
    }
}