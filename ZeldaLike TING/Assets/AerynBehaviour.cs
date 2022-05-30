using System;
using System.Collections.Generic;
using PathCreation;
using PathCreation.Examples;
using UnityEngine;

public class AerynBehaviour : MonoBehaviour
{
    public bool isFreed;
    [SerializeField] private PathFollower path;
    [SerializeField] private float playerSlowDistance;
    [SerializeField] private float slowSpeed;
    [SerializeField] private float playerMaxDistance;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float playerMinDistance;
    [SerializeField] private float maxSpeed;
    private float distancefromPlayer;
    public bool firstPath = true;
    public bool isThirdPath;
    [SerializeField] private PathCreator secondPath;
    [SerializeField] private PathCreator thirdPath;
    [SerializeField] private GameObject shield;
    private Vector3 lastPos;

    private void Update()
    {
        if (isFreed && firstPath)
        {
            distancefromPlayer = Vector3.Distance(transform.position, Controller.instance.transform.position);
            if (distancefromPlayer > playerMaxDistance)
            {
                path.enabled = false;
            }
            else if (distancefromPlayer > playerSlowDistance && distancefromPlayer < playerMaxDistance)
            {
                path.enabled = true;
                path.speed = slowSpeed;
            }
            else if(distancefromPlayer < playerSlowDistance && distancefromPlayer > playerMinDistance)
            {
                path.enabled = true;
                path.speed = normalSpeed;
            }
            else if(distancefromPlayer < playerMinDistance)
            {
                path.enabled = true;
                path.speed = maxSpeed;
            }
            transform.position = path.transform.position;
        }

        if (isFreed && !firstPath && !isThirdPath)
        {
            path.pathCreator = secondPath;
            path.speed = 3;
            path.enabled = true;
            transform.position = path.transform.position;
        }

        if (isThirdPath)
        {
            path.pathCreator = thirdPath;
            path.speed = 3;
            path.enabled = true;
            transform.position = path.transform.position;
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, playerMaxDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerMinDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, playerSlowDistance);
    }
}
