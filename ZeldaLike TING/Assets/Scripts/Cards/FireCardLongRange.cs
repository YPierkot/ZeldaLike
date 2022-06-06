using System;
using System.Collections;
using AI;
using UnityEngine;

public class FireCardLongRange : MonoBehaviour
{
    public LayerMask mask; //Ennemy & Interact
    public LayerMask groundMask;
    //public GameObject DebugSphere;
    public GameObject fireFX;


    public void FireCardLongEffect()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        StartCoroutine(FireCardLongEffectCo());
    }
    
    private IEnumerator FireCardLongEffectCo()
    {
        Destroy(Instantiate(fireFX, transform.position + new Vector3(0, -.5f,0), Quaternion.identity), 3.4f);
        yield return new WaitForSeconds(.22f);
        //Destroy(Instantiate(DebugSphere, transform.position, Quaternion.identity),1f);
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2.5f, mask);
        foreach (var col in colliders)
        {
            Debug.Log("Collide with " + col.name);
            switch (col.transform.tag)
            {
                case "Interactable": col.GetComponent<InteracteObject>().OnFireEffect(); break;
                case "Ennemy" : col.transform.GetComponent<AI.AbstractAI>().LooseHp(2); break;
            }
        }
        Destroy(gameObject);
    }
        
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponentInParent<Transform>().CompareTag("Player") && !other.isTrigger)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    
    private void OnDestroy()
    {
        CardsController.isFireGround = false;
        CardsController.instance.LaunchCardCD(1);
        UIManager.Instance.UpdateCardUI();
    }
}
