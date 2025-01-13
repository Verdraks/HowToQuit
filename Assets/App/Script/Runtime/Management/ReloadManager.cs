using System;
using System.Collections;
using UnityEngine;
public class ReloadManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float timeBeforeReload;
    [SerializeField] private string sceneToLoad;

    [Header("References")]
    [SerializeField] private RSO_SliderValue rsoSliderValue;
    
    [Header("Output")]
    [SerializeField] private RSE_LoadScene rseLoadScene;

    private void Start()
    {
        rsoSliderValue.Value = 0f;
        StartCoroutine(Reload());
        StartCoroutine(Utils.Delay(timeBeforeReload, ()=>rseLoadScene.Call(sceneToLoad)));
    }

    private IEnumerator Reload()
    {
        float timeElapsed = 0f;
        
        while (timeElapsed < timeBeforeReload)
        {
            rsoSliderValue.Value = Mathf.Lerp(0, 1f,timeElapsed / timeBeforeReload);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}