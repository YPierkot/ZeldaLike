using System;
using System.Collections;
using System.Collections.Generic;
using AI;
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
            
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3);
        foreach (var col in colliders)
        {
            Debug.Log(col.name);
            if (col.transform.CompareTag("Interactable"))
            {
                col.GetComponent<InteracteObject>().Freeze(transform.position);
            }
            else if (col.transform.CompareTag("Ennemy"))
            {
                if (col.transform.GetComponent<SwingerAI>())
                    col.transform.GetComponent<SwingerAI>().FreezeEnnemy();
                else if (col.transform.GetComponent<KamikazeAI>())
                    col.transform.GetComponent<KamikazeAI>().FreezeEnnemy();
                else if (col.transform.GetComponent<MageAI>())
                    col.transform.GetComponent<MageAI>().FreezeEnnemy();
                else if (col.transform.GetComponent<BomberAI>())
                    col.transform.GetComponent<BomberAI>().FreezeEnnemy();
            }
        }
        
        Destroy(gameObject, 0.3f);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (!other.transform.CompareTag("Ennemy")|| !other.transform.CompareTag("Player"))
        {
            transform.position = Vector3.zero;
        }
        
        Destroy(gameObject, 0.3f);
    }

    private void OnDestroy()
    {
        CardsController.isIceGround = false;
    }
}
