using UnityEngine;
public class CameraController : MonoBehaviour
{
    
    [Header("Settings")]
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float distanceFromTarget = 5f;
    [SerializeField] private float minYAngle = -30f;
    [SerializeField] private float maxYAngle = 60f;

    [Header("References")]
    [SerializeField] private RSE_InputLook rseInputLook;
    [SerializeField] private RSO_TargetTransform rsoTargetData;

    private Vector3 _currentRotation;
    private Vector2 _lastInputLook;

    private void OnEnable()
    {
        rseInputLook.action += OnInputLook;
    }

    private void OnDisable()
    {
        rseInputLook.action -= OnInputLook;
    }
    
    private void Start()
    {
        if (rsoTargetData.Value == null) return;

        _currentRotation = transform.eulerAngles;
        UpdateCameraPosition();
    }

    private void OnInputLook(Vector2 input)
    {
        if (_lastInputLook != input)
        {
            _lastInputLook = input;
            UpdateCameraRotation();
        }
        
    }

    private void LateUpdate()
    {
        // Update camera position
        UpdateCameraPosition();
    }

    private void UpdateCameraRotation()
    {
        if (rsoTargetData.Value == null) return;
    
        // Process input for rotation
        float mouseX = _lastInputLook.x * rotationSpeed * Time.deltaTime;
        float mouseY = -_lastInputLook.y * rotationSpeed * Time.deltaTime;
    
        // Adjust rotation
        _currentRotation.y += mouseX;
        _currentRotation.x = Mathf.Clamp(_currentRotation.x + mouseY, minYAngle, maxYAngle);
    }

    private void UpdateCameraPosition()
    {
        if (rsoTargetData.Value == null) return;

        // Calculate new position and rotation
        Quaternion rotation = Quaternion.Euler(_currentRotation.x, _currentRotation.y, 0);
        Vector3 position = rsoTargetData.Value.Position - (rotation * Vector3.forward * distanceFromTarget);

        // Set the camera's position and rotation
        transform.position = position;
        transform.LookAt(rsoTargetData.Value.Position);
    }
}