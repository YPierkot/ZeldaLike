using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCardGroundEffect : MonoBehaviour
{
    [SerializeField] private LayerMask Ennemy;
    
    public void ActivateRedGroundEffect()
    {
        Debug.Log("Fireball Short Range Shoot");
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5, Ennemy);
        foreach (var col in colliders)
        {
            switch (col.transform.tag)
            {
                case "Interactable":
                    col.GetComponent<InteracteObject>().Burn();
                    break;
                
                case "Ennemy" :
                    col.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                    col.gameObject.GetComponent<ResetColor>().StartCoroutine(col.gameObject.GetComponent<ResetColor>().ResetObjectColor());
                    break;
                    
            }
            
        }
        
        Destroy(gameObject);
    }
}
