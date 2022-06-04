using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    private void Start()
    {
        ///transform.position = new Vector3(transform.position.x, Controller.instance.transform.position.y, transform.position.z);
    }

    private float speed ;
    private void OnEnable()
    {
        speed = GetComponentInParent<BossManager>().laserSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //if(Controller.instance != null)transform.LookAt(Controller.instance.transform);
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation((Controller.instance.transform.position - transform.position).normalized, transform.up), speed/Vector3.Angle(transform.forward, (Controller.instance.transform.position - transform.position).normalized));
          transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Controller.instance.transform.position - transform.position), speed/Vector3.Angle(transform.forward,(Controller.instance.transform.position - transform.position).normalized) );
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.forward);
    }
}
