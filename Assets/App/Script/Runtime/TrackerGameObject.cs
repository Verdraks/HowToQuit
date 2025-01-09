using System;
using BT.ScriptablesObject;
using UnityEngine;
public class TrackerGameObject : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Transform target;

    [Space(10)] 
    [SerializeField] private RuntimeScriptableObject<TransformData> rsoTransformData;

    private void Awake()
    {
        rsoTransformData.Value = new TransformData();
    }

    private void LateUpdate()
    {
        rsoTransformData.Value.Position = target.position;
        rsoTransformData.Value.Rotation = target.rotation;
    }
}