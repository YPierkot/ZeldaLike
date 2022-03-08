using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace AI
{
    public class BomberAI : AbtractAI
    {
        #region Variables
        [Header("Specific values"), Space]
        [SerializeField] private GameObject bombPrefab;
        [SerializeField] private float e_rangeWander = 2;
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
            
            Vector3 newMoveTarget = new Vector3(Random.Range(basePosition.x - e_rangeWander, basePosition.x + e_rangeWander), (1), 
                Random.Range(basePosition.z - e_rangeWander, basePosition.z + e_rangeWander));
            
            e_transform.DOMove(newMoveTarget, 1.8f).OnComplete(() => isMoving = false);
        }
        
        protected override void Attack()
        {
            base.Attack();

            
            
            if (isAttacking)
                return;
            isAttacking = true;
            
            // Attack Pattern
            StartCoroutine(DoDropBomb());
        }

        private IEnumerator DoDropBomb()
        {
            Vector3 bombPos = new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.x);
            var bomb = Instantiate(bombPrefab, bombPos, Quaternion.identity);
            
            yield return new WaitForSeconds(0.4f);
            bomb.GetComponent<Bomb>().ExploseBomb();

            //yield return new WaitForSeconds(0.15f);
            // Add Screen Shake
            
            yield return new WaitForSeconds(1f);
            isAttacking = false;
            
            ChangeState(AIStates.walking);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, e_rangeWander);
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, e_rangeSight);
        }
    }
}
