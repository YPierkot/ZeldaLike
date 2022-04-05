using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class WindCardLongRange : MonoBehaviour
{
    private BoxCollider collider;

    public Vector3 velocity;
    
    [SerializeField] private LayerMask interactMask;
    [SerializeField] private Vector3 attractivePoint;

    [SerializeField] private LayerMask groundMask;

    private void OnEnable()
    {
        if (collider == null) collider = GetComponent<BoxCollider>();
        collider.isTrigger = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 4f);
    }

    public void WindCardLongEffect()
    {
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        attractivePoint = gameObject.transform.position;
            
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3, interactMask);
        foreach (var col in colliders)
        {
            if (col.CompareTag("Interactable"))
            {
                if (col.GetComponent<Switcher>() != null)
                {
                    Switcher swich = col.GetComponent<Switcher>();
                    if (swich.windAffect) swich.Switch();
                }
                else if (col.GetComponent<InteracteObject>().windAffect)
                {
                    Sequence objectSequence = DOTween.Sequence();
                    objectSequence.Append(col.gameObject.GetComponent<Rigidbody>().DOMove(
                        new Vector3(attractivePoint.x, col.gameObject.transform.position.y, attractivePoint.z), 4f));
                    StartCoroutine(StopMovement(2.5f, col.gameObject)); 
                }
            }
            else
            {
                col.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(col.gameObject.GetComponent<Rigidbody>().DOMove(
                    new Vector3(attractivePoint.x, col.gameObject.transform.position.y, attractivePoint.z), 4f));
                StartCoroutine(StopMovement(2.5f, col.gameObject));
            }
        }
        
        Destroy(gameObject, 0.2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.ToString() == groundMask.ToString())
        {
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        
        if (!other.transform.CompareTag("Player"))
        {
            this.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        
        if (other.transform.CompareTag("Interactable") )
        {
            Debug.Log(other.transform.name);
            if (other.transform.GetComponent<InteracteObject>().windThrough)
            {
                Debug.Log(velocity);
                collider.isTrigger = true;
                GetComponent<Rigidbody>().velocity = velocity;
            }
            else WindCardLongEffect();
        }
    }
    

    /*private void OnTriggerExit(Collider other)
    {
        collider.isTrigger = false;
    }*/

    private IEnumerator StopMovement(float timeToStopMovement, GameObject objTransform)
    {
        yield return new WaitForSeconds(timeToStopMovement);
        objTransform.transform.DOKill();
    }

    private void OnDestroy()
    {
        CardsController.isWindGround = false;
    }
}
