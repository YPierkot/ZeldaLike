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

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!dashing)
        {
            if (!changingPoint) transform.position = Vector3.Lerp(transform.position, cameraPoint.position, cameraSpeed*0.1f);
            else
            {
                transform.position = Vector3.Lerp(transform.position, cameraPoint.position, changePointSpeed*0.1f*(1/Vector3.Distance(transform.position, cameraPoint.position)*2));
                Debug.Log("Slow change");
            }
        }

        if (changingPoint)
        {
            if (Vector3.Distance(transform.position, cameraPoint.position) < 0.5f)
            {
                changingPoint = false;
            }
        }
    }

    public void ChangePoint(Transform newTransform)
    {
        changingPoint = true;
        cameraPoint = newTransform;
    }
}
