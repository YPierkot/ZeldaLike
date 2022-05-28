using System;
using AI;
using UnityEngine;

public class FireBallEffect : MonoBehaviour
{
    public LayerMask interactMask;
    public LayerMask groundMask; //Ennemy & Interact
    
    public void ActivateRedGroundEffect()
    {
        Debug.LogFormat("acces");
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2, interactMask);
        foreach (var col in colliders)
        {
            switch (col.transform.tag)
            {
                case "Interactable": col.GetComponent<InteracteObject>().OnFireEffect(); break;
                case "Ennemy": col.GetComponent<AI.AbstractAI>().LooseHp(2); break;
            }
        }
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponentInParent<Transform>().CompareTag("Player") && !other.isTrigger)
        {
            ActivateRedGroundEffect();
            Debug.Log("oui");
        }
    }
}
