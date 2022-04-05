using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCardGroundEffect : MonoBehaviour
{
    [SerializeField] private LayerMask Ennemy;
    
    public void ActivateIceGroundEffect()
    {
        float zTransform = transform.position.z;
        float xTransform = transform.position.x;
        float yTransform = transform.position.y;
        
        Debug.Log("Ice Spike Launch");
        Collider[] cols = Physics.OverlapCapsule(new Vector3(xTransform, yTransform, zTransform + 3.7f), new Vector3(xTransform, yTransform, zTransform + 8), 2.5f, Ennemy);
        foreach (var ennemy in cols)
        {
            // Apply IceShortRange effect
            ennemy.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        Destroy(gameObject, 0.2f);
    }
}
