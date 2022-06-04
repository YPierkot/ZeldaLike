using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossBall : MonoBehaviour
{
    public enum EffectType
    {
        None, Fire, Wind, Rock, Ice
    }
    
    [SerializeField] private float speedModifier = 1;
    [SerializeField] private float heightMultiplicator;
    [Space]
    [SerializeField] public EffectType ballType;

    // FIRE
    private bool damage;
    
    // WIND
    [SerializeField] private float repulseForce;
    private bool repulse;
    
    // ROCK
    [SerializeField] private Transform rockTransform;
    
    //ICE 
    private bool slowPlayer;
    
    private MeshRenderer mesh;
    private Collider collider;
    public GameObject particles;
    
    private float maxSpeed = 0.01f;
    private float currentDistance;
    private float totalDistance;
    private bool ballLaunch;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 target;

    private GameObject warnerObject;

    private bool impacted;

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();

        switch (ballType)
        {
            case EffectType.Fire :
                particles = transform.GetChild(0).gameObject;
                break;
        }
    }

    private void Update()
    {
        if (ballLaunch)
        {
            if (currentDistance <= 1)
            {
                float speed = maxSpeed*speedModifier;
                currentDistance += speed;
                Vector3 position = startPosition + new Vector3( targetPosition.x*currentDistance, Mathf.Cos((currentDistance - 0.5f)*Mathf.PI) * heightMultiplicator, targetPosition.z*currentDistance);
                transform.position = position;
            }
            else if(!impacted)
            {
                
                transform.position = target;
                ballEffect();
            }

        }
    }

    public void LaunchBall(Vector3 _target,float speed, GameObject warner)
    { 
        Debug.Log("Ball Launch to " + _target);
        target = _target;
        startPosition = transform.position;
        targetPosition = _target - startPosition;
        Vector2 target2DPosition = new Vector2(_target.x, _target.z).normalized;
        totalDistance = Vector2.Distance(new Vector2(startPosition.x, startPosition.z), target2DPosition);
        ballLaunch = true;

        maxSpeed = speed;
        warnerObject = warner;

    }

    void ballEffect()
    {
        SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.bossProjectilImpact);
        warnerObject.SetActive(false);
        mesh.enabled = false;
        impacted = true;
        
        if(collider != null)collider.enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);

        switch (ballType)
        {
            case EffectType.Fire : FireEffect();
                break;
            case EffectType.Wind : WindEffect();
                break;
            
            default : StartCoroutine(Destroyed());
                break;
        }
    }

    void FireEffect()
    {
        StartCoroutine(Destroyed(1.5f));
    }

    void WindEffect()
    {
        StartCoroutine(Destroyed(1.5f));
    }

    void RockEffect()
    {
        rockTransform.rotation = quaternion.Euler(0, Random.Range(0f,360f), 0);
        StartCoroutine(Destroyed(2));
    }
    

    IEnumerator Destroyed(float time = 2f)
    {
        yield return new WaitForSeconds(time);
        if (slowPlayer) Controller.instance.isSlow = false;
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if(!other.CompareTag("Player")) return;
        
        switch (ballType)
        {
            case EffectType.Fire :
                if(!damage)PlayerStat.instance.TakeDamage();
                damage = true;
                break;
            
            
            case EffectType.Wind :
                if (!repulse)
                {
                    Debug.Log("Repusle");
                    Vector3 dir = (other.transform.position - transform.position).normalized;
                    Controller.instance.rb.AddForce(dir*repulseForce);
                    repulse = true;
                }
                break;
            
            case EffectType.Ice :
                Controller.instance.isSlow = true; 
                slowPlayer = true; 
                break; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("Player")) return;
        repulse = false;
        damage = false;
        slowPlayer = false;
        Controller.instance.isSlow = false;
    }
}
