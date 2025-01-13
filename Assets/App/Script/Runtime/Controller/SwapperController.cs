using System;
using UnityEngine;
using UnityEngine.Serialization;

public class SwapperController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject[] controllerPrefabs;

    [Header("References")]
    [SerializeField] private RSO_SpawnPosition rsoSpawnPosition;
    
    [Header("Input")]
    [SerializeField] private RSE_TeleportBack rseTeleportBack;
    [SerializeField] private RSE_InputSwapController rseInputSwapController;
    
    private int _indexCurrentController;
    private PhysicController[] _controllersInst;

    private void Awake() => InitControllers();

    private void OnEnable()
    {
        rseInputSwapController.action += OnInputSwapController;
        rseTeleportBack.action += TeleportToLastPoint;
    }

    private void OnDisable()
    {
        rseInputSwapController.action -= OnInputSwapController;
        rseTeleportBack.action -= TeleportToLastPoint;
    }

    private void Start()
    {
        _indexCurrentController = _controllersInst.Length / 2;
        _controllersInst[_indexCurrentController].Teleport(rsoSpawnPosition.Value, Quaternion.identity);
        _controllersInst[_indexCurrentController].gameObject.SetActive(true);
    }
    
    private void InitControllers()
    {
        _controllersInst = new PhysicController[controllerPrefabs.Length];
        for (var i = 0; i < controllerPrefabs.Length; i++)
        {
            var controllerPrefab = controllerPrefabs[i];
            var instController = Instantiate(controllerPrefab, transform);
            instController.SetActive(false);
            _controllersInst[i] = instController.GetComponent<PhysicController>();
        }
    }

    private void OnInputSwapController(int value)
    {
        var oldValue = _indexCurrentController;
        _indexCurrentController = Mathf.Clamp(_indexCurrentController + value, 0, _controllersInst.Length - 1);
        if (oldValue != _indexCurrentController) SwapController(oldValue);
    }

    private void SwapController(int oldValue)
    {
        var oldController = _controllersInst[oldValue];
        var newController = _controllersInst[_indexCurrentController];
        oldController.gameObject.SetActive(false);
        newController.Teleport(oldController.transform.position, oldController.transform.rotation);
        newController.gameObject.SetActive(true);
    }

    private void TeleportToLastPoint()
    {
        _controllersInst[_indexCurrentController].Teleport(rsoSpawnPosition.Value, Quaternion.identity);
    }
    
}