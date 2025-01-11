using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool spawnPointOnAwake;
    
    [Header("References")]
    [SerializeField] private RSO_SpawnPosition rsoSpawnPosition;

    private void Awake()
    {
        if (spawnPointOnAwake) rsoSpawnPosition.Value = transform.position;
    }
}