using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCardLongRange : MonoBehaviour
{

    public LayerMask Ennemy;
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 4f);
    }

    public void IceCardLongEffet()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3, Ennemy);
        foreach (var col in colliders)
        {
            col.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            col.gameObject.GetComponent<ResetColor>().StartCoroutine(col.gameObject.GetComponent<ResetColor>().ResetObjectColor());
            if (col.gameObject.GetComponent<MoveForTest>())
            {
                col.gameObject.GetComponent<MoveForTest>().StartCoroutine(col.gameObject.GetComponent<MoveForTest>().FreezePos());
            }
        }
        
        Destroy(gameObject, 0.3f);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Collider"))
        {
            IceCardLongEffet();
        }
        
        Destroy(gameObject, 0.3f);
    }
}
