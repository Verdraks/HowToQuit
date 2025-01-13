using System;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Serialization;
using Task = System.Threading.Tasks.Task;

public class AchievementListManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] RSO_AchievementRuntimeData rsoAchievementRuntimeData;
    [SerializeField] private GameObject achievementUiPrefab;
    [SerializeField] private Transform achievementsContainer;
    [Header("Input")]
    [SerializeField] private RSE_TriggerAchievement rseTriggerAchievement;

    private Tuple<AchievementUiIcon,string>[] achievementUiObjects;

    private async void Start()
    {
        await Task.Delay(100);
        SetupHUD();
    }

    private void OnEnable() => rseTriggerAchievement.action += UpdateHUD;
    private void OnDisable() => rseTriggerAchievement.action -= UpdateHUD;
    
    private void SetupHUD()
    {
        achievementUiObjects = new Tuple<AchievementUiIcon, string>[rsoAchievementRuntimeData.Value.achievements.Length] ;

        for (int i = 0; i < rsoAchievementRuntimeData.Value.achievements.Length; i++)
        {
            AchievementUiIcon achievementUiIconObject = Instantiate(achievementUiPrefab, achievementsContainer).GetComponent<AchievementUiIcon>();
            achievementUiObjects[i] = new Tuple<AchievementUiIcon, string>(achievementUiIconObject, rsoAchievementRuntimeData.Value.achievements[i].achievementId);
            achievementUiObjects[i].Item1.UpdateElement(rsoAchievementRuntimeData.Value.achievementsCompleted[i],rsoAchievementRuntimeData.Value.achievements[i]);
        }
        
    }

    private void UpdateHUD()
    {
        if (achievementUiObjects == null) return;
        for (int i = 0; i < achievementUiObjects.Length; i++)
        {
            if (achievementUiObjects[i].Item2 == rsoAchievementRuntimeData.Value.lastAchievementCompletedID)
            {
                achievementUiObjects[i].Item1.UpdateElement(true, rsoAchievementRuntimeData.Value.achievements[i]);
            }
        }
        
    }
    
}