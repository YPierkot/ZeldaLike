using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Scripting;
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

    [Serializable]class RuneDict
    {
        public runeType type;
        public GameObject rune;
    }
    
    public enum BossState
    {
        idle,
        Tp,
        lasetAttack,
        ballAttack
    }

    enum runeType
    {
        fire, ice, wind
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
    [SerializeField] private LastCinematicManager LastCinematicManager;
    [Header("---RUNE")] 
    [SerializeField] private RuneDict[] runes;
    public List<GameObject> activeRune;

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
    private int ballCount;
    [Space]
    [SerializeField] private GameObject[] balls;

    public List<Vector3> ballPosList;
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
    private bool onShield;
    [SerializeField] Color destroyableColor ;
    [SerializeField] Color invincibleColor ;
    

    //Animation
    private Animator animator;

    void Start()
    {
        life = maxLife;
        onShield = true;
        _groundY = 0.95f;// groundY();
        UIManager.Instance.maxLife = maxLife;
        animator = GetComponentInChildren<Animator>();
        laserLine = GetComponentInChildren<ParticleSystem>();
        laserLineR = GetComponentInChildren<LineRenderer>();
        laser = laserLineR.transform;
        
        laserLine.Stop();

        boss = transform.GetChild(0);
        TransformTP_Zone = transform.GetChild(1);
        
        warnings = new GameObject[(int)ballAmountRange.y];
        for (int i = 0; i < warnings.Length; i++)
        {
            warnings[i] = Instantiate(attackWarning);
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
        if ( 0 < life )
        {
            UIManager.Instance.BosslifeBar.transform.parent.gameObject.SetActive(true);
        }
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
        if(activeRune.Count != 0)
            foreach (var rune in activeRune)
            {
                rune.SetActive(false);
            }
        activeRune.Clear();
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
            Vector3 rdm = new Vector3(Random.Range(-sizeTP_Zone.x, sizeTP_Zone.x), 2f, Random.Range(-sizeTP_Zone.y, sizeTP_Zone.y));
            boss.position = TransformTP_Zone.position + rdm;
        }
    }

    public void EndTeleport()
    {
        
        if (teleporting)
        {
           
            SetupRune();
            
            TeleportAttack();
            teleporting = false;
            castAttack = true;
            currentState = BossState.idle;
        }
        else shield.gameObject.SetActive(true);
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
                
                if (Physics.Raycast(boss.position, rayDir, out RaycastHit hit, laserLenght))
                {
                    if(hit.transform.CompareTag("Player")) PlayerStat.instance.TakeDamage();
                    //else Debug.Log("Raycast hit " + hit.transform.name);
                }

                laserLenght++;
                
                _laserTimer -= Time.deltaTime;
            }
            else
            {
                laserLineR.SetPosition(1, (laser.position + laser.forward*50));

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
        idleCount = 0;
        ballCastFinish = false; 
        warningCD = false; 
        canThrow = true;
        animator.SetTrigger("BallAttack"); 
        ballsAmount = Random.Range((int)ballAmountRange.x, ((int)ballAmountRange.y + 1));
        ballCount = 0;
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
                if (ballCount == ballsAmount) 
                {
                    ballCastFinish = true;
                    animator.SetTrigger("BallThrow");
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
                newBall.pos = /*TransformTP_Zone.position +*/ newBallPos ;
                newBall.warning = warnings[ballCount];

                ballCount++;
                StartCoroutine(BallDelayLaunch(newBall, ballThrowTimer));
                
                //ballQueue.Enqueue(newBall);
                
                //Debug.Log($"Generate ball n°{ballQueue.Count} at {newBall.pos}");
                warningCD = true;
                StartCoroutine(warningCoolDown());
                
            }
        }
        /*else if (ballQueue.Count != 0)
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
        }*/
        else
        {
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
        if (!onShield)
        {
            //shield.gameObject.SetActive(false);
            StartCoroutine(FreezeCD());
        }
    }

    void SetupRune()
    {
        onShield = true;
        shield.gameObject.SetActive(true);
        activeRune.Clear();
        int i = Random.Range(0, runes.Length);
        for (int j = 0; j < runes.Length; j++)
        {
            if (i != j)
            {
                Debug.Log("Set " + j);
                GameObject currentRune = runes[j].rune;
                activeRune.Add(currentRune);
               
                float xFactor = Random.value <= 0.5f ? -1 : 1;
                float yFactor = Random.value <= 0.5f ? -1 : 1;
                Vector3 vec = new Vector3(Random.Range(2f, 5f)*xFactor, TransformTP_Zone.position.y-boss.position.y, Random.Range(2f, 5f)*yFactor);
                if (activeRune.Count != 0)
                {
                    int breaker = 0;
                    Debug.Log($"Pos1 : {activeRune[0].transform.position}, Pos 2 : {vec + boss.position}, Distance : {Vector3.Distance(activeRune[0].transform.position, vec+boss.position)}");
                    while (Vector3.Distance(activeRune[0].transform.position, vec + boss.position) < 3f)
                    {
                        xFactor = Random.value <= 0.5f ? -1 : 1;
                        yFactor = Random.value <= 0.5f ? -1 : 1;
                        vec = new Vector3(Random.Range(2f, 5f)*xFactor, TransformTP_Zone.position.y-boss.position.y, Random.Range(2f, 5f)*yFactor);
                        Debug.Log("distance : " + Vector3.Distance(activeRune[0].transform.position, vec));
                        breaker++;
                        if (breaker == 50) break;
                    }
                }
                currentRune.transform.position = vec + boss.position;
                LineRenderer line = currentRune.GetComponent<LineRenderer>();
                line.SetPosition(0,currentRune.transform.GetChild(0).GetChild(0).position);
                line.SetPosition(1,boss.position);
                currentRune.SetActive(true);
            }
        }
    }
    
    public void RuneDestroy(GameObject rune)
    {
        activeRune.Remove(rune);
        rune.SetActive(false);
        if (activeRune.Count == 0)
        {
            onShield = false;
            shield.gameObject.SetActive(false);
        }
        else Debug.Log("Still " + activeRune[0].name);
    }

    public void TakeDamage(int damage)
    {
        //Debug.Log("Damage Boss");
        if (!onShield)
        {
            life -= damage;
            UIManager.Instance.BossLifeUpdate(life);
            if (life <= 0) Death();
        }
        else tpNext = true;
    }

    void Death()
    {
        Debug.Log("Dead");
        LastCinematicManager.StartCoroutine(LastCinematicManager.LastCinematic());
        enabled = false;
    }

    IEnumerator FreezeCD()
    {
        isFreeze = true;
        animator.speed /= 2;
        yield return new WaitForSeconds(5f);
        isFreeze = false;
        if (currentState == BossState.idle) currentState = BossState.Tp;
        animator.speed *= 2;

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

    IEnumerator BallDelayLaunch(BallSets ballSet, float delayTime)
    {
        ballSet.warning.SetActive(true);
        ballSet.warning.transform.position = ballSet.pos;
        
        yield return new WaitForSeconds(delayTime);
        
        GameObject newBall = Instantiate(ballSet.ball, new Vector3(boss.position.x, boss.position.y, boss.position.z), Quaternion.identity);
        SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.bossProjectilShoot);
        newBall.GetComponent<BossBall>().LaunchBall(ballSet.pos, ballSpeed, ballSet.warning);
    }
}
