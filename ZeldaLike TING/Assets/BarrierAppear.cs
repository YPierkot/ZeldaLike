using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierAppear : MonoBehaviour
{
    [SerializeField] private float appearSpeed;
    private float actualScale;
    [SerializeField] private float maxScale;
    private bool start;

    private void Start()
    {
        
        StartCoroutine(DelayStart());
    }

    private void Update()
    {
        if (actualScale <= maxScale && start)
        {
            actualScale = transform.localScale.y;
            transform.localScale = new Vector3(transform.localScale.x, actualScale += appearSpeed,
                transform.localScale.z);
        }
        
    }

    private IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(2f);
        GetComponent<MeshRenderer>().enabled = true;
        start = true;
    }
}
