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
    [SerializeField] private Transform firstDestination;
    public bool firstPath = true;
    public bool isThirdPath;
    public bool isFourthPath;
    [SerializeField] private PathFollower secondPath;
    [SerializeField] private PathFollower thirdPath;
    [SerializeField] private PathFollower fourthPath;
    [SerializeField] public GameObject shield;
    private Animator anim;
    private Vector3 lastPos;


    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (!GameManager.Instance.isTutorial)
        {
            if (isFreed && firstPath)
            {
                        distancefromPlayer = Vector3.Distance(transform.position, Controller.instance.transform.position);
                        if (Vector3.Distance(transform.position, firstDestination.position) > Vector3.Distance(Controller.instance.transform.position, firstDestination.position))
                        {
                            path.enabled = true;
                            path.speed = maxSpeed;
                            transform.position = path.transform.position;
                            return;
                        }
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
                secondPath.speed = 3;
                secondPath.enabled = true;
                transform.position = secondPath.transform.position;
            }
            
            if (isThirdPath)
            {
                thirdPath.speed = 4.5f;
                thirdPath.enabled = true;
                transform.position = thirdPath.transform.position;
            }
            
            if (isFourthPath)
            {
                fourthPath.speed = 4.5f;
                fourthPath.enabled = true;
                transform.position = fourthPath.transform.position;
            }
        }
        
        
        Vector3 posDiff = transform.position - lastPos;
        lastPos = transform.position;
        
        if (posDiff.z <= -0.01f && posDiff.z != 0)
        {
            anim.ResetTrigger("AerynSide");
            anim.ResetTrigger("AerynUp");
            anim.SetTrigger("AerynDown");
        }
        else if (posDiff.x >= 0.01f && posDiff.x != 0)
        {
            anim.ResetTrigger("AerynUp");
            anim.ResetTrigger("AerynDown");
            anim.SetTrigger("AerynSide");
            anim.GetComponent<SpriteRenderer>().flipX = true;
        }

        else if (posDiff.x <= -0.01f && posDiff.x != 0)
        {
            anim.GetComponent<SpriteRenderer>().flipX = false;
            anim.ResetTrigger("AerynUp");
            anim.ResetTrigger("AerynDown");
            anim.SetTrigger("AerynSide");
        }

        else if (posDiff.z >= 0.01f && posDiff.z != 0)
        {
            anim.ResetTrigger("AerynSide");
            anim.ResetTrigger("AerynDown");
            anim.SetTrigger("AerynUp");
        }
        else
        {
            anim.ResetTrigger("AerynSide");
            anim.ResetTrigger("AerynUp");
            anim.SetTrigger("AerynDown");
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
