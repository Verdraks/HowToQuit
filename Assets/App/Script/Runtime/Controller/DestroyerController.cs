using UnityEngine;

public class DestroyerController : PhysicController
{
    [Header("Settings")] 
    [SerializeField] private Vector3 sizeCast;
    [SerializeField] private Vector3 positionCast;
    [SerializeField] private float distanceCast;
    [SerializeField] private LayerMask layerMask;
    
    [SerializeField] private int maxCastObject;

    private RaycastHit[] hits;

    private void Awake() => hits = new RaycastHit[maxCastObject];

    protected override void OnInputAbility()
    {
        int hitCount = Physics.BoxCastNonAlloc(transform.position + positionCast, sizeCast*0.5f, transform.forward, hits,Quaternion.identity,distanceCast,layerMask);
        if (hitCount > 0)
        {
            for (int i = 0; i < hitCount; i++)
            {
                Debug.Log($"Hit: {hits[i].transform.name}");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position+ positionCast,sizeCast);
    }
}