using UnityEngine;
public class TriangleController : PhysicController
{
    [Header("Parameters")]
    [SerializeField] private float shootCooldown;
    
    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform rootBulletShoot;
    [SerializeField] private RSE_InputFire rseInputFire;

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
        print("shoot");
        
        // Instantiate(bulletPrefab,rootBulletShoot.position,Quaternion.identity);
        // _canShoot = false;
        // StartCoroutine(Utils.Delay(shootCooldown, ()=> _canShoot = true));
    }
}