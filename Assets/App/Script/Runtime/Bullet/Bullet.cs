using System.Collections;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime = 2f;
    [Header("Output")]
    [SerializeField] private RSE_BulletReturn rseBulletReturn;
    
    public void Activate()
    {
        gameObject.SetActive(true);
        StartCoroutine(Move());
        StartCoroutine(Utils.Delay(lifeTime, Deactivate));
    }

    private IEnumerator Move()
    {
        while (gameObject.activeSelf)
        {
            transform.Translate(Vector3.right * (speed * Time.deltaTime));
            yield return null;
        }
    }
    
    private void Deactivate()
    {
        StopCoroutine(Move());
        rseBulletReturn.Call(this);
    }
}