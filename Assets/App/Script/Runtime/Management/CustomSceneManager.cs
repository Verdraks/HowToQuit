using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    private bool _sceneCurrentlyLoaded = false;

    [Header("Input")] 
    [SerializeField] private RSE_LoadScene rseLoadScene;

    private void OnEnable() => rseLoadScene.action += LoadScene;
    private void OnDisable() => rseLoadScene.action -= LoadScene;

    private void LoadScene(string sceneName)
    {
        if (!_sceneCurrentlyLoaded) return;
        _sceneCurrentlyLoaded = true;
        
        
        StartCoroutine(Utils.LoadSceneAsync(sceneName, LoadSceneMode.Single,OnSceneLoaded));
    }
    
    private void OnSceneLoaded()
    {
        _sceneCurrentlyLoaded = false;
        Debug.Log("Scene Loaded");
    }
}