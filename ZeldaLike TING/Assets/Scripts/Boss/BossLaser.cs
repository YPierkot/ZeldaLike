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
    }

    // Update is called once per frame
    void Update()
    {
        //if(Controller.instance != null)transform.LookAt(Controller.instance.transform);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation((Controller.instance.transform.position - transform.position).normalized, transform.up), speed/Vector3.Angle(transform.forward, (Controller.instance.transform.position - transform.position).normalized));
    }
}