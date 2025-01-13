using System;
using BT.ScriptablesObject;
using UnityEngine;
using UnityEngine.Serialization;

public class ClipPlayer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private AudioClip clip;
    [SerializeField] private float volume = 1f;
    
    [Header("Input")] 
    [SerializeField] private RuntimeScriptableEvent rseInput;
    
    [Header("Output")]
    [SerializeField] private RSE_PlayClip rsePlayClip;


    private void OnEnable() => rseInput.action += PlayClip;
    private void OnDisable() => rseInput.action -= PlayClip;

    private void PlayClip()
    {
        rsePlayClip.Call(clip,transform.position,volume);
    }
    
}