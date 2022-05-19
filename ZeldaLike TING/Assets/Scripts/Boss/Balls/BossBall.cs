using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBall : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 0.01f;
    [SerializeField] private float heightMultiplicator;
    [SerializeField] private float destroyTime;
    private float currentDistance;
    private float totalDistance;
    private bool ballLaunch;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector2 target2DPosition;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (ballLaunch)
        {
            if (currentDistance <= 1)
            {
                float speed = maxSpeed;
                currentDistance += speed;
                Vector3 position = startPosition + new Vector3( targetPosition.x*currentDistance, Mathf.Cos((currentDistance - 0.5f)*Mathf.PI) * heightMultiplicator, targetPosition.z*currentDistance);
                transform.position = position;
            }

        }
    }

    public void LaunchBall(Vector3 target)
    {
        Debug.Log("Ball Launch to " + target);
        startPosition = transform.position;
        targetPosition = target - startPosition;
        target2DPosition = new Vector2(target.x, target.z).normalized;
        totalDistance = Vector2.Distance(new Vector2(startPosition.x, startPosition.z), target2DPosition);
        ballLaunch = true;
    }

    IEnumerable Destroyed()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
