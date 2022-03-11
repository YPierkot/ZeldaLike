using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Elevetor : MonoBehaviour
{
    [SerializeField] private Transform platform;
    [SerializeField] private Transform[] passPoint;
    [SerializeField] private float waitingTime = 2f;
    [SerializeField] private float speed = 0.05f;
    [SerializeField] private bool auto;
    private int pointIndex = 0;

    private bool waiting;
    private bool moving;
    
    void Start()
    {
       if(auto)Move();
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(platform.transform.position, passPoint[pointIndex].position)> speed)
        {
            platform.transform.position += (passPoint[pointIndex].position - platform.transform.position).normalized * speed;
        }
        else if (!waiting && auto) StartCoroutine("Waiter");
        else
        {
            moving = false;
        }
    }

    void Move()
    {
        if (!moving)
        {
            if (pointIndex == passPoint.Length - 1) pointIndex = 0;
            else pointIndex++;
            waiting = false;
            moving = true;
        }

    }

    IEnumerator Waiter()
    {
        waiting = true;
        yield return new WaitForSeconds(waitingTime);
        Move();
        
    }
}
