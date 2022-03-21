using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCardLongRange : MonoBehaviour
{

    public LayerMask Ennemy;
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 3f);
    }

    public void IceCardLongEffet()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3, Ennemy);
        foreach (var col in colliders)
        {
            Debug.Log(col.name);
            if (col.transform.CompareTag("Interactable"))
            {
                col.GetComponent<InteracteObject>().Freeze(transform.position);
            }
        }
        
        Destroy(gameObject, 0.3f);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (!other.transform.CompareTag("Ennemy"))
        {
            IceCardLongEffet();
        }
        
        Destroy(gameObject, 0.3f);
    }

    private void OnDestroy()
    {
        CardsController.isIceGround = false;
    }
}
