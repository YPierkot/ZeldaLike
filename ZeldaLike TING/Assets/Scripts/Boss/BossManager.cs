using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossManager : MonoBehaviour
{
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
    private int idleCount = 0;
    public bool isFreeze;
    [SerializeField] private bool castAttack;
    private bool tpNext;

    [Header("TP")] private bool teleporting;
    [Space] [SerializeField] private Vector2 sizeTP_Zone;
    [SerializeField] private bool DebugTP_Zone;

    [Header("---LASER")] 
    [SerializeField] private float laserTimer = 5;
    public float laserSpeed;

    private Transform laser;
    private LineRenderer laserLine;
    private bool laserStart;
    private bool laserStartThrow;
    private Vector3 laserPos;
    [HideInInspector] public bool castingLaser;
    private float _laserTimer;
    
    [Header("---BALL")]
    [SerializeField] private Vector2 ballAmountRange;
    [SerializeField] private float ballThrowTimer;
    [Space]
    [SerializeField] private GameObject[] balls;
    private bool ballStart;

    [Header("---SHIELD")] 
    private MeshRenderer shieldMesh;
    [SerializeField] Color destroyableColor ;
    [SerializeField] Color invincibleColor ;
    
    private Queue<GameObject> ballQueue;
    [HideInInspector] public bool canThrow;

    //Animation
    private Animator animator;

    void Start()
    {
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
        if (currentState == BossState.idle)
        {
            if (tpNext) currentState = BossState.Tp;
            else if (idleCount == 3) currentState = BossState.Tp;
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
            _laserTimer = laserTimer;
            Debug.Log(Controller.instance.transform.position);
            laserPos = Controller.instance.transform.position;
            laser.rotation = Quaternion.LookRotation(Controller.instance.transform.position);
            laserLine.SetPosition(0, boss.position);
            laserLine.SetPosition(1, boss.position);
        }
        else if (_laserTimer >= 0)
        {
            /*laserPos = Vector3.Lerp(laserPos, Controller.instance.transform.position, laserSpeed / Vector3.Distance(laserPos, Controller.instance.transform.position));
            Vector3 rayDir = (laserPos - boss.position).normalized;
            rayDir = new Vector3(rayDir.x, 0, rayDir.z);*/
            Vector3 rayDir = laser.forward;
            if (!castingLaser)
            {
                if (!laserStartThrow)
                {
                    SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.bossLaser);
                    laserStartThrow = true;
                }
                if (Physics.Raycast(boss.position, rayDir, out RaycastHit hit, Mathf.Infinity, 15))
                    laserLine.SetPosition(1, hit.point);
                else laserLine.SetPosition(1, rayDir * 100);
                _laserTimer -= Time.deltaTime;
            }
        }
        else
        {
            currentState = BossState.idle;
            laserStart = false;
            laserLine.enabled = false;
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
            ballStart = true;
            animator.SetTrigger("BallAttack");
            int ballsAmount = Random.Range((int)ballAmountRange.x, ((int)ballAmountRange.y + 1));
            ballQueue = new Queue<GameObject>();
            for (int i = 0; i < ballsAmount; i++)
            {
                int index = Random.Range(0,balls.Length);
                ballQueue.Enqueue(balls[index]);
            }
        }
        else if (ballQueue.Count != 0)
        {
            
            if (canThrow)
            {
                Debug.Log("Throw");
                GameObject newBall = Instantiate(ballQueue.Dequeue(), new Vector3(boss.position.x, 0, boss.position.z), Quaternion.identity);
                SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.bossProjectilShoot);
                Vector3 rdm = new Vector3(Random.Range(-sizeTP_Zone.x, sizeTP_Zone.x), 1, Random.Range(-sizeTP_Zone.y, sizeTP_Zone.y));
                newBall.GetComponent<BossBall>().LaunchBall(TransformTP_Zone.position + rdm);
                canThrow = false;
                StartCoroutine(ballThrowCD());
            }
            else Debug.Log("Ball Queue");
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
        else currentState = BossState.lasetAttack;
        castAttack = false;
    }

    private void OnDrawGizmos()
    {
        if (DebugTP_Zone && TransformTP_Zone != null) Gizmos.DrawWireCube(TransformTP_Zone.position, new Vector3(sizeTP_Zone.x, 0, sizeTP_Zone.y) * 2);

        if (boss != null) Gizmos.DrawLine(boss.position, laserPos);
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
        Debug.Log("Damage Boss");
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
        if (currentState == BossState.idle) tpNext = true;
        animator.speed *= 2;
        shield.gameObject.SetActive(true);

    }

    IEnumerator ballThrowCD()
    {
        yield return new WaitForSeconds(ballThrowTimer);
        canThrow = true;
    }
}
