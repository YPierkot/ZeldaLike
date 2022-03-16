using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace AI
{
    public class KamikazeAI : AbtractAI
    {
        #region Variables
        [Header("Specific Values"), Space]
        [SerializeField] private float e_rangeWander = 3.7f;
        [SerializeField] private float e_aoeRange = 1.4f;
        private Vector3 basePosition;
        
        [SerializeField] private bool isMoving;
        [SerializeField] private bool isAttacking;
        [SerializeField] private bool canMove;
        [Space(2)] [SerializeField] private Animator kamikazeAnimator;
        private float spriteDir;
        #endregion
        
        protected override void Init()
        {
            base.Init();
            basePosition = transform.position;
            isAttacking = false;
            canMove = true;
        }

        public override void ChangeState(AIStates aiState)
        {
            base.ChangeState(aiState);
            
            if (aiState == AIStates.walking)
                basePosition = transform.position;
        }

        protected override void Walk()
        {
            Debug.Log("Wander State");
            
            if (isAttacking)
                return;
            if (isMoving)
                return;
            
            base.Walk();
            
            isMoving = true;
            
            Vector3 newMoveTarget = new Vector3(Random.Range(basePosition.x - e_rangeWander, basePosition.x + e_rangeWander), basePosition.y, 
                Random.Range(basePosition.z - e_rangeWander, basePosition.z + e_rangeWander));
            
            e_transform.DOMove(newMoveTarget, 1.8f).OnComplete(() => isMoving = false);
            
            kamikazeAnimator.SetBool("isWalk", isMoving);
        }
        
        protected override void Attack()
        {
            base.Attack();
            Debug.Log(isAttacking);
            Debug.Log(isMoving);
            
            Debug.Log("Attacking State");
            
            if (Vector3.Distance(playerTransform.position, transform.position) <= e_rangeAttack)
            {
                if (isAttacking)
                    return;
                isAttacking = true;
                
                // Attack Pattern
                StartCoroutine(DoAttack());
            }
            else
            {
                if (!isAttacking)
                {
                    transform.DOKill();
                    transform.position = Vector3.MoveTowards(transform.position, playerTransform.position,
                        e_speed * Time.deltaTime);
                    kamikazeAnimator.SetBool("isWalk", true);
                }
                else
                {
                    kamikazeAnimator.SetBool("isWalk", false);
                }
                
                spriteDir = playerTransform.position.x - transform.position.x;

                if (spriteDir < 0)
                    e_sprite.flipX = true;
                else
                    e_sprite.flipX = false;
            }
        }

        private IEnumerator DoAttack()
        {
            kamikazeAnimator.SetBool("isAttack", true);
            yield return new WaitForSeconds(0.95f);
            kamikazeAnimator.SetBool("isAttack", false);
            Collider[] playercol = Physics.OverlapSphere(transform.position, e_aoeRange, playerLayerMask);
            foreach (var player in playercol)
            {
                player.GetComponent<PlayerStat>().TakeDamage();
            }
            
            Destroy(gameObject);
        }

        private IEnumerator StopMob()
        {
            canMove = false;
            Debug.Log(canMove);
            yield return new WaitForSeconds(1f);
            canMove = true;
            Debug.Log(canMove);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, e_rangeWander);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, e_rangeSight);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, e_rangeAttack);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, e_aoeRange);
            
        }
    }
}
