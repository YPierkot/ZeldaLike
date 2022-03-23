using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace AI
{
    public class SwingerAI : AbtractAI
    {
        #region Variables

        [Header("Specific Values"), Space]
        [SerializeField] private float e_rangeWander = 4f;
        [SerializeField] private float e_aoeRange = 1f;
        private Vector3 basePosition;
        
        [SerializeField] private bool isMoving;
        [SerializeField] private bool isAttacking;
        [Space(2)] [SerializeField] private Animator swingerAnimator;
        private bool debugBool;
        private Vector3 dir;
        private float spriteDir;
        #endregion
        
        protected override void Init()
        {
            base.Init();
            basePosition = transform.position;
            isAttacking = false;
        }

        public override void ChangeState(AIStates aiState)
        {
            base.ChangeState(aiState);
            
            if (aiState == AIStates.walking)
                basePosition = transform.position;
        }
        
        protected override void Walk()
        {
            if(isAttacking)
                return;
            
            base.Walk();
            
            if(isMoving)
                return;
            
            isMoving = true;
            swingerAnimator.SetBool("isWalk", true);
            
            Vector3 newMoveTarget = new Vector3(Random.Range(basePosition.x - e_rangeWander, basePosition.x + e_rangeWander), basePosition.y, 
                Random.Range(basePosition.z - e_rangeWander, basePosition.z + e_rangeWander));
            
            e_transform.DOMove(newMoveTarget, 1.8f).OnComplete(() => isMoving = false);
            
            if (!isMoving)
            {
                swingerAnimator.SetBool("isWalk", false);
            }
        }

        protected override void Attack()
        {
            base.Attack();

            if (Vector3.Distance(playerTransform.position, transform.position) <= e_rangeAttack)
            {
                if (isAttacking)
                    return;
                
                isAttacking = true;
                isMoving = false;
                
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
                    swingerAnimator.SetBool("isWalk", true);
                    
                    RaycastHit groundHit;
                    if (Physics.Raycast(transform.position, Vector3.down, out groundHit, 0.2f, groundLayerMask)) transform.position = groundHit.point + new Vector3(0, 0.1f, 0);
                    else transform.position += new Vector3(0, -0.1f, 0);
                    Debug.DrawRay(transform.position, Vector3.down*1, Color.blue);
                }
                else
                {
                    swingerAnimator.SetBool("isWalk", false);
                }
                
                spriteDir = playerTransform.position.x - transform.position.x;

                if (spriteDir < 0)
                    e_sprite.flipX = true;
                else
                    e_sprite.flipX = false;
            }
        }

        private const float radiusShootPoint = 1.9f;
        private IEnumerator DoAttack()
        {
            e_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            
            dir = playerTransform.position - transform.position;
            dir.Normalize();
            
            if (dir.x < 0)
                e_sprite.flipX = true;
            else
                e_sprite.flipX = false;
            
            swingerAnimator.SetBool("isAttack", true);

            Debug.DrawRay(transform.position, dir*2, Color.green, 1f);
            
            yield return new WaitForSeconds(.53f); // Temps de l'animation avant hit 

            Collider[] playercol = Physics.OverlapSphere(transform.position + dir * radiusShootPoint, e_aoeRange, playerLayerMask);
            foreach (var player in playercol)
            {
                player.GetComponent<PlayerStat>().TakeDamage();
            }

            yield return new WaitForSeconds(1.17f); // Anim fini 
            swingerAnimator.SetBool("isAttack", false);
            CanMove();  
            
            yield return new WaitForSeconds(1.5f); // Temps avant de pouvoir ré attaqué
            isAttacking = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, e_rangeWander); // Zone of Wandering
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, e_rangeSight); // Zone of player detection
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, e_rangeAttack); // Zone of attack range
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, radiusShootPoint); // Zone de spawn de l'aoe
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position + dir * radiusShootPoint, e_aoeRange); // Zone aoe
        }

        private void CanMove()
        {
            e_rigidbody.constraints = RigidbodyConstraints.None;
            e_rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
            e_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}
