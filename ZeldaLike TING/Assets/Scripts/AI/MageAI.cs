using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
                //StartCoroutine(DoAttack());
            }
            else
            {
                if (!isAttacking)
                {
                    transform.DOKill();
                    transform.position = Vector3.MoveTowards(transform.position, playerTransform.position,
                        e_speed * Time.deltaTime);
                    mageAnimator.SetBool("isWalk", true);
                }
                else
                {
                    mageAnimator.SetBool("isWalk", false);
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
            yield return new WaitForSeconds(0.5f); // Temps de l'anim
            //Vector3 eyePos = 
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
