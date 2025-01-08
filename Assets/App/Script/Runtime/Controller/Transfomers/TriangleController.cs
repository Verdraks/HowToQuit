using UnityEngine;
public class TriangleController : PhysicController
{
    [Header("Settings")]
    [SerializeField] private float shootCooldown;
    
    [Header("References")]
    [SerializeField] private Transform rootBulletShoot;
    [Header("Input")]
    [SerializeField] private RSE_InputFire rseInputFire;
    [Header("Output")]
    [SerializeField] private RSE_BulletFire rseBulletFire;

    private bool _canShoot = true;
        
    protected override void OnEnable()
    {
        base.OnEnable();
        rseInputFire.action += OnInputFire;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        rseInputFire.action -= OnInputFire;
    }

    private void OnInputFire()
    {
        if (!_canShoot) return;
        rseBulletFire.Call(rootBulletShoot.position,Quaternion.identity);
        _canShoot = false;
        StartCoroutine(Utils.Delay(shootCooldown, ()=> _canShoot = true));
    }
}