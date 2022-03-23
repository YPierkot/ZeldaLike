using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCardGroundEffect : MonoBehaviour
{
    [SerializeField] private LayerMask interactMask;
    [SerializeField] float repulsivePower = 500f;
    [SerializeField] float repulsiveRadius = 4f;
    [SerializeField] Vector3 repulsivePoint;

    private void Start()
    {
        repulsivePoint = transform.position;
    }

    public void ActivateWindGroundEffect()
    {
        Debug.Log("Wind Short Range Launch");
        repulsivePoint = transform.position;
        Collider[] cols = Physics.OverlapSphere(transform.position, 3, interactMask);
        foreach (var col in cols)
        {
            if (col.transform.CompareTag("Interactable"))
            {
                if (col.GetComponent<GemWindPuzzle>() != null)
                {
                    col.GetComponent<GemWindPuzzle>().WindInteract();
                }
            }
            else 
            {
                col.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
                col.gameObject.GetComponent<ResetColor>()
                    .StartCoroutine(col.gameObject.GetComponent<ResetColor>().ResetObjectColor());
                col.gameObject.GetComponent<Rigidbody>()
                    .AddExplosionForce(repulsivePower, transform.position, repulsiveRadius, 2);
            }
        }
    }
}
