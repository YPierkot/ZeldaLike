using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossManager : MonoBehaviour
{
    public enum BossState
    {
        idle, Tp, lasetAttack, ballAttack 
    }

    private Transform boss;

    [SerializeField] private Transform shield;
    
    [SerializeField] private BossState currentState = BossState.idle;
    [HideInInspector] public Transform TransformTP_Zone;

    private bool isFreeze;
    
    [Header("TP")] 
    private bool teleporting;
    [Space]
    [SerializeField] private Vector2 sizeTP_Zone;
    [SerializeField] private bool DebugTP_Zone;
    //Animation
    private Animator animator;
    
    void Start()
    {
        boss = transform.GetChild(0);
        Debug.Log(boss);
        TransformTP_Zone = transform.GetChild(1);
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        switch (currentState)
        {
          case BossState.idle         : IdleUpdate(); break;
          case BossState.Tp           : TpUpdate(); break;
          case BossState.lasetAttack  : LaserAttackUpdate(); break;
          case BossState.ballAttack   : BallAttackUpdate(); break;
        }
    }

    #region Idle

    void IdleUpdate()
    {
        
    }

    #endregion

    #region TP

    void TpUpdate()
    {
        Debug.Log("TP Update");
        if (!teleporting) LaunchTeleport();
    }
    

    private void LaunchTeleport()
    {
        shield.gameObject.SetActive(false);
        teleporting = true;
        animator.SetTrigger("StartTP");
    }

    public void Teleport()
    {
        if (teleporting)
        {
            Debug.Log("TP");
            Vector3 rdm = new Vector3(Random.Range(-sizeTP_Zone.x, sizeTP_Zone.x), transform.position.y, Random.Range(-sizeTP_Zone.y, sizeTP_Zone.y));
            boss.position = TransformTP_Zone.position + rdm;
        }
    }

    public void EndTeleport()
    {
        Debug.Log("Finish Appear");
        shield.gameObject.SetActive(true);
        if (teleporting)
        {
            teleporting = false;
            RandomAttack();
        }
    }

    #endregion

    #region LaserAttack

    void LaserAttackUpdate()
    {
        Debug.Log("Laser Update");
        currentState = BossState.idle;
    }

    #endregion

    #region BallAttack

    void BallAttackUpdate()
    {
        Debug.Log("Ball Update");
        currentState = BossState.idle;
    }

    #endregion

    void RandomAttack()
    {
        if (Random.value <= 0.5f) currentState = BossState.ballAttack;
        else currentState = BossState.lasetAttack;
    }
    
    private void OnDrawGizmos()
    {
        if(DebugTP_Zone && TransformTP_Zone != null)Gizmos.DrawWireCube(TransformTP_Zone.position, new Vector3(sizeTP_Zone.x, 0, sizeTP_Zone.y)*2);
    }

    public void Freeze()
    {
        Debug.Log("FreezeAffect");
        if (currentState == BossState.idle)
        {
            Debug.Log("Freeze");
            shield.gameObject.SetActive(false);
            FreezeCD();
        }
    }

    IEnumerator FreezeCD()
    {
        
        isFreeze = true;
        animator.speed /= 2;
        yield return new WaitForSeconds(5f);
        isFreeze = false;
        animator.speed *= 2;
        shield.gameObject.SetActive(true);
        
    }
    
}
