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
        
        #endregion

        protected override void Init()
        {
            base.Init();
            basePosition = transform.position;
            isAttacking = false;
        }

        public override void ChangeState(AIStates aiStates)
        {
            base.ChangeState(aiStates);
            
        }

        protected override void Walk()
        {
            if (isAttacking)
                return;
            
            base.Walk();

            if (isMoving)
                return;

            isMoving = true;
            
            Vector3 newMoveTarget = new Vector3(Random.Range(basePosition.x - e_rangeWander, basePosition.x + e_rangeWander), basePosition.y, 
                Random.Range(basePosition.z - e_rangeWander, basePosition.z + e_rangeWander));
            
            e_transform.DOMove(newMoveTarget, 1.8f).OnComplete(() => isMoving = false);
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
                StartCoroutine(DoAttack());
            }
            else
            {
                transform.DOKill();
                transform.position = Vector3.MoveTowards(transform.position, playerTransform.position,
                    e_speed * Time.deltaTime);
            }
        }

        private IEnumerator DoAttack()
        {
            yield return new WaitForSeconds(0.5f);
            Collider[] playercol = Physics.OverlapSphere(transform.position, e_aoeRange, playerLayerMask);
            foreach (var player in playercol)
            {
                player.GetComponent<PlayerStat>().TakeDamage();
            }
            
            Destroy(gameObject, Single.MinValue);
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
