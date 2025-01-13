using UnityEngine;

public class PhantomController : PhysicController
{
    [Header("Settings")]
    [SerializeField] private LayerMask excludeLayerMask;
    
    private LayerMask _originalExcludeLayerMask;
    private bool _abilityEnable;
    
    private void Awake() => _originalExcludeLayerMask = rb.excludeLayers;
    
    protected override void OnInputAbility()
    {
        _abilityEnable = !_abilityEnable;
        rb.excludeLayers = _abilityEnable ? excludeLayerMask: _originalExcludeLayerMask;
        if(!_abilityEnable) CheckConditionControllerEndAbility();
    }

    private void CheckConditionControllerEndAbility()
    {
        
    }
}