using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUi : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image imageComp;
    [SerializeField] private TMP_Text textTitleComp;
    [SerializeField] private TMP_Text textDescriptionComp;
    [Space(10)]
    [SerializeField] private RSO_AchievementComplete rsoAchievementComplete;

    [Header("Input")]
    [SerializeField] private RSE_TriggerAchievement rseTriggerAchievement;


    private void OnEnable() => rseTriggerAchievement.action += UpdateElement;
    private void OnDisable() => rseTriggerAchievement.action -= UpdateElement;

    private void UpdateElement()
    {
        imageComp.sprite = rsoAchievementComplete.Value.achievementIcon;
        textTitleComp.text = rsoAchievementComplete.Value.achievementName;
        textDescriptionComp.text = rsoAchievementComplete.Value.achievementDescription;
    }
}