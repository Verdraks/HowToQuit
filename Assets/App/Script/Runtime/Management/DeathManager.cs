using System;
using System.Threading.Tasks;
using UnityEngine;
public class DeathManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string sceneToLoadOnDeath;
    [SerializeField] private int deathDelay = 100;
    
    [Header("Input")] 
    [SerializeField] private RSE_Death rseDeath;
    [Header("Output")]
    [SerializeField] private RSE_SaveData rseSaveData;
    [SerializeField] private RSE_LoadScene rseLoadScene;
    [SerializeField] private RSE_TriggerAnimation rseTriggerAnimation;
    

    private void OnEnable() => rseDeath.action += OnDeath;

    private void OnDisable() => rseDeath.action -= OnDeath;

    private async void OnDeath()
    {
        await Task.Delay(deathDelay);
        rseSaveData.Call();
        rseLoadScene.Call(sceneToLoadOnDeath);
    }
    
}