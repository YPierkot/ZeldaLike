using System.Collections;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace AI
{
    public class BomberAI : AbstractAI
    {
        #region Variables
        [Header("Specific values"), Space]
        [SerializeField] private GameObject bombPrefab;
        [SerializeField] private float e_rangeWander = 2;
        
        [SerializeField] private float e_fliSpeed = 1.7f;
        [SerializeField] private float e_fliRange = 1.7f;
        
        private Vector3 basePosition;
        private Vector3 wanderDir;
        private float spriteDir;

        [SerializeField] private Animator bomberAnimator;
        [SerializeField] private bool isMoving;
        [SerializeField] private bool isAttacking;
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
            base.Walk();

            if(isMoving)
                return;
            
            isMoving = true;

            Vector3 newMoveTarget = new Vector3(Random.Range(basePosition.x - e_rangeWander, basePosition.x + e_rangeWander), basePosition.y, 
                Random.Range(basePosition.z - e_rangeWander, basePosition.z + e_rangeWander));
            
            e_transform.DOMove(newMoveTarget, 1.8f).OnComplete(() => isMoving = false);
        }   
        
        protected override void Attack()
        {
            if (isFreeze) return;
            base.Attack();

            if (Vector3.Distance(playerTransform.position, transform.position) <= e_fliRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, -playerTransform.position, e_fliSpeed * Time.deltaTime);
                
                RaycastHit groundHit;
                if (Physics.Raycast(transform.position, Vector3.down, out groundHit, 1f, groundLayerMask)) transform.position = groundHit.point + new Vector3(0, 0.1f, 0);
                else transform.position += new Vector3(0, -0.1f, 0);
                Debug.DrawRay(transform.position, Vector3.down*1, Color.blue);
            }
            
            if (Vector3.Distance(playerTransform.position, transform.position) <= e_rangeAttack)
            {
                if (isAttacking)
                    return;
                
                isAttacking = true;
                isMoving = false;
            
                // Attack Pattern
                StartCoroutine(DoDropBomb());
            }
            else
            {
                if (!isAttacking)
                {
                    transform.DOKill();
                    transform.position = Vector3.MoveTowards(transform.position, playerTransform.position,
                        e_speed * Time.deltaTime);
                    
                    RaycastHit groundHit;
                    if (Physics.Raycast(transform.position, Vector3.down, out groundHit, 1f, groundLayerMask)) transform.position = groundHit.point + new Vector3(0, 0.1f, 0);
                    else transform.position += new Vector3(0, -0.1f, 0);
                    Debug.DrawRay(transform.position, Vector3.down*1, Color.blue);
                }

                spriteDir = playerTransform.position.x - transform.position.x;

                if (spriteDir < 0)
                    e_sprite.flipX = false;
                else
                    e_sprite.flipX = true;
            }
        }

        private IEnumerator DoDropBomb()
        {
            bomberAnimator.SetBool("isAttack", isAttacking);
            yield return new WaitForSeconds(0.85f);
            bomberAnimator.SetBool("isAttack", false);
            
            yield return new WaitForSeconds(0.3f);
            if (GetComponent<AI.AbstractAI>().e_currentAiState != AIStates.dead != null)
            {
                Vector3 bombPos = new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z);
                var bomb = Instantiate(bombPrefab, bombPos, Quaternion.identity);
                bomb.GetComponent<Bomb>().ExploseBomb();
            }
            yield return new WaitForSeconds(1.85f);
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
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, e_fliRange); // Zone of the flie range
        }
        
    }
}
