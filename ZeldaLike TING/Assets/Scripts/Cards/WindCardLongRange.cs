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
    public GameObject DebugSphere;

    private void OnEnable()
    {
        if (collider == null) collider = GetComponent<BoxCollider>();
        collider.isTrigger = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 4.4f);
    }

    public void WindCardLongEffect()
    {
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        attractivePoint = transform.position;
        
        Destroy(Instantiate(DebugSphere, attractivePoint, Quaternion.identity),2f);
        
        Collider[] colliders = Physics.OverlapSphere(attractivePoint, 4.4f, interactMask);
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
                    //EnnemyWindAttraction(col.gameObject);;
                }
            }
            if (col.CompareTag("Ennemy"))
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    EnnemyWindAttraction(col.gameObject);
                    i++;
                }
            }
            else
            {
                EnnemyWindAttraction(col.gameObject);
            }
        }
        
        Destroy(gameObject);
    }

    private void EnnemyWindAttraction(GameObject enemy)
    {
        enemy.transform.DOKill();
                    
        var shootPointPos = (enemy.transform.position - transform.position);
        var targetPos = new Vector3((enemy.transform.position.x + shootPointPos.x) /* forceModifier*/, 
            enemy.transform.position.y + shootPointPos.y + 1f,
            (enemy.transform.position.z + shootPointPos.z) /* forceModifier*/);
        
        enemy.transform.DOMove(targetPos, 1f).OnComplete(() => enemy.transform.DOKill());
        Debug.Log($"{enemy.name} got attracted !");
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
        
        if (other.transform.CompareTag("Interactable"))
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
    
    private IEnumerator StopMovement(float timeToStopMovement, GameObject objTransform)
    {
        yield return new WaitForSeconds(timeToStopMovement);
        objTransform.transform.DOKill();
    }

    private void OnDestroy()
    {
        CardsController.instance.StartCoroutine(CardsController.instance.LaunchCardCD(4));
        CardsController.isWindGround = false;
        UIManager.Instance.UpdateCardUI();
    }
}
