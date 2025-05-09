using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Assertions.Must;
using Random = UnityEngine.Random;

namespace AI
{
    public class SwingerAI : AbstractAI
    {
        #region Variables

        [Header("Specific Values"), Space]
        [SerializeField] private float e_rangeWander = 4f;
        [SerializeField] private float e_aoeRange = 1f;
        private Vector3 basePosition;
        
        [SerializeField] private bool isMoving;
        [SerializeField] private bool isAttacking;
        private bool debugBool;
        private Vector3 dir;
        private float spriteDir;
        private float lastSpriteDir;
        #endregion
        
        // TEST REGION 
        public Vector3[] directions;
        public GameObject DebugSphere;
        [Range(0.2f, 5)] public float anchorDst = 1f;
        [Range(0.2f, 10)] public float f = 1f;
        
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
            e_animator.SetBool("isWalk", true);
            
            Vector3 newMoveTarget = new Vector3(Random.Range(basePosition.x - e_rangeWander, basePosition.x + e_rangeWander), basePosition.y, 
                Random.Range(basePosition.z - e_rangeWander, basePosition.z + e_rangeWander));
            
            e_transform.DOMove(newMoveTarget, 1.8f).OnComplete(() => isMoving = false);
            
            if (!isMoving)
            {
                e_animator.SetBool("isWalk", false);
            }
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
                isMoving = false;
                isAttacking = true;
                StartCoroutine(DoAttack());
            }
            else
            {
                if (isHitStun) return;

                if (!isAttacking)
                {
                    transform.DOKill();
                    GoToPlayer();
                    e_animator.SetBool("isWalk", true);
                    
                    RaycastHit groundHit;
                    if (Physics.Raycast(transform.position, Vector3.down, out groundHit, 0.2f, groundLayerMask)) transform.position = groundHit.point + new Vector3(0, 0.1f, 0);
                    else transform.position += new Vector3(0, -0.1f, 0);
                    Debug.DrawRay(transform.position, Vector3.down*1, Color.blue);
                }
                else
                {
                    e_animator.SetBool("isWalk", false);
                }
                
                spriteDir = playerTransform.position.x - transform.position.x;

                if (spriteDir < 0) {
                    e_sprite.flipX = true;
                    SpawnFXPos.transform.localPosition = new Vector3(-Mathf.Abs(SpawnFXPos.transform.localPosition.x), SpawnFXPos.transform.localPosition.y, SpawnFXPos.transform.localPosition.z);
                }
                else {
                    e_sprite.flipX = false;
                    SpawnFXPos.transform.localPosition = new Vector3(Mathf.Abs(SpawnFXPos.transform.localPosition.x), SpawnFXPos.transform.localPosition.y, SpawnFXPos.transform.localPosition.z);
                }
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

            if (!isHitStun) e_animator.SetBool("isAttack", true);
            SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.swingerAttack);
            
            yield return new WaitForSeconds(.40f); // Temps de l'animation avant hit & recast dmg point
            
            dir = playerTransform.position - transform.position; dir.Normalize();
            
            yield return new WaitForSeconds(.10f);

            Collider[] playercol = Physics.OverlapSphere(transform.position + dir * radiusShootPoint, e_aoeRange, playerLayerMask);
            foreach (var player in playercol) { if (e_currentAiState != AIStates.dead || !isFreeze || !isHitStun) PlayerStat.instance.TakeDamage(); }

            yield return new WaitForSeconds(1.2f); // Anim fini 
            e_animator.SetBool("isAttack", false);
            CanMove();  
            
            yield return new WaitForSeconds(1.3f); // Temps avant de pouvoir ré attaqué
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
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, e_rangeUnfollow);
        }

        private void CanMove()
        {
            e_rigidbody.constraints = RigidbodyConstraints.None;
            e_rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
            e_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}
