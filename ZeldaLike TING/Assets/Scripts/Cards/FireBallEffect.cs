using System;
using AI;
using UnityEngine;

public class FireBallEffect : MonoBehaviour
{
    public LayerMask mask; //Ennemy & Interact
    
    public void ActivateRedGroundEffect()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2, mask);
        foreach (var col in colliders)
        {
            switch (col.transform.tag)
            {
                case "Interactable": col.GetComponent<InteracteObject>().OnFireEffect(); break;
                case "Ennemy": if (col.isTrigger) col.GetComponent<AI.AbstractAI>().LooseHp(2); break;
            }
        }
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.CompareTag("Player"))
        {
            ActivateRedGroundEffect();
        }
    }
}
