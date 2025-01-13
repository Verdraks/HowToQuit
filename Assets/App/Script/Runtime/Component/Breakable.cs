using UnityEngine;
public class Breakable : MonoBehaviour,IBreakable
{
    public void Break()
    {
        Destroy(gameObject);
    }
}