using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyProjectile(gameObject));
        
    }
    
    private static IEnumerator DestroyProjectile(GameObject obj)
    {
        yield return new WaitForSeconds(10);
        Destroy(obj);
    }
}
