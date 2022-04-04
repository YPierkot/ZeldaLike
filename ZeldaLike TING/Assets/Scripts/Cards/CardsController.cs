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
    [SerializeField] GameObject fireCardGrounded;
    public static bool isFireGround;
    public bool canUseFireCard;
    
    // IceCard
    [Header("Ice Card")] public GameObject iceCardGrounded;
    public static bool isIceGround;
    public bool canUseIceCard;
    
    [Space(10)]
    [Header("Wall Card")] // Wall Card
    public GameObject wallCardGrounded;
    public static bool isWallGround;
    public GameObject WallSR;
    public bool canUseWallCard;
    
    [Space(10)] // Wind Card
    [Header("Wind Card")] [SerializeField]
    public GameObject groundWindCard;
    public static bool isWindGround;
    public GameObject windCardGrounded;
    public bool canUseWindCard;
    
    [SerializeField] private LayerMask interactMask;
    [SerializeField] float repulsivePower = 500f;
    [SerializeField] float repulsiveRadius = 4f;
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
            case CardsState.Ice: IceLongRange(); break;
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
                Vector3 shootPointPos = (controller.pointerPosition - transform.position);
                shootPointPos.Normalize();
                
                fireCardGrounded = PoolManager.Instance.PoolInstantiate(PoolManager.Object.fireCard);
                fireCardGrounded.transform.position = transform.position + shootPointPos * radiusShootPoint;
                fireCardGrounded.GetComponent<Rigidbody>().velocity =
                    shootPointPos * Time.deltaTime * projectileSpeed * 2;
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
                Vector3 shootPointPos = (controller.pointerPosition- transform.position);
                shootPointPos.Normalize();
                
                iceCardGrounded = PoolManager.Instance.PoolInstantiate(PoolManager.Object.iceCard);
                iceCardGrounded.transform.position = transform.position + shootPointPos * radiusShootPoint;
                iceCardGrounded.GetComponent<Rigidbody>().velocity = 
                    shootPointPos * Time.deltaTime * projectileSpeed * 2;
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
                Vector3 shootPointPos = (controller.pointerPosition - transform.position);
                shootPointPos.Normalize();
                
                wallCardGrounded = PoolManager.Instance.PoolInstantiate(PoolManager.Object.wallCard);
                wallCardGrounded.transform.position = transform.position + shootPointPos * radiusShootPoint;
                wallCardGrounded.GetComponent<Rigidbody>().velocity = 
                    shootPointPos * Time.deltaTime * projectileSpeed * 2;
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
                Vector3 shootPointPos = (controller.pointerPosition - transform.position);
                shootPointPos.Normalize();
                
                windCardGrounded = PoolManager.Instance.PoolInstantiate(PoolManager.Object.windCard);
                windCardGrounded.transform.position = transform.position + shootPointPos * radiusShootPoint;
                windCardGrounded.GetComponent<WindCardLongRange>().velocity = shootPointPos * Time.deltaTime * projectileSpeed;
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
            //StartCoroutine(LaunchCardCD(2));
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
            StartCoroutine(LaunchCardCD(4));
        }
    }
    

    // EFFECTS CODE
    private void ActivateFireShortEffect() // A VERIF
    {
        Vector3 shootPointPos = (controller.pointerPosition - transform.position);
        shootPointPos.Normalize();

        GameObject fb = PoolManager.Instance.PoolInstantiate(PoolManager.Object.fireBall);
        fb.transform.position = transform.position + shootPointPos * radiusShootPoint;
        fb.GetComponent<Rigidbody>().velocity = shootPointPos * Time.deltaTime * projectileSpeed;
    }
    
    
    public void ActivateIceGroundEffect() // OK
    {
        Vector3 shootPointPos = (transform.position - controller.pointerPosition);
        shootPointPos.Normalize();
        
        Debug.Log(shootPointPos);
        
        var tempoV31 = new Vector3(0, 0, 1.3f);
        var tempoV32 = new Vector3(0, 0, 6);
        
        var GoDir1 = transform.position + shootPointPos * radiusShootPoint;
        Debug.Log(GoDir1);
        
        Debug.DrawRay(GoDir1 + tempoV31, new Vector3(GoDir1.x, 1, GoDir1.z) + tempoV31, Color.green, 3f);
        
        Collider[] cols = Physics.OverlapCapsule(GoDir1 + tempoV31, GoDir1 + tempoV32, 2.5f, Ennemy);
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
    
    
    public void ActivateWallGroundEffect() // C'est OK
    {
        float zTransform = transform.position.z;
        float xTransform = transform.position.x;
        float yTransform = transform.position.y;

        Debug.Log("Wall Short Range Launched");
        GameObject wall = Instantiate(WallSR, new Vector3(xTransform, yTransform - 2.9f, zTransform), Quaternion.identity);
        wall.transform.DOMove(new Vector3(xTransform, yTransform - .25f, zTransform), 1.5f);
        Destroy(wall, 4f);
    }
    
    public void ActivateWindGroundEffect() // EN COURS DE VERIF
    {
        repulsivePoint = transform.position;
        Collider[] cols = Physics.OverlapSphere(transform.position, 3);
        foreach (var col in cols)
        {
            switch (col.transform.tag)
            {
                case "Interactable": if (col.GetComponent<GemWindPuzzle>() != null) col.GetComponent<GemWindPuzzle>().WindInteract();
                    break;

                case "Ennemy":
                    col.gameObject.GetComponent<Rigidbody>()
                        .AddExplosionForce(repulsivePower, transform.position, repulsiveRadius, 1f);
                    break;
                default: break;
            }
        }
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
}