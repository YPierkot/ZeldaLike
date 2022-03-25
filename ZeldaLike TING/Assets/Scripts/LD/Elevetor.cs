using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Elevetor : InteracteObject
{
    private BoxCollider collider;
    
    [Header("==== ELEVETOR ====")]
    [SerializeField] private Transform platform;
    [SerializeField] private Transform[] passPoint;
    [SerializeField] private float waitingTime = 2f;
    [SerializeField] private float speed = 0.05f;
    [SerializeField] private bool auto;
    [Space] 
    [SerializeField] private float boxHeight = 1;
    private int pointIndex = 0;
    private List<Transform> eleveteList = new List<Transform>();

    private bool waiting;
    private bool moving;
    
    void Start()
    {
       if(auto)Move();
       collider = GetComponent<BoxCollider>();
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(platform.transform.position, passPoint[pointIndex].position)> speed)
        {
            Vector3 movement = (passPoint[pointIndex].position - platform.position).normalized * speed;
            platform.position += movement;
            collider.center += movement;
            //Debug.Log($"New Position : {platform.position}");
            foreach (Transform obj in eleveteList)
            {
                if (!obj.CompareTag("Player"))
                {
                    if (obj != null) obj.position += movement;
                    else eleveteList.Remove(obj);
                }
                else
                {
                    obj.position += new Vector3(movement.x, 0, movement.z);
                }
            }
        }
        else if (!waiting && auto) StartCoroutine("Waiter");
        else
        {
            moving = false;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3(platform.position.x, platform.position.y+boxHeight, platform.position.z), transform.localScale);
    }

    public void Move()
    {
        //Debug.Log("Move Launch");
        if (!moving)
        {
            if (pointIndex == passPoint.Length - 1) pointIndex = 0;
            else pointIndex++;
            waiting = false;
            moving = true;
            /*eleveteList.Clear();
           Collider[] eleveteObject= Physics.OverlapBox(new Vector3(platform.position.x, platform.position.y+boxHeight, platform.position.z), transform.localScale/2);
           foreach (Collider col in eleveteObject)
           {
              if(col.transform != platform) eleveteList.Add(col.transform );
           }*/
        }

    }
    
    IEnumerator Waiter()
    {
        waiting = true;
        yield return new WaitForSeconds(waitingTime);
        Move();
        
    }


    private void OnTriggerEnter(Collider other)
    {
       if(other.transform != platform) eleveteList.Add(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        eleveteList.Remove(other.transform);
    }
}

