using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCardLongRange : MonoBehaviour
{
    public LayerMask Ennemy;
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 4f);
    }

    public void FireCardLongEffect()
    { 
        Debug.Log("Oui alors");
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5, Ennemy);
        foreach (var col in colliders)
        {
            col.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            col.gameObject.GetComponent<ResetColor>().StartCoroutine(col.gameObject.GetComponent<ResetColor>().ResetObjectColor());
        }
        
        Destroy(gameObject);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (!other.transform.CompareTag("Ennemy"))
        {
            FireCardLongEffect();
        }
        Destroy(gameObject, 0.3f);
    }

    private void OnDestroy()
    {
        CardsController.isFireGround = false;
    }
}
