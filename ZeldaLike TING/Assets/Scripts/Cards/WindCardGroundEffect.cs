using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCardGroundEffect : MonoBehaviour
{
    [SerializeField] private LayerMask Ennemy;
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
        Collider[] cols = Physics.OverlapSphere(transform.position, 3, Ennemy);
        foreach (var col in cols)
        {
            col.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
            col.gameObject.GetComponent<ResetColor>().StartCoroutine(col.gameObject.GetComponent<ResetColor>().ResetObjectColor());
            col.gameObject.GetComponent<Rigidbody>().AddExplosionForce(repulsivePower, transform.position, repulsiveRadius, 2);
        }
        Destroy(gameObject, 0.2f);
    }
}
