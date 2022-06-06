using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCrate : MonoBehaviour
{
    public GameObject fxDestroyCrate;

    private void OnDestroy()
    {
        GameObject fxCrate = PoolManager.Instance.PoolInstantiate(PoolManager.Object.fxDestroyCrate);
        fxCrate.transform.position = transform.position + new Vector3(0, .4f, 0);
    }
}
