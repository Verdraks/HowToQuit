using UnityEngine;
using System.Collections.Generic;

public class BulletManager : MonoBehaviour
{
    
    [Header("Settings")]
    [SerializeField] private int poolSize = 10;
    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [Header("Input")]
    [SerializeField] private RSE_BulletReturn rseBulletReturn;
    [SerializeField] private RSE_BulletFire rseBulletFire;
    
    private List<Bullet> bulletPool = new();

    private void OnEnable()
    {
        rseBulletFire.action += FireBullet;
        rseBulletReturn.action += ReturnBulletToPool;
    }

    private void OnDisable()
    {
        rseBulletFire.action -= FireBullet;
        rseBulletReturn.action -= ReturnBulletToPool;
    }

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Bullet newBullet = Instantiate(bulletPrefab,transform).GetComponent<Bullet>();
            newBullet.gameObject.SetActive(false);
            bulletPool.Add(newBullet);
        }
    }
    
    private Bullet GetBulletFromPool()
    {
        foreach (Bullet bullet in bulletPool)
        {
            if (!bullet.gameObject.activeSelf)
            {
                return bullet;
            }
        }
        return null;
    }

    private void FireBullet(Vector3 position, Quaternion rotation)
    {
        Bullet bullet = GetBulletFromPool();
        if (bullet != null)
        {
            bullet.transform.position = position;
            bullet.transform.rotation = rotation;
            bullet.Activate();
        }
    }


    private void ReturnBulletToPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
}
