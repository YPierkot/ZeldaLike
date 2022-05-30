using System.Collections;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace AI
{
    public class MageAI : AbstractAI
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
        [SerializeField] private bool isPanic;

        public float debugX;
        public float debugZ;
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
            if (isAttacking) return;
            if (isMoving) return;
            if (isHitStun) return;

            
            base.Walk();
            
            isMoving = true;
            
            Vector3 newMoveTarget = new Vector3(Random.Range(basePosition.x - e_rangeWander, basePosition.x + e_rangeWander), basePosition.y, 
                Random.Range(basePosition.z - e_rangeWander, basePosition.z + e_rangeWander));
            
            e_transform.DOMove(newMoveTarget, 1.8f).OnComplete(() => isMoving = false);
        }

        protected override void Attack()
        {
            if (isFreeze) return;
            base.Attack();
            if (isHitStun) return;

            
            if (Vector3.Distance(playerTransform.position, transform.position) <= e_rangeAttack)
            {
                if (isAttacking) return;
                if (isPanic) return;
                    
                if (eyeCounter == 0) { StartCoroutine(PanicPhase(4f)); return; }
                
                // Attack Pattern
                if (isHitStun) return;
                isAttacking = true;
                if(e_currentAiState != AIStates.dead || !isFreeze) StartCoroutine(DoAttack());
                
            }
            else
            {
                if (isHitStun) return;
                if (!isAttacking)
                {
                    transform.DOKill();
                    GoToPlayer();
                    
                    RaycastHit groundHit;
                    if (Physics.Raycast(transform.position, Vector3.down, out groundHit, 0.4f, groundLayerMask)) transform.position = groundHit.point + new Vector3(0, 0.2f, 0);
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

        private IEnumerator PanicPhase(float panicTime)
        {
            isPanic = true;
            mageAnimator.SetBool("isPanic", isPanic);
            yield return new WaitForSeconds(panicTime);
            isPanic = false;
            mageAnimator.SetBool("isPanic", isPanic);
            eyeCounter = 3;
        }

        private IEnumerator DoAttack()
        {
            mageAnimator.SetBool("isAttack", true);
            yield return new WaitForSeconds(1.75f); // Temps de l'anim
            mageAnimator.SetBool("isAttack", false);
            if (e_sprite.flipX == true)
            {
                if (GetComponent<AI.AbstractAI>().e_currentAiState != AIStates.dead)
                {
                    Instantiate(eyeGameObject,
                        new Vector3(transform.position.x - eyePosOffset.x, transform.position.y + eyePosOffset.y,
                            transform.position.z), Quaternion.identity);
                }
            }
            else
            {
                if (GetComponent<AI.AbstractAI>().e_currentAiState != AIStates.dead)
                {
                    Instantiate(eyeGameObject,
                        new Vector3(transform.position.x + eyePosOffset.x, transform.position.y + eyePosOffset.y,
                            transform.position.z), Quaternion.identity);
                }
            }

            eyeCounter -= 1;
            yield return new WaitForSeconds(2f);
            isAttacking = false;
        }
        
         private void GoToPlayer()
        {
            RaycastHit collisionHit;
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z), out collisionHit, 
                Vector3.Distance(playerTransform.position, transform.position), groundLayerMask))
            {
                Debug.DrawLine(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z), Color.red);

                float pointX = collisionHit.point.x;
                float pointZ = collisionHit.point.z;
                
                debugX = (playerTransform.position.x - transform.position.x);
                debugZ = (playerTransform.position.z - transform.position.z);

                if (debugZ > 0) // Joueur au dessus
                {
                    if (debugX > 0) // Joueur à droite
                    {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3( pointX - 2.5f, 0,pointZ + 1f),
                            e_speed * Time.deltaTime);
                    }
                    else // Joueur à gauche
                    {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(pointX + 2f, 0,pointZ + 1f),
                            e_speed * Time.deltaTime);
                    }
                }
                else // Joueur en dessous
                {
                    if (debugX > 0) // Joueur à droite
                    {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(pointX + 2f, 0,pointZ - .7f),
                            e_speed * Time.deltaTime);
                    }
                    else // Joueur à gauche
                    {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(pointX - 2f, 0,pointZ - .7f),
                            e_speed * Time.deltaTime);
                    }
                }
            }
            else
            {
                Debug.DrawLine(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z), Color.green);
                transform.position = Vector3.MoveTowards(transform.position, playerTransform.position,
                    e_speed * Time.deltaTime);
            }
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
