using System;
using UnityEngine;
public class DeathManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string sceneToLoadOnDeath;
    
    [Header("Input")] 
    [SerializeField] private RSE_Death rseDeath;
    [Header("Output")]
    [SerializeField] private RSE_LoadScene rseLoadScene;
    

    private void OnEnable() => rseDeath.action += OnDeath;

    private void OnDisable() => rseDeath.action -= OnDeath;

    private void OnDeath()
    {
        rseLoadScene.Call(sceneToLoadOnDeath);
    }
    
}