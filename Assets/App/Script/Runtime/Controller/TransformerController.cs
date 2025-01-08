using System;
using System.Collections.Generic;
using UnityEngine;

public class TransformerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] List<GameObject> transformerPrefabs = new();

    [Header("References")]
    [SerializeField] private RSO_SpawnPoint rsoSpawnPoint;
    [Header("Input")]
    [SerializeField] private RSE_InputTransformer rseInputTransformer;

    private int _indexTransformerPrefab;
    private GameObject _transformerInst;

    private void Start() => InitComponent();

    private void InitComponent()
    {
        _indexTransformerPrefab = transformerPrefabs.Count / 2;
        SwitchTransformerPrefab(rsoSpawnPoint.Value);
    }

    private void OnEnable()
    {
        rseInputTransformer.action += OnInputTransformer;
    }

    private void OnDisable()
    {
        rseInputTransformer.action -= OnInputTransformer;
    }

    private void OnInputTransformer(int value)
    {
        int oldValue = _indexTransformerPrefab;
        _indexTransformerPrefab = Mathf.Clamp(_indexTransformerPrefab + value, 0,transformerPrefabs.Count-1);
        if (oldValue != _indexTransformerPrefab)
            SwitchTransformerPrefab(_transformerInst? _transformerInst.transform.position : rsoSpawnPoint.Value);
    }

    private void SwitchTransformerPrefab(Vector3 position)
    {
        if (_transformerInst != null)
        {
            Destroy(_transformerInst);
        }
        _transformerInst = Instantiate(transformerPrefabs[_indexTransformerPrefab],position,Quaternion.identity);
    }
    
}