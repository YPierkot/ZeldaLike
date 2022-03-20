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
                    StartCoroutine(StopMovement(2.5f, objectSequence)); 
                }
            }
            else
            {
                col.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
                col.gameObject.GetComponent<ResetColor>().StartCoroutine(col.gameObject.GetComponent<ResetColor>().ResetObjectColor());

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(col.gameObject.GetComponent<Rigidbody>().DOMove(
                    new Vector3(attractivePoint.x, col.gameObject.transform.position.y, attractivePoint.z), 4f));
                StartCoroutine(StopMovement(2.5f, mySequence));
            }
        }
        
        Destroy(gameObject, 0.2f);
    }
    
    private void OnCollisionEnter(Collision other)
    {
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
        else if (!other.transform.CompareTag("Ennemy"))
        {
            WindCardLongEffect();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collider.isTrigger = false;
    }

    private IEnumerator StopMovement(float timeToStopMovement, Sequence sequence)
    {
        yield return new WaitForSeconds(timeToStopMovement);
        DOTween.Kill(sequence);
    }

    private void OnDestroy()
    {
        CardsController.isWindGround = false;
    }
}
