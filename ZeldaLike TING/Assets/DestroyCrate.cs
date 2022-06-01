using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCrate : MonoBehaviour
{
    public GameObject fxDestroyCrate;

    private void OnDestroy()
    {
        Destroy(Instantiate(fxDestroyCrate, transform.position + new Vector3(0,.4f,0), Quaternion.identity), 1f);
    }
}
