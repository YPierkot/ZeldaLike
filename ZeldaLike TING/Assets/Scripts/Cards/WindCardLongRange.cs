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
    [HideInInspector] public Vector3 velocity;
    
    [SerializeField] private LayerMask interactMask;
    [SerializeField] private Vector3 attractivePoint;
    [SerializeField] private float attractiveRadius = 4.7f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private GameObject windFX;
    
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attractiveRadius);
    }
    public void WindCardLongEffect()
    {
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        attractivePoint = transform.position;
        
        Destroy(Instantiate(windFX, attractivePoint, Quaternion.identity),3f);
        
        Collider[] colliders = Physics.OverlapSphere(attractivePoint, attractiveRadius, interactMask);
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
                    col.GetComponent<InteracteObject>().OnWindEffect(CardsController.instance);
                    EnnemyWindAttraction(col.gameObject);;
                }
            }
            else if (col.CompareTag("Ennemy"))
            {
                if(!col.isTrigger) EnnemyWindAttraction(col.gameObject);
            }
        }
        
        Destroy(gameObject);
    }
    private void EnnemyWindAttraction(GameObject enemy)
    {
        enemy.transform.DOKill();
        enemy.transform.DOKill();
                    
        var shootPointPos = (enemy.transform.position - attractivePoint);
        var targetPos = new Vector3((enemy.transform.position.x - shootPointPos.x), 
            enemy.transform.position.y,
            (enemy.transform.position.z - shootPointPos.z));

        enemy.transform.DOMove(targetPos, 1.7f).OnComplete(() => enemy.transform.DOKill());
        Debug.Log($"{enemy.name} got attracted !");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponentInParent<Transform>().CompareTag("Player") && !other.isTrigger)
        {
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }
    
    private void OnDestroy()
    {
        CardsController.instance.LaunchCardCD(4);
        CardsController.isWindGround = false;
        UIManager.Instance.UpdateCardUI();
    }
}
