using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

public class RedCardGroundEffect : MonoBehaviour
{
    [SerializeField] private LayerMask Ennemy;
    
    public void ActivateRedGroundEffect()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5, Ennemy);
        foreach (var col in colliders)
        {
            switch (col.transform.tag)
            {
                case "Interactable":
                    col.GetComponent<InteracteObject>().Burn();
                    break;
                
                case "Ennemy":
                    if (col.transform.GetComponent<SwingerAI>())
                    {
                        col.transform.GetComponent<SwingerAI>().LooseHp(2);
                    }
                    else if (col.transform.GetComponent<KamikazeAI>())
                    {
                        col.transform.GetComponent<KamikazeAI>().LooseHp(2);
                    }
                    else if (col.transform.GetComponent<MageAI>())
                    {
                        col.transform.GetComponent<MageAI>().LooseHp(2);
                    }
                    else if (col.transform.GetComponent<BomberAI>())
                    {
                        col.transform.GetComponent<BomberAI>().LooseHp(2);
                    }
                    break;
            }
        }
        
        Destroy(gameObject);
    }
}
