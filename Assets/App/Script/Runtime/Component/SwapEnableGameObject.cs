using System;
using UnityEngine;
public class SwapEnableGameObject : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private GameObject[] objects;
    [Header("References")]
    [SerializeField] private bool swapEnable;

    public void Start()
    {
        Swap();
    }

    public void Swap()
    {
        foreach (var go in objects)
        {
            go.SetActive(swapEnable);
        }
        swapEnable = !swapEnable;
    }
}