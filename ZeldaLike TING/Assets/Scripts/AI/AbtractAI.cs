using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static AI.AbtractAI.AIStates;

namespace AI
{
    public class AbtractAI : MonoBehaviour
    {
        #region Variables
        
        [Header("Commom Values"), Space]
        [SerializeField] private int e_hp = 1; // Enemy Health Points
        [SerializeField] public float e_rangeSight = 10f; // Enemy Detect Range
        [SerializeField] public float e_rangeAttack = 10f; // Enemy Attack Range
        [SerializeField] protected float e_speed = 10; // Enemy Speed
        [SerializeField] public SpriteRenderer e_sprite; // Enemy Sprite Renderer
        [SerializeField] public LayerMask playerLayerMask; // Player Layer
        [SerializeField] public LayerMask groundLayerMask; // Player Layer
        
        [Header("Effects"), Space]
        [SerializeField] private float e_stuntValue = 0;
        [SerializeField] private float e_FreezeValue = 0;
        
        public enum AIStates
        {
            walking, 
            attacking, 
            dead
        }

        protected AIStates e_currentAiState = walking;
        
        protected Controller player;
        protected Transform playerTransform;
        protected Transform e_transform;
        protected Rigidbody e_rigidbody;
        
        private bool init;
        #endregion

        private void Start()
        {
            Init();
        }

        protected virtual void Init()
        {
            if (init) return;
            init = true;
            
            player = Controller.instance;
            playerTransform = player.transform;
            e_transform = transform;
            e_rigidbody = GetComponent<Rigidbody>();
        }
        
        public void Update()
        {
            BehaviourAI();
            //Debug.Log($"Sight range: {e_rangeSight}, distance with player: {Vector3.Distance(playerTransform.position, e_transform.position) < e_rangeSight}");
        }
        
        
        #region State machine

        public virtual void ChangeState(AIStates aiState)
        {
            if (!init) Init();
            
            if (e_currentAiState == dead) return;
            
            e_currentAiState = aiState;
        }
        
        protected virtual void BehaviourAI()
        {
            switch (e_currentAiState)
            {
                case attacking : Attack();
                    break;
                case walking: Walk();
                    break;
                case dead: Die();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void Walk()
        {
            if (Vector3.Distance(playerTransform.position, e_transform.position) < e_rangeSight)
                ChangeState(attacking); // Skip in attack state if player is in sight range
        }

        protected virtual void Attack()
        {
            
        }

        protected void Die()
        {
            Destroy(gameObject);
            // ADD All other things
        }
        #endregion

        #region Public Method
        public void LooseHp(int count)
        {
            e_hp -= count;
            
            if(e_hp <= 0)
                ChangeState(dead);
            else
            {
                if(e_currentAiState == walking)
                    ChangeState(attacking);
                
                e_sprite.DOColor(Color.red, 0.1f).OnComplete(() => e_sprite.DOColor(Color.white, 0.1f));
                e_sprite.DOFade(0.25f, 0.1f).OnComplete(()=> e_sprite.DOFade(1, 0.1f));
            }
        }


        public void FreezeEnnemy()
        {
            Debug.Log("Coroutine se lance");
            StartCoroutine(fE());
        }

        private IEnumerator fE()
        {
            Debug.Log("OUAIS çA FONCTIONNE LE SANG");
            if (e_speed > 0)
            {
                e_speed -= 2;
                yield return new WaitForSeconds(4f);
                e_speed += 2;
            }
        }
        

        private void OnDestroy()
        {
            e_sprite.DOKill();
            e_transform.DOKill();
        }
        #endregion
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, e_rangeSight);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, e_rangeAttack);
        }
    }
}

