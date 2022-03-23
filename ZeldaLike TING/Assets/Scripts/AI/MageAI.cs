using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace AI
{
    public class MageAI : AbtractAI
    {
        #region Variables
        [Header("Specific Values"), Space]
        [SerializeField] private float e_rangeWander = 3.7f;
        private Vector3 basePosition;
        
        [SerializeField] private bool isMoving;
        [SerializeField] private bool isAttacking;
        [SerializeField] private bool canMove;
        [Space(2)] [SerializeField] private Animator mageAnimator;
        private float spriteDir;

        [Header("Eye Spawning"), Space] 
        [SerializeField] private Vector2 eyePosOffset;
        [SerializeField] private GameObject eyeGameObject;
        [SerializeField] private byte eyeCounter; 
        
        #endregion
        
        protected override void Init()
        {
            base.Init();
            basePosition = transform.position;
            isAttacking = false;
            eyeCounter = 3;
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
            
            mageAnimator.SetBool("isWalk", isMoving);
        }

        protected override void Attack()
        {
            base.Attack();
            if (Vector3.Distance(playerTransform.position, transform.position) <= e_rangeAttack)
            {
                if (isAttacking)
                    return;
                isAttacking = true;
                
                // Attack Pattern
                if (eyeCounter > 0)
                {
                    StartCoroutine(DoAttack());
                }
            }
            else
            {
                if (!isAttacking)
                {
                    transform.DOKill();
                    transform.position = Vector3.MoveTowards(transform.position, playerTransform.position,
                        e_speed * Time.deltaTime);
                    
                    RaycastHit groundHit;
                    if (Physics.Raycast(transform.position, Vector3.down, out groundHit, 0.5f, groundLayerMask)) transform.position = groundHit.point + new Vector3(0, 0.2f, 0);
                    else transform.position += new Vector3(0, -0.2f, 0);
                    Debug.DrawRay(transform.position, Vector3.down*1, Color.blue);
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
           
            mageAnimator.SetBool("isAttack", true);
            yield return new WaitForSeconds(1.75f); // Temps de l'anim
            mageAnimator.SetBool("isAttack", false);
            if (e_sprite.flipX == true)
            { 
                Instantiate(eyeGameObject,
                    new Vector3(transform.position.x - eyePosOffset.x, transform.position.y + eyePosOffset.y,
                        transform.position.z), Quaternion.identity);
            }
            else
            {
                Instantiate(eyeGameObject,
                    new Vector3(transform.position.x + eyePosOffset.x, transform.position.y + eyePosOffset.y,
                        transform.position.z), Quaternion.identity);
            }

            eyeCounter -= 1;
            yield return new WaitForSeconds(2f);
            isAttacking = false;
        }
        
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, e_rangeWander);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, e_rangeSight);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, e_rangeAttack);
        }
    }
}
