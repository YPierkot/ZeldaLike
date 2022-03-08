using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TreeEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CameraController : MonoBehaviour
{
    public Transform cameraPoint;

    [SerializeField] private float cameraSpeed;
    [SerializeField] private float changePointSpeed;

    private bool changingPoint;
    

    [NonSerialized] public bool dashing = false;
    private bool moveToPlayer;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (changingPoint)
        {
           if(moveToPlayer) transform.position = Vector3.Lerp(transform.position, cameraPoint.position, changePointSpeed*0.1f*(1/Vector3.Distance(transform.position, cameraPoint.position)*15));
           else transform.position = Vector3.Lerp(transform.position, cameraPoint.position, changePointSpeed*0.1f);
            Debug.Log("Slow change");
            
            if (Vector3.Distance(transform.position, cameraPoint.position) < 0.3f)
            {
                changingPoint = false;
            }
        }
        else if (!dashing)
        {
            transform.position = Vector3.Lerp(transform.position, cameraPoint.position, cameraSpeed*0.1f);
        }

    }

    public void ChangePoint(Transform newTransform, bool _moveToPlayer = false)
    {
        changingPoint = true;
        cameraPoint = newTransform;
        moveToPlayer = _moveToPlayer;

    }
}
