using System;
using UnityEngine;
public class ReloadManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float timeBeforeReload;
    [SerializeField] private string sceneToLoad;

    [Header("Output")]
    [SerializeField] private RSE_LoadScene rseLoadScene;

    private void Start()
    {
        StartCoroutine(Utils.Delay(timeBeforeReload, ()=>rseLoadScene.Call(sceneToLoad)));
    }
}