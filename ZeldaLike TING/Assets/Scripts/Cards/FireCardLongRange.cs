using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using Unity.VisualScripting;
using UnityEngine;

public class FireCardLongRange : MonoBehaviour
{
    public LayerMask mask; //Ennemy & Interact
    public LayerMask groundMask;
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 4f);
    }

    public void FireCardLongEffect()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5, mask);
        foreach (var col in colliders)
        {
            switch (col.transform.tag)
            {
                case "Interactable":
                    col.GetComponent<InteracteObject>().OnFireEffect();
                    break;
                
                case "Ennemy" :
                    if (col.transform.GetComponent<SwingerAI>())
                        col.transform.GetComponent<SwingerAI>().LooseHp(2);
                    else if (col.transform.GetComponent<KamikazeAI>())
                        col.transform.GetComponent<KamikazeAI>().LooseHp(2);
                    else if (col.transform.GetComponent<MageAI>())
                        col.transform.GetComponent<MageAI>().LooseHp(2);
                    else if (col.transform.GetComponent<BomberAI>())
                        col.transform.GetComponent<BomberAI>().LooseHp(2);
                    break;
            }
        }
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.CompareTag("Player"))
        {
            this.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        
        if (other.ToString() == groundMask.ToString())
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5);
    }

    private void OnDestroy()
    {
        CardsController.isFireGround = false;
    }
}
