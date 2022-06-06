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
        [SerializeField] public float e_rangeUnfollow = 25f; // Enemy Unlock Player Range
        [SerializeField] private float e_hitStunTime = 3f;
        [SerializeField] protected float e_speed = 10; // Enemy Speed
        [SerializeField] protected Animator e_animator; // Enemy Speed
        [SerializeField] public SpriteRenderer e_sprite; // Enemy Sprite Renderer
        [SerializeField] public LayerMask playerLayerMask; // Player Layer
        [SerializeField] public LayerMask groundLayerMask; // Player Layer
        [SerializeField] private Transform spawnFXPos = null;
        
        [SerializeField] protected GameObject fxBomb;
        [SerializeField] private string hitStunStr;
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
            if (Vector3.Distance(playerTransform.position, e_transform.position) > e_rangeUnfollow)
                ChangeState(walking);
        }

        private IEnumerator waitDieAnim()
        {
            GetComponentInChildren<Animator>().SetBool("isDead", true);
            yield return new WaitForSeconds(0.75f);
            if(GetComponent<Dropper>() != null)GetComponent<Dropper>().Loot();
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
                HitStun();
                transform.GetChild(0).DOScale(new Vector3(1.17f,1.17f,1.17f), 0.2F).OnComplete(() => transform.GetChild(0).DOScale(new Vector3(1f,1f,1f), 0.2F));
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
            if (e_hitStunCO != null) {StopCoroutine(e_hitStunCO); e_hitStunCO = StartCoroutine(EnemyStunCo());}
            else e_hitStunCO = StartCoroutine(EnemyStunCo());
        }

        private IEnumerator EnemyStunCo()
        {
            isHitStun = true;
            e_animator.Play(hitStunStr);
            yield return new WaitForSeconds(0.33f);
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
        
        protected void GoToPlayer()
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

                var debugX = (playerTransform.position.x - transform.position.x);
                var debugZ = (playerTransform.position.z - transform.position.z);

                if (debugZ > 0) // Joueur au dessus
                {
                    if (debugX > 0)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3( pointX + 0.3f, transform.position.y,pointZ + 3f),e_speed * Time.deltaTime);
                        Debug.DrawLine(transform.position, new Vector3( pointX + 0.3f, transform.position.y,pointZ + 3f), Color.yellow, .4f);
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(pointX - 3f, transform.position.y,pointZ + 0.3f), e_speed * Time.deltaTime);
                        Debug.DrawLine(transform.position, new Vector3( pointX + 0.3f, transform.position.y,pointZ + 3f), Color.yellow, .4f);
                    }
                }
                else // Joueur en dessous
                {
                    if (debugX > 0)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(pointX + 3f, transform.position.y,pointZ + 0.4f), e_speed * Time.deltaTime);
                        Debug.DrawLine(transform.position, new Vector3(pointX + 3f, transform.position.y,pointZ + 0.3f), Color.yellow, .4f);
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(pointX - 3f, transform.position.y,pointZ + .4f), e_speed * Time.deltaTime);
                        Debug.DrawLine(transform.position, new Vector3(pointX - 2f, transform.position.y,pointZ - 3f), Color.yellow, .4f);
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
    }
}

