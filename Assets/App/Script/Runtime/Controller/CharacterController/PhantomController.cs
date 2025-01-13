using UnityEngine;

public class PhantomController : PhysicController
{
    [Header("Settings")]
    [SerializeField] private LayerMask excludeLayerMask;
    [SerializeField] private Collider collider;
    [SerializeField] private string deathTag = "Suffocate";
    [Header("Output")]
    [SerializeField] private RSE_DeathCondition rseDeathCondition;
    [SerializeField] private RSE_Death rseDeath;
    
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
        if (Physics.BoxCast(collider.bounds.center, collider.bounds.extents, Vector3.up, Quaternion.identity, 1f,
                excludeLayerMask))
        {
            rseDeathCondition.Call(deathTag);
            rseDeath.Call();
        }
    }
}