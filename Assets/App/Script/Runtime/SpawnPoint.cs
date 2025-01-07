using System;
using UnityEngine;
public class SpawnPoint : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private bool spawnPointStart;
    
    [Header("References")]
    [SerializeField] private RSO_SpawnPoint rsoSpawnPoint;

    private void Awake()
    {
        if (spawnPointStart) rsoSpawnPoint.Value = transform.position;
    }
}