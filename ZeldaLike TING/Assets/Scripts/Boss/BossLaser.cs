using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    private float speed ;
    private void OnEnable()
    {
        speed = GetComponentInParent<BossManager>().laserSpeed;
        if(Controller.instance != null)transform.LookAt(Controller.instance.transform);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(Controller.instance.transform.position); //Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Controller.instance.transform.position), speed);
    }
}
