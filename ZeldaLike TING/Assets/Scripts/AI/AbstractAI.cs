using System.Collections;
using UnityEngine;
using DG.Tweening;
using static AI.AbstractAI.AIStates;

namespace AI
{
    public class AbstractAI : MonoBehaviour
    {
        #region Variables
        
        [Header("Commom Values"), Space]
        [SerializeField] private int e_hp = 1; // Enemy Health Points
        [SerializeField] public float e_rangeSight = 10f; // Enemy Detect Range
        [SerializeField] public float e_rangeAttack = 10f; // Enemy Attack Range
        [SerializeField] private float e_hitStunTime = 3f;
        [SerializeField] protected float e_speed = 10; // Enemy Speed
        [SerializeField] protected Animator e_animator; // Enemy Speed
        [SerializeField] public SpriteRenderer e_sprite; // Enemy Sprite Renderer
        [SerializeField] public LayerMask playerLayerMask; // Player Layer
        [SerializeField] public LayerMask groundLayerMask; // Player Layer
        [SerializeField] private Transform spawnFXPos = null;
        public Transform SpawnFXPos => spawnFXPos;
        
        public enum AIStates
        {
            walking, 
            attacking, 
            dead,
            hit
        }

        [HideInInspector] public AIStates e_currentAiState = walking;
        
        protected Controller player;
        protected Transform playerTransform;
        protected Transform e_transform;
        protected Rigidbody e_rigidbody;

        private Coroutine e_hitStunCO;
        protected bool isFreeze;
        private float freezeTime;
        
        
        public bool isHitStun;
       
       
        
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
            e_animator = GetComponentInChildren<Animator>();
        }
        
        public void Update()
        {
            BehaviourAI();
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
                case attacking : Attack(); break;
                case walking: Walk(); break;
                case dead: Die(); break;
                case hit: Hit(); break;
                default: throw new System.ArgumentOutOfRangeException();
            }
        }

        protected void Hit()
        {
            if(isHitStun) return;
            
            HitStun();
        }

        protected virtual void Walk()
        {
            if (Vector3.Distance(playerTransform.position, e_transform.position) < e_rangeSight)
                ChangeState(attacking); // Skip in attack state if player is in sight range
        }

        protected virtual void Attack() 
        {
            
        }

        private IEnumerator waitDieAnim()
        {
            GetComponentInChildren<Animator>().SetBool("isDead", true);
            yield return new WaitForSeconds(0.75f);
            Destroy(gameObject);
        }
        
        protected void Die()
        {
            e_transform.DOKill();
            StartCoroutine(waitDieAnim());
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
                StartCoroutine(FlashRed());

                if(e_currentAiState == walking)
                    ChangeState(attacking);
            }
        }

        private IEnumerator FlashRed()
        {
            e_sprite.color = Color.red;
            yield return new WaitForSeconds(0.14f);
            e_sprite.color = isFreeze ? new Color(146f/255f, 237f/255f, 255f/255f) : Color.white;
        }

        public void SlowEnemy()
        {
            StartCoroutine(sE());
        }
        
        private IEnumerator sE()
        {
            Debug.Log("OUAIS çA FONCTIONNE LE SANG");
            e_speed /= 2;
            yield return new WaitForSeconds(4.5f);
            e_speed *= 2;
        }
        
        public void FreezeEnemy(float fT)
        {
            StartCoroutine(fE(fT));
        }

        private IEnumerator fE(float fTCo)
        {
            Vector2 speed = new Vector2(e_speed, e_animator.speed);
            e_animator.speed = 0;
            e_sprite.color = new Color(146f / 255f, 237f / 255f, 255f / 255f);
            isFreeze = true;
            e_speed = 0;
            
            yield return new WaitForSeconds(fTCo);
            
            e_animator.speed = speed.y;
            e_sprite.color = new Color(1,1,1);
            e_speed = speed.x;
            isFreeze = false;
        }

        private void HitStun()
        {
            Debug.Log("Oui 2");
            if (e_hitStunCO == null) e_hitStunCO = StartCoroutine(EnemyStunCo());
            else { StopCoroutine(e_hitStunCO); e_hitStunCO = StartCoroutine(EnemyStunCo());}
        }

        private IEnumerator EnemyStunCo()
        {
            Debug.Log("Oui 3");
            isHitStun = true;
            yield return new WaitForSeconds(e_hitStunTime);
            isHitStun = false;
            ChangeState(attacking);
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

