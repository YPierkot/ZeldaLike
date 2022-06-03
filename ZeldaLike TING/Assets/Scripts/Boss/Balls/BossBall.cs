using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBall : MonoBehaviour
{
    private float maxSpeed = 0.01f;
    [SerializeField] private float speedModifier;
    [SerializeField] private float heightMultiplicator;
    private float currentDistance;
    private float totalDistance;
    private bool ballLaunch;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    private GameObject warnerObject;

    private bool impacted;

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
            else if(!impacted)
            {
                ballEffect();
            }

        }
    }

    public void LaunchBall(Vector3 target,float speed, GameObject warner)
    { 
        Debug.Log("Ball Launch to " + target);
        startPosition = transform.position;
        targetPosition = target - startPosition;
        Vector2 target2DPosition = new Vector2(target.x, target.z).normalized;
        totalDistance = Vector2.Distance(new Vector2(startPosition.x, startPosition.z), target2DPosition);
        ballLaunch = true;

        maxSpeed = speed;
        warnerObject = warner;

    }

    void ballEffect()
    {
        SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.bossProjectilImpact);
        warnerObject.SetActive(false);
        StartCoroutine("Destroyed", 2f);
        impacted = true;
    }

    IEnumerable Destroyed(float destroyTime)
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
