using System;
using System.Collections;
using AI;
using Unity.Collections;
using DG.Tweening;
using UnityEngine;

public class CardsController : MonoBehaviour
{
    public static CardsController instance;
    private Controller controller;
    
    // Cards Variables 
    public bool canUseCards;
    
    [Header("FireCard")]
    public bool canUseFireCard;
    public static bool isFireGround;
    [SerializeField] GameObject fireCardGrounded;
    
    // IceCard
    [Header("Ice Card")] 
    public bool canUseIceCard;
    public static bool isIceGround;
    public GameObject iceCardGrounded;
    
    [Space(10)]
    [Header("Wall Card")] // Wall Card
    public bool canUseWallCard;
    public static bool isWallGround;
    public GameObject wallCardGrounded;
    public GameObject WallSR;
    
    [Space(10)] // Wind Card
    [Header("Wind Card")] [SerializeField]
    public bool canUseWindCard;
    public static bool isWindGround;
    public GameObject windCardGrounded;
    [Space(5)]
    [SerializeField] private LayerMask interactMask;
    [SerializeField] float repulsivePower = 500f;
    [SerializeField] float repulsiveRadius = 4.5f;
    [SerializeField] Vector3 repulsivePoint;
    
    [Space(10)] // Wind Card
    [Header("Utilities")] 
    public Transform m_tranform;
    public LayerMask Ennemy;
    public int projectileSpeed;
    
    public enum CardsState
    {
        Null = 0, Fire, Ice, Wall, Wind
    }
    
    public CardsState State = CardsState.Null;

    private void Awake()
    {
        if (instance != null)
            instance = this;
        else 
            instance = this;
    }

    private void Start()
    {
        controller = GetComponent<Controller>();
        canUseCards = true;
        canUseFireCard = canUseIceCard = canUseWallCard = canUseWindCard = true;
        isFireGround = isIceGround = isWallGround = isWindGround = false;
    }

    public void ShortRange()
    {
        if (!canUseCards) return;
        
        switch(State)
        {
            case CardsState.Null: break;
            case CardsState.Fire: FireballShortRange(); break;
            case CardsState.Ice: IceShortRange(); break;
            case CardsState.Wall: WallShortRange(); break;
            case CardsState.Wind: WindShortRange(); break;
        }
    }

    public void LongRange()
    {
        if (!canUseCards) return;
        
        switch(State)
        {
            case CardsState.Null: break;
            case CardsState.Fire: FireballLongRange(); break;
            case CardsState.Ice:  IceLongRange(); break;
            case CardsState.Wall: WallLongRange(); break;
            case CardsState.Wind: WindLongRange(); break;
        }
    }

    #region CardEffectsLongRange

    private const float radiusShootPoint = 0.55f;
    // Fire Card
    private void FireballLongRange()
    {
        if (canUseFireCard)
        {
            if (!isFireGround)
            {
                canUseFireCard = false;
                
                Vector3 shootPointPos = (controller.pointerPosition - transform.position);
                shootPointPos.Normalize();
                
                fireCardGrounded = PoolManager.Instance.PoolInstantiate(PoolManager.Object.fireCard);
                fireCardGrounded.transform.position = transform.position + shootPointPos * radiusShootPoint;
                fireCardGrounded.GetComponent<Rigidbody>().velocity =
                    shootPointPos * Time.deltaTime * projectileSpeed;
                isFireGround = true;
                
                StartCoroutine(LaunchCardCD(1));
            }
        }
        else if (isFireGround) fireCardGrounded.GetComponent<FireCardLongRange>().FireCardLongEffect();
    }
    
    // Ice Card
    private void IceLongRange()
    {
        if (canUseIceCard)
        {
            if (!isIceGround)
            {
                canUseIceCard = false;

                
                Vector3 shootPointPos = (controller.pointerPosition- transform.position);
                shootPointPos.Normalize();
                
                iceCardGrounded = PoolManager.Instance.PoolInstantiate(PoolManager.Object.iceCard);
                iceCardGrounded.transform.position = transform.position + shootPointPos * radiusShootPoint;
                iceCardGrounded.GetComponent<Rigidbody>().velocity = 
                    shootPointPos * Time.deltaTime * projectileSpeed;
                isIceGround = true;
                
                StartCoroutine(LaunchCardCD(2));
            }
        }
        else if(isIceGround) iceCardGrounded.GetComponent<BlueCardLongRange>().IceCardLongEffet();
    }
    
    // Wall Card
    private void WallLongRange()
    {
        if (canUseWallCard)
        {
            if (!isWallGround)
            {
                canUseWallCard = false;

                
                Vector3 shootPointPos = (controller.pointerPosition - transform.position);
                shootPointPos.Normalize();
                
                wallCardGrounded = PoolManager.Instance.PoolInstantiate(PoolManager.Object.wallCard);
                wallCardGrounded.transform.position = transform.position + shootPointPos * radiusShootPoint;
                wallCardGrounded.GetComponent<Rigidbody>().velocity = 
                    shootPointPos * Time.deltaTime * projectileSpeed;
                isWallGround = true;

                StartCoroutine(LaunchCardCD(3));
            }
        }
        else if (isWallGround) wallCardGrounded.GetComponent<WallCardLongRange>().WallCardLongEffect();
    }
    
    private void WindLongRange()
    {
        if (canUseWindCard)
        {
            if (!isWindGround)
            {
                canUseWindCard = false;
                
                Vector3 shootPointPos = (controller.pointerPosition - transform.position);
                shootPointPos.Normalize();
                
                windCardGrounded = PoolManager.Instance.PoolInstantiate(PoolManager.Object.windCard);
                windCardGrounded.transform.position = transform.position + shootPointPos * radiusShootPoint;
                windCardGrounded.GetComponent<Rigidbody>().velocity = shootPointPos * Time.deltaTime * projectileSpeed;
                isWindGround = true;

                StartCoroutine(LaunchCardCD(4));
            }
        }
        else if(isWindGround) windCardGrounded.GetComponent<WindCardLongRange>().WindCardLongEffect();
    }
    
    #endregion
    
    #region EffectCardInstant
    
    // Fire Card 
    private void FireballShortRange()
    {
        if (canUseFireCard)
        {
            ActivateFireShortEffect();
            StartCoroutine(LaunchCardCD(1));
        }
    }

    // Ice Card
    private void IceShortRange()
    {
        if (canUseIceCard)
        {
            ActivateIceGroundEffect();
            StartCoroutine(LaunchCardCD(2));
        }   
    }

    // Wall Card
    private void WallShortRange()
    {
        if (canUseWallCard)
        {
            ActivateWallGroundEffect();
            StartCoroutine(LaunchCardCD(3));
        }
    }
    
    // Wind Card
    private void WindShortRange()
    {
        if (canUseWindCard)
        {
            ActivateWindGroundEffect();
            //StartCoroutine(LaunchCardCD(4));
        }
    }
    

    // EFFECTS CODE
    private void ActivateFireShortEffect() // OK FX EN COURS D'INTE
    {
        Vector3 shootPointPos = (controller.pointerPosition - transform.position);
        shootPointPos.Normalize();

        GameObject fb = PoolManager.Instance.PoolInstantiate(PoolManager.Object.fireBall);
        fb.transform.position = transform.position + shootPointPos * radiusShootPoint;
        
        fb.GetComponent<Rigidbody>().velocity = shootPointPos * Time.deltaTime * projectileSpeed * 2;
    }
    
    private const float rangeIceShot = 8f;
    private const float rangeStartIceShot = 1f;
    private const float radiusIceShot = 2.5f;
    public void ActivateIceGroundEffect() // OK MANQUE FX
    {
        Vector3 shootPointPos = (controller.pointerPosition - transform.position);
        shootPointPos.Normalize();
        
        var GoDir =  (transform.position + shootPointPos * radiusShootPoint) ;
        
        
        Debug.DrawRay(new Vector3(GoDir.x, transform.position.y, GoDir.z * rangeStartIceShot), new Vector3(shootPointPos.x * rangeIceShot, controller.pointerPosition.y/2 + 2f, shootPointPos.z * rangeIceShot), Color.red, 3f);
        
        Collider[] cols = Physics.OverlapCapsule(new Vector3(GoDir.x, transform.position.y, GoDir.z * rangeStartIceShot), new Vector3(shootPointPos.x * rangeIceShot, controller.pointerPosition.y/2 + 2f, shootPointPos.z * rangeIceShot), radiusIceShot,Ennemy);
        foreach (var ennemy in cols)
        {
            if (ennemy.transform.GetComponent<SwingerAI>())
            {
                ennemy.transform.GetComponent<SwingerAI>().LooseHp(1);
                ennemy.transform.GetComponent<SwingerAI>().FreezeEnnemy();
            }
            else if (ennemy.transform.GetComponent<KamikazeAI>())
            {
                ennemy.transform.GetComponent<KamikazeAI>().LooseHp(1);
                ennemy.transform.GetComponent<KamikazeAI>().FreezeEnnemy();
            }
            else if (ennemy.transform.GetComponent<MageAI>())
            {
                ennemy.transform.GetComponent<MageAI>().LooseHp(1);
                ennemy.transform.GetComponent<MageAI>().FreezeEnnemy();
            }
            else if (ennemy.transform.GetComponent<BomberAI>())
            {
                ennemy.transform.GetComponent<BomberAI>().LooseHp(1);
                ennemy.transform.GetComponent<BomberAI>().FreezeEnnemy();
            }
        }
    }

    public void ActivateWallGroundEffect() // C'est OK FX ARRIVE
    {
        float zTransform = transform.position.z;
        float xTransform = transform.position.x;
        float yTransform = transform.position.y;

        Debug.Log("Wall Short Range Launched");
        GameObject wall = Instantiate(WallSR, new Vector3(xTransform, yTransform - 2.9f, zTransform), Quaternion.identity);
        wall.transform.DOMove(new Vector3(xTransform, yTransform - .25f, zTransform), 1.5f);
        Destroy(wall, 4f);
    }

    private const float forceModifier = 1.6f;
    public void ActivateWindGroundEffect() // OK
    {
        repulsivePoint = transform.position;
        Collider[] cols = Physics.OverlapSphere(transform.position, repulsiveRadius);
        foreach (var col in cols)
        {
            switch (col.transform.tag)
            {
                case "Interactable": if (col.GetComponent<GemWindPuzzle>() != null) col.GetComponent<GemWindPuzzle>().WindInteract();
                    break;

                case "Ennemy":
                    EnnemyWindRepultion(col.gameObject);
                    break;
                default: break;
            }
        }
    }


    private void EnnemyWindRepultion(GameObject enemy)
    {
        enemy.transform.DOKill();
                    
        var shootPointPos = (enemy.transform.position - transform.position);
        var targetPos = new Vector3((enemy.transform.position.x + shootPointPos.x) /* forceModifier*/, 
            enemy.transform.position.y + shootPointPos.y + 1f,
            (enemy.transform.position.z + shootPointPos.z) /* forceModifier*/);
        
        enemy.transform.DOMove(targetPos, 0.3f).OnComplete(() => enemy.transform.DOKill());
        Debug.Log($"{enemy.name} got repulsed !");
    }
    #endregion
    
    public IEnumerator LaunchCardCD(byte cardType) // INT 1 = Fire / 2 = Ice / 3 = Wall / 4 = Wind
    {
        switch (cardType)
        {
            case 1: canUseFireCard = false; break;
            case 2: canUseIceCard = false; break;
            case 3: canUseWallCard = false; break;
            case 4: canUseWindCard = false; break;
            default: break;
        }

        yield return new WaitForSeconds(4f);
        
        switch (cardType)
        {
            case 1: canUseFireCard = true; break;
            case 2: canUseIceCard = true; break;
            case 3: canUseWallCard = true; break;
            case 4: canUseWindCard = true; break;
            default: break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, repulsiveRadius);
    }
}