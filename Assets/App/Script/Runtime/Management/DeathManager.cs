using System;
using UnityEngine;
public class DeathManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string sceneToLoadOnDeath;
    [SerializeField] private float deathDelay;
    
    [Header("Input")] 
    [SerializeField] private RSE_Death rseDeath;
    [Header("Output")]
    [SerializeField] private RSE_SaveData rseSaveData;
    [SerializeField] private RSE_LoadScene rseLoadScene;
    [SerializeField] private RSE_TriggerAnimation rseTriggerAnimation;
    

    private void OnEnable() => rseDeath.action += OnDeath;

    private void OnDisable() => rseDeath.action -= OnDeath;

    private void OnDeath()
    {
        rseSaveData.Call();
        rseLoadScene.Call(sceneToLoadOnDeath);
    }
    
}