using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WindCardLongRange : MonoBehaviour
{
    
    [SerializeField] private LayerMask Ennemy;
    [SerializeField] private Vector3 attractivePoint;
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 4f);
    }

    public void WindCardLongEffect()
    {
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        attractivePoint = gameObject.transform.position;
            
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3, Ennemy);
        foreach (var col in colliders)
        {
            col.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
            col.gameObject.GetComponent<ResetColor>().StartCoroutine(col.gameObject.GetComponent<ResetColor>().ResetObjectColor());

            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(col.gameObject.GetComponent<Rigidbody>().DOMove(
                new Vector3(attractivePoint.x, col.gameObject.transform.position.y, attractivePoint.z), 4f));
            StartCoroutine(StopMovement(2.5f, mySequence));
        }
        
        Destroy(gameObject, 0.2f);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Collider"))
        {
            WindCardLongEffect();
        }
    }

    private IEnumerator StopMovement(float timeToStopMovement, Sequence sequence)
    {
        yield return new WaitForSeconds(timeToStopMovement);
        DOTween.Kill(sequence);
    }
    
}
