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
    private bool deactivate = false;

    private void OnEnable()
    {
        
        StartCoroutine(DelayStart());
    }

    public virtual void Update()
    {
        if (actualScale <= maxScale && start && !deactivate)
        {
            actualScale = transform.localScale.y;
            transform.localScale = new Vector3(transform.localScale.x, actualScale += appearSpeed,
                transform.localScale.z);
        }

        if (deactivate && actualScale >= 0) 
        {
            actualScale = transform.localScale.y;
            transform.localScale = new Vector3(transform.localScale.x, actualScale -= appearSpeed *2,
                transform.localScale.z);
        }

        if (actualScale <= 0.2f && deactivate)
        {
            gameObject.SetActive(false);
        }
        
    }

    private IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(2f);
        GetComponent<MeshRenderer>().enabled = true;
        start = true;
    }

    private void OnDisable()
    {
        deactivate = true;
    }
}
