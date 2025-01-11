using UnityEngine;
public class TransformData
{
    public Vector3 Position;
    public Quaternion Rotation;
    
    public Vector3 Forward => Rotation * Vector3.forward;
    public Vector3 Right => Rotation * Vector3.right;
}