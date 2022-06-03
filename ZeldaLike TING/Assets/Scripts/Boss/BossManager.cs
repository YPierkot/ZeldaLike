using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class BossManager : MonoBehaviour
{

    class BallSets
    {
        public GameObject ball;
        public Vector3 pos;
        public GameObject warning;
    }
    public enum BossState
    {
        idle,
        Tp,
        lasetAttack,
        ballAttack
    }

    private Transform boss;
    public bool stayIdle;
    [SerializeField] private int maxLife; 
    public int life;
    [SerializeField] private Transform shield;

    [SerializeField] private BossState currentState = BossState.idle;
    [HideInInspector] public Transform TransformTP_Zone;

    private bool idleStart;
    [SerializeField] private int idleCount = 0;
    public bool isFreeze;
    [SerializeField] private bool castAttack;
    private bool tpNext;

    [Header("TP")] private bool teleporting;
    [Space] [SerializeField] private Vector2 sizeTP_Zone;
    [SerializeField] private bool DebugTP_Zone;

    [Header("---LASER")] 
    [SerializeField] private float laserTimer = 5;
    public float laserSpeed;
    [SerializeField] private float laserLenght =1f;

    private Transform laser;
    private LineRenderer laserLine;
    private bool laserStart;
    private bool laserStartThrow;
    private Vector3 laserPos;
    [HideInInspector] public bool castingLaser;
    private float _laserTimer;
    
    [Header("---BALL")]
    [SerializeField] float ballSpeed;
    [SerializeField] private Vector2 ballAmountRange;
    private int ballsAmount;
    [SerializeField] private float warningTimerCD = .5f;
    [SerializeField] private float ballThrowTimer;
    [Space]
    [SerializeField] private GameObject[] balls;
    [SerializeField] GameObject attackWarning;
    [SerializeField] private GameObject[] warnings;
    private bool ballStart;
    private bool ballCastFinish;
    private bool warningCD;
    
    private Queue<BallSets> ballQueue;
    
    [HideInInspector] public bool canThrow;

    [Header("---SHIELD")] 
    private MeshRenderer shieldMesh;
    [SerializeField] Color destroyableColor ;
    [SerializeField] Color invincibleColor ;
    

    //Animation
    private Animator animator;

    void Start()
    {
        warnings = new GameObject[(int)ballAmountRange.y];
        for (int i = 0; i < warnings.Length; i++)
        {
            warnings[i] = Instantiate(attackWarning, transform);
            warnings[i].SetActive(false);
        }
        
        life = maxLife;
        UIManager.Instance.BosslifeBar.maxValue = maxLife;
        animator = GetComponentInChildren<Animator>();
        laserLine = GetComponentInChildren<LineRenderer>();
        laser = laserLine.transform;
        shieldMesh = shield.GetComponent<MeshRenderer>();
        invincibleColor = shieldMesh.material.color;

        boss = transform.GetChild(0);
        TransformTP_Zone = transform.GetChild(1);
    }

    void Update()
    {
        switch (currentState)
        {
            case BossState.idle:
                IdleUpdate();
                break;
            case BossState.Tp:
                TpUpdate();
                break;
            case BossState.lasetAttack:
                LaserAttackUpdate();
                break;
            case BossState.ballAttack:
                BallAttackUpdate();
                break;
        }
    }

    #region Idle

    void IdleUpdate()
    {
        if (!idleStart)
        {
            shieldMesh.material.color = destroyableColor;
            idleStart = true;
        }
    }

    public void EndIdle()
    {
        if (currentState == BossState.idle && !stayIdle)
        {
            if (tpNext) currentState = BossState.Tp;
            else if (idleCount >= 3) currentState = BossState.Tp;
            else if (castAttack) RandomAttack();
            else if (DialogueManager.Instance != null)
            {
                if (!DialogueManager.Instance.isCinematic) idleCount++;
            }
            else idleCount++;
        }
    }

    #endregion

    #region TP

    void TpUpdate()
    {
        if (!teleporting) LaunchTeleport();
    }
    
    private void LaunchTeleport()
    {
        Debug.Log("TP");
        idleCount = 0;
        tpNext = false;
        shield.gameObject.SetActive(false);
        teleporting = true;
        animator.SetTrigger("StartTP");
        SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.bossTP);
        
    }

    public void Teleport()
    {
        if (teleporting)
        {
            Vector3 rdm = new Vector3(Random.Range(-sizeTP_Zone.x, sizeTP_Zone.x), transform.position.y,
                Random.Range(-sizeTP_Zone.y, sizeTP_Zone.y));
            boss.position = TransformTP_Zone.position + rdm;
        }
    }

    public void EndTeleport()
    {
        shield.gameObject.SetActive(true);
        if (teleporting)
        {
            teleporting = false;
            castAttack = true;
            currentState = BossState.idle;
        }
    }

    #endregion

    #region LaserAttack

    void LaserAttackUpdate()
    {
        if (!laserStart)
        {
            idleCount = 0;
            laserStart = true;
            laserStartThrow = false;
            castingLaser = true;
            SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.bossLaserCast);
            animator.SetTrigger("LaserAttack");
            laserLine.enabled = true;
            laser.gameObject.SetActive(true);
            _laserTimer = laserTimer;
            Debug.Log(Controller.instance.transform.position);
            laserPos = Controller.instance.transform.position;
            laser.rotation = Quaternion.LookRotation(Controller.instance.transform.position-boss.position);
            laserLine.SetPosition(0, laser.position);
            laserLine.SetPosition(1, laser.position);
        }
        else if (_laserTimer >= 0)
        {
            /*laserPos = Vector3.Lerp(laserPos, Controller.instance.transform.position, laserSpeed / Vector3.Distance(laserPos, Controller.instance.transform.position));
            Vector3 rayDir = (laserPos - boss.position).normalized;
            rayDir = new Vector3(rayDir.x, 0, rayDir.z);*/
            Vector3 rayDir = laser.forward;//new Vector3(laser.forward.x, 1, laser.forward.z);
            //Vector3 rayDir = new Vector3(laser.forward.x, laser.forward.y, laser.forward.z);
            if (!castingLaser)
            {
                if (!laserStartThrow)
                {
                    SoundEffectManager.Instance.StopSound(SoundEffectManager.Instance.sounds.bossLaserCast);
                    SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.bossLaser, loop:true);
                    laserStartThrow = true;
                }

                if (Physics.Raycast(boss.position, rayDir, out RaycastHit hit, Mathf.Infinity))
                {
                    if (hit.transform.CompareTag("Player")) PlayerStat.instance.TakeDamage();
                    else laserLine.SetPosition(1, hit.point);
                }
                else laserLine.SetPosition(1, (laser.position +laser.forward*laserLenght));
                
                _laserTimer -= Time.deltaTime;
            }
        }
        else
        {
            SoundEffectManager.Instance.StopSound(SoundEffectManager.Instance.sounds.bossLaser);
            currentState = BossState.idle;
            laserStart = false;
            laserLine.enabled = false;
            laser.gameObject.SetActive(false);
        }
    }

    #endregion

    #region BallAttack

    void BallAttackUpdate()
    {
        if (!ballStart)
        {
            Debug.Log("Ball Start");
            idleCount = 0;
            ballCastFinish = false;
            warningCD = false;
            canThrow = true;
            animator.SetTrigger("BallAttack");
            ballsAmount = Random.Range((int)ballAmountRange.x, ((int)ballAmountRange.y + 1));
            ballQueue = new Queue<BallSets>();
            
            ballStart = true;
        }
        else if( !ballCastFinish)
        {
            if (!warningCD)
            {
                if (ballQueue.Count == ballsAmount) 
                {
                    ballCastFinish = true;
                    animator.SetTrigger("BallThrow");
                    Debug.Log("ballQueue Size : " + ballQueue.Count);
                    return;
                }
                
                BallSets newBall = new BallSets();
            
                int index = Random.Range(0,balls.Length);
                
                newBall.ball = balls[index];
                newBall.pos = new Vector3(Random.Range(-sizeTP_Zone.x, sizeTP_Zone.x), 1, Random.Range(-sizeTP_Zone.y, sizeTP_Zone.y));
                newBall.warning = warnings[ballQueue.Count];
                
                newBall.warning.SetActive(true);
                newBall.warning.transform.position = newBall.pos;
                
                ballQueue.Enqueue(newBall);
                
                Debug.Log($"Generate ball n°{ballQueue.Count} at {newBall.pos}");
                    warningCD = true; 
                    StartCoroutine(warningCoolDown());
                
            }
        }
        else if (ballQueue.Count != 0)
        {
            if (canThrow)
            {
                Debug.Log("Throw");
                BallSets ballSet = ballQueue.Dequeue();
                GameObject newBall = Instantiate(ballSet.ball, new Vector3(boss.position.x, 0, boss.position.z), Quaternion.identity);
                SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.bossProjectilShoot);
                newBall.GetComponent<BossBall>().LaunchBall(TransformTP_Zone.position + ballSet.pos, ballSpeed, ballSet.warning);
                canThrow = false;
                StartCoroutine(ballThrowCD());
            }
        }
        else
        {
            Debug.Log("Ball End");
            animator.SetTrigger("BallEnd");
            currentState = BossState.idle;
            ballStart = false;
        }
    }

    #endregion

    void RandomAttack()
    {
        if(stayIdle) return;
        idleStart = false;
        shieldMesh.material.color = invincibleColor;
        if (Random.value <= 0.5f) currentState = BossState.lasetAttack;
        else currentState = BossState.ballAttack;
        castAttack = false;
    }

    private void OnDrawGizmos()
    {
        if (DebugTP_Zone && TransformTP_Zone != null) Gizmos.DrawWireCube(TransformTP_Zone.position, new Vector3(sizeTP_Zone.x, 0, sizeTP_Zone.y) * 2);

        //if (boss != null) Gizmos.DrawLine(boss.position, laserPos);
        Gizmos.color = Color.red;
        if(laser != null) Gizmos.DrawRay(laser.position, laser.forward);
    }

    public void Freeze()
    {
        Debug.Log("FreezeAffect");
        if (currentState == BossState.idle)
        {
            Debug.Log("Freeze");
            shield.gameObject.SetActive(false);
            StartCoroutine(FreezeCD());
        }
    }

    public void TakeDamage(int damage)
    {
        //Debug.Log("Damage Boss");
        if (isFreeze)
        {
            life -= damage;
            UIManager.Instance.BossLifeUpdate(life);
            if (life <= 0) Death();
        }
        else tpNext = true;
    }

    void Death()
    {
        gameObject.SetActive(false);
    }

    IEnumerator FreezeCD()
    {
        isFreeze = true;
        animator.speed /= 2;
        yield return new WaitForSeconds(5f);
        isFreeze = false;
        if (currentState == BossState.idle) currentState = BossState.Tp;
        animator.speed *= 2;
        shield.gameObject.SetActive(true);

    }

    IEnumerator warningCoolDown()
    {
        yield return new WaitForSeconds(warningTimerCD);
        warningCD = false;
    }
    
    IEnumerator ballThrowCD()
    {
        yield return new WaitForSeconds(ballThrowTimer);
        canThrow = true;
    }
}
