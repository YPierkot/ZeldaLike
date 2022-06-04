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
    public float groundY()
    {
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit);
        return hit.point.y;
    }
    private float _groundY;
    public bool stayIdle;
    [SerializeField] private int maxLife; 
    public int life;
    [SerializeField] private Transform shield;

    [SerializeField] private BossState currentState = BossState.idle;
    [HideInInspector] public Transform TransformTP_Zone;
    [Space]
    [SerializeField] private Vector2 sizeAttackZone;
    [SerializeField] private Vector2 distanceFromPlayer;

    private bool idleStart;
    private int idleCount = 0;
    [HideInInspector] public bool isFreeze;
    private bool castAttack;
    private bool tpNext;

    [Header("TP")] 
    [SerializeField] private Vector2 sizeTP_Zone;
    [SerializeField] private bool DebugTP_Zone;
    [SerializeField] private GameObject teleportProjectilePrefab;
     private ParticleSystem[] teleportProjectiles;
    [SerializeField] private float speedProjTP; 
    [SerializeField] private float maxDistanceProjTP; 
     private float distanceProjTP; 
    private bool teleporting;
    private bool moveProjTP;
    

    [Header("---LASER")] 
    [SerializeField] private float laserTimer = 5;
    public float laserSpeed;
    [SerializeField] private float laserLenght =1f;

    private Transform laser;
    private ParticleSystem laserLine;
    private LineRenderer laserLineR;
    private bool laserStart;
    private bool laserStartThrow;
    private Vector3 laserPos;
    [HideInInspector] public bool castingLaser;
    private float _laserTimer;
    private bool waitLaserState;
    
    [Header("---BALL")]
    [SerializeField] float ballSpeed;
    [SerializeField] private Vector2 ballAmountRange;
    private int ballsAmount;
    [Space]
    [SerializeField] private GameObject[] balls;
    private List<Vector3> ballPosList;
    [SerializeField] GameObject attackWarning;
    [SerializeField] private GameObject[] warnings;
    [Space]
    [SerializeField] private float warningTimerCD = .5f;
    [SerializeField] private float ballThrowTimer;
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
        life = maxLife;
        _groundY = groundY();
        UIManager.Instance.BosslifeBar.maxValue = maxLife;
        animator = GetComponentInChildren<Animator>();
        laserLine = GetComponentInChildren<ParticleSystem>();
        laserLineR = GetComponentInChildren<LineRenderer>();
        laser = laserLineR.transform;
        shieldMesh = shield.GetComponent<MeshRenderer>();
        invincibleColor = shieldMesh.material.color;
        
        laserLine.Stop();

        boss = transform.GetChild(0);
        TransformTP_Zone = transform.GetChild(1);
        
        warnings = new GameObject[(int)ballAmountRange.y];
        for (int i = 0; i < warnings.Length; i++)
        {
            warnings[i] = Instantiate(attackWarning, transform);
            warnings[i].SetActive(false);
        }

        teleportProjectiles = new ParticleSystem[5];
        for (int i = 0; i < 5; i++)
        {
            teleportProjectiles[i] = Instantiate(teleportProjectilePrefab, boss.position, Quaternion.identity, boss).GetComponent<ParticleSystem>();
            teleportProjectiles[i].gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (moveProjTP)
        {
            
            foreach (var proj in teleportProjectiles)
            {
                
                proj.transform.position += proj.transform.forward * speedProjTP;
            }
            distanceProjTP += speedProjTP;
            if (distanceProjTP > maxDistanceProjTP) moveProjTP = false;

        }
        
        #region StateMachine

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

        #endregion
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
            else if (idleCount >= 2) currentState = BossState.Tp;
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
            TeleportAttack();
            teleporting = false;
            castAttack = true;
            currentState = BossState.idle;
        }
    }

    void TeleportAttack()
    {
        float offset = Random.Range(0f, 360f);
        for (int i = 0; i < 5; i++)
        {
            Vector3 dir = new Vector3(0, (360*i / 5) + offset, 0);
            teleportProjectiles[i].transform.position = boss.position;
            teleportProjectiles[i].transform.rotation = Quaternion.Euler(dir);
            teleportProjectiles[i].gameObject.SetActive(true);
            teleportProjectiles[i].Play(true);
        }
        moveProjTP = true;
    }

    #endregion

    #region LaserAttack

    void LaserAttackUpdate()
    {
        if (!laserStart)
        {
            idleCount = 0;
            laserStart = true;
            laserLenght = 0f;
            laserStartThrow = false;
            castingLaser = true;
            SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.bossLaserCast);
            animator.SetTrigger("LaserAttack");
            laserLineR.enabled = true;
            laser.gameObject.SetActive(true);
            _laserTimer = laserTimer;
            Debug.Log(Controller.instance.transform.position);
            laserPos = Controller.instance.transform.position;
            laser.rotation = Quaternion.LookRotation(Controller.instance.transform.position-boss.position);
            laserLineR.SetPosition(0, laser.position);
            laserLineR.SetPosition(1, laser.position);
        }
        else if (_laserTimer >= 0)
        {
            Vector3 rayDir = laser.forward;
            if (!castingLaser)
            {
                if (!laserStartThrow)
                {
                    SoundEffectManager.Instance.StopSound(SoundEffectManager.Instance.sounds.bossLaserCast);
                    SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.bossLaser, loop:true);
                    laserLineR.enabled = false;
                    laserLine.Play();
                    laserStartThrow = true;
                }

                //laserLenght
                
                _laserTimer -= Time.deltaTime;
            }
            else
            {
                if (Physics.Raycast(boss.position, rayDir, out RaycastHit hit, laserLenght))
                {
                    if(hit.transform.CompareTag("Player"))laserLineR.SetPosition(1, hit.point);
                }
                else laserLineR.SetPosition(1, (laser.position + laser.forward*50));

                if (!waitLaserState)
                {
                    StartCoroutine(laserState());
                    waitLaserState = true;
                }
            }
            
        }
        else
        {
            SoundEffectManager.Instance.StopSound(SoundEffectManager.Instance.sounds.bossLaser);
            currentState = BossState.idle;
            laserStart = false;
            //laserLine.enabled = false;
            laser.gameObject.SetActive(false);
        }
    }

    #endregion

    #region BallAttack

    private void BallAttackStart()
    {
       Debug.Log("Ball Start");
        idleCount = 0; 
        ballCastFinish = false; 
        warningCD = false; 
        canThrow = true;
        animator.SetTrigger("BallAttack"); 
        ballsAmount = Random.Range((int)ballAmountRange.x, ((int)ballAmountRange.y + 1)); 
        ballQueue = new Queue<BallSets>(); 
        ballPosList = new List<Vector3>();
        ballStart = true; 
    }
    
    void BallAttackUpdate()
    {
        if (!ballStart)BallAttackStart();
        else if(!ballCastFinish)
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

                Vector3 newBallPos = Vector3.zero;
                bool canSpawn = false;
                int i = 0;
                while (!canSpawn)
                {
                    newBallPos = new Vector3(Random.Range(-(distanceFromPlayer.x), distanceFromPlayer.x), 0, Random.Range(-distanceFromPlayer.y, distanceFromPlayer.y)) + Controller.instance.transform.position;
                    newBallPos.y = 0.1f;
                    
                    /*if (newBallPos.x > sizeAttackZone.x) newBallPos.x = sizeAttackZone.x;
                    if (newBallPos.x < -sizeAttackZone.x) newBallPos.x = -sizeAttackZone.x;
                    if (newBallPos.z > sizeAttackZone.y) newBallPos.z = sizeAttackZone.y;
                    if (newBallPos.z < -sizeAttackZone.y) newBallPos.z = -sizeAttackZone.y;
                    */
                    

                    canSpawn = true;
                    if (ballPosList.Count == 0) break;
                    foreach (var pos in ballPosList)
                    {
                        if (Vector3.Distance(newBallPos, pos) < 1.5f)
                        {
                            canSpawn = false;
                            break;
                        }
                    }

                    i++;
                    if (i >= 50) {Debug.LogError("WhileBreak"); break;}
                }
                ballPosList.Add(newBallPos);
                
                newBall.ball = balls[index];
                newBall.pos = newBallPos;
                newBall.warning = warnings[ballQueue.Count];
                
                newBall.warning.SetActive(true);
                newBall.warning.transform.position = newBall.pos;
                
                ballQueue.Enqueue(newBall);
                
                //Debug.Log($"Generate ball n°{ballQueue.Count} at {newBall.pos}");
                warningCD = true;
                StartCoroutine(warningCoolDown());
                
            }
        }
        else if (ballQueue.Count != 0)
        {
            if (canThrow)
            {
                //Debug.Log("Throw");
                BallSets ballSet = ballQueue.Dequeue();
                GameObject newBall = Instantiate(ballSet.ball, new Vector3(boss.position.x, 0, boss.position.z), Quaternion.identity);
                SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.bossProjectilShoot);
                //Debug.Log($"TP Zone Pos :{TransformTP_Zone.position} + ballSet.pos : {ballSet.pos} = {TransformTP_Zone.position + ballSet.pos}");
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
        
        if (TransformTP_Zone != null) Gizmos.DrawWireCube(TransformTP_Zone.position, new Vector3(sizeAttackZone.x, 0, sizeAttackZone.y) * 2);
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

    IEnumerator laserState()
    {
        yield return new WaitForSeconds(0.1f);
        if (castingLaser) laserLineR.enabled = !laserLineR.enabled;
        waitLaserState = false;
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
