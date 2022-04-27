using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class pushWayPoint : MonoBehaviour
{
    public pushWayPoint topPoint;
    public pushWayPoint leftPoint;
    public pushWayPoint rightPoint;
    public pushWayPoint botPoint;
    [Space] public UnityEvent OnBlockEnter;
    
    
    [Header("---EDITOR Options")]
    public bool canSetWaypoint = true;

    private void Start()
    {
        if(GetComponent<MeshRenderer>() != null) GetComponent<MeshRenderer>().enabled = false;
        if(GetComponent<Collider>() != null) GetComponent<Collider>().enabled = false;
    }
}
