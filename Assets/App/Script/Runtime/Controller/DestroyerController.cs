using UnityEngine;

public class DestroyerController : PhysicController
{
    [Header("Settings")] 
    [SerializeField] private Vector3 sizeCast;
    [SerializeField] private Vector3 positionCast;
    [SerializeField] private float distanceCast;
    [SerializeField] private LayerMask layerMask;
    
    [SerializeField] private int maxCastObject;

    private RaycastHit[] _hits;

    private void Awake() => _hits = new RaycastHit[maxCastObject];

    protected override void OnInputAbility()
    {
        int hitCount = Physics.BoxCastNonAlloc(transform.position + positionCast, sizeCast, transform.forward, _hits,Quaternion.identity,distanceCast,layerMask);
        if (hitCount > 0)
        {
            for (int i = 0; i < hitCount; i++)
            {
                _hits[i].transform.GetComponent<IBreakable>()?.Break();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube( transform.position + transform.rotation * positionCast  ,sizeCast);
    }
}