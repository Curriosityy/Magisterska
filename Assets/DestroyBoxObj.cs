using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBoxObj : MonoBehaviour, IDamageable
{
    
    public GameObject destroyedV;
       
    
    public void DealDamage(int damagage)
    {
        Instantiate(destroyedV, transform.position, transform.rotation);
        Destroy(gameObject);

    }
}
