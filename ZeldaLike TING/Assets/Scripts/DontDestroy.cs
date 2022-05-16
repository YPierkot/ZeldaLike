using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static DontDestroy instantiate;

    private void Awake()
    {
        if (instantiate == null) instantiate = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        DontDestroyOnLoad(this);
    }
}
