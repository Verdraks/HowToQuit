using UnityEngine;

[CreateAssetMenu(fileName = "SSO_ControllerStat", menuName = "ScriptableObject/SSO_ControllerStat")]
public class SSO_ControllerStat : ScriptableObject
{
    [Header("Settings")]
    public float speed = 5f;
    public float sprintMultiplier = 1.5f;
    [Space(10)]
    public float jumpForce = 10f;
    public float jumpCooldown = 0.5f; 
    public float coyoteeTime = 0.2f;
    [Space(10)]
    public float fallAcceleration = 2f;
    public float distanceCheck = 0.2f;
    [Space(10)]
    public float smoothStopTime = 0.2f;
    public float rotationSpeed = 360f;
}