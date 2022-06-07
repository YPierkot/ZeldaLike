using System.Collections;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace AI
{
    public class KamikazeAI : AbstractAI
    {
        #region Variables
        [Header("Specific Values"), Space]
        [SerializeField] private float e_rangeWander = 3.7f;
        [SerializeField] private float e_aoeRange = 1.4f;
        private float groundDst = 1f;
        private Vector3 basePosition;
        
        [SerializeField] private bool isMoving;
        [SerializeField] private bool isAttacking;
        [SerializeField] private bool canMove;
        [Space(2)] [SerializeField] private Animator kamikazeAnimator;
        private float spriteDir;
        [SerializeField] private GameObject fxExplode;
        #endregion
        
        protected override void Init()
        {
            base.Init();
            basePosition = transform.position;
            isAttacking = false;
            canMove = true;
        }
        
        public float debugX;
        public float debugZ;
        
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
            
            kamikazeAnimator.SetBool("isWalk", isMoving);
        }
        
        protected override void Attack()
        {
            if (isFreeze) return;
            base.Attack();

            if (Vector3.Distance(playerTransform.position, transform.position) <= e_rangeAttack)
            {
                if (isAttacking) return;
                if (isHitStun) return;
                
                // Attack Pattern
                isAttacking = true;
                if(e_currentAiState != AIStates.dead || !isFreeze) StartCoroutine(DoAttack());
            }
            else
            {
                if (!isAttacking)
                {
                    transform.DOKill();
                    GoToPlayer();
                    
                    kamikazeAnimator.SetBool("isWalk", true);
                    
                    RaycastHit groundHit;
                    if (Physics.Raycast(transform.position, Vector3.down, out groundHit, 0.2f, groundLayerMask)) transform.position = groundHit.point + new Vector3(0, 0.1f, 0);
                    else transform.position += new Vector3(0, -0.1f, 0);
                    Debug.DrawRay(transform.position, Vector3.down*1, Color.blue);
                }
                else
                {
                    kamikazeAnimator.SetBool("isWalk", false);
                }
                
                spriteDir = playerTransform.position.x - transform.position.x;

                if (spriteDir < 0) e_sprite.flipX = true;
                else e_sprite.flipX = false;
            }
        }

        private void GoToPlayer()
        {
            RaycastHit collisionHit;
            
            Vector3 dir = new Vector3(playerTransform.position.x - transform.position.x, playerTransform.position.y - transform.position.y,
                playerTransform.position.z - transform.position.z).normalized;
            
            Debug.DrawRay(transform.position, dir);
            
            if (Physics.Raycast(transform.position, dir, out collisionHit, Vector3.Distance(playerTransform.position, transform.position) * 1.5f, groundLayerMask))
            {
                Debug.DrawLine(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), 
                    new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z), Color.red);

                float pointX = collisionHit.point.x;
                float pointZ = collisionHit.point.z;
                
                debugX = (playerTransform.position.x - transform.position.x);
                debugZ = (playerTransform.position.z - transform.position.z);

                if (debugZ > 0) // Joueur au dessus
                {
                    if (debugX > 0) transform.position = Vector3.MoveTowards(transform.position, new Vector3( pointX + 0.3f, 0,pointZ + 3f),e_speed * Time.deltaTime);
                    else transform.position = Vector3.MoveTowards(transform.position, new Vector3(pointX - 3f, 0,pointZ + 0.3f), e_speed * Time.deltaTime);
                }
                else // Joueur en dessous
                {
                    if (debugX > 0) transform.position = Vector3.MoveTowards(transform.position, new Vector3(pointX + 3f, 0,pointZ + 0.3f), e_speed * Time.deltaTime);
                    else transform.position = Vector3.MoveTowards(transform.position, new Vector3(pointX - 2f, 0,pointZ - 1f), e_speed * Time.deltaTime);
                }
            }
            else
            {
                Debug.DrawLine(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z), Color.green);
                transform.position = Vector3.MoveTowards(transform.position, playerTransform.position,
                    e_speed * Time.deltaTime);
            }
        }
        
        private IEnumerator DoAttack()
        {
            e_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            
            kamikazeAnimator.SetBool("isAttack", true);
            SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.boomerExplosion);
            yield return new WaitForSeconds(.11f);

            var fxKamikaze = PoolManager.Instance.PoolInstantiate(PoolManager.Object.fxKamikaze);
            fxKamikaze.transform.position = transform.position;
            
            yield return new WaitForSeconds(0.72f);
            bool isPlayer = Physics.CheckSphere(transform.position, e_aoeRange, playerLayerMask);
            if (isPlayer && GetComponent<AI.AbstractAI>().e_currentAiState != AIStates.dead) PlayerStat.instance.TakeDamage();
            
            Destroy(gameObject);
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
