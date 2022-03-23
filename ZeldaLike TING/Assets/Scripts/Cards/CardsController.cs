using System;
using System.Collections;
using Unity.Collections;
using DG.Tweening;
using UnityEngine;

public class CardsController : MonoBehaviour
{
    public static CardsController instance;
    private Controller controller;
    // Cards Variables 
    public bool canUseCards;
    
    [Space(10)]
    [Header("Fire Card")]
    public GameObject fireCard;
    public static bool isFireGround;
    public GameObject fireBall;
    public GameObject fireCardGrounded;
    public bool canUseFireCard;
    
    [Space(10)]
    [Header("Ice Card")] // IceCard
    public GameObject iceCard;
    public static bool isIceGround;
    public GameObject iceCardGrounded;
    public bool canUseIceCard;
    
    [Space(10)]
    [Header("Wall Card")] // Wall Card
    public GameObject wallCard;
    public GameObject wallCardGrounded;
    public static bool isWallGround;
    public GameObject WallSR;
    public bool canUseWallCard;
    
    [Space(10)] // Wind Card
    [Header("Wind Card")]
    public static bool isWindGround;
    public GameObject windCard;
    public GameObject windCardGrounded;
    public GameObject groundWindCard;
    public bool canUseWindCard;
    
    [SerializeField] private LayerMask interactMask;
    [SerializeField] float repulsivePower = 500f;
    [SerializeField] float repulsiveRadius = 4f;
    [SerializeField] Vector3 repulsivePoint;
    
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 4f);
    }

    public void ShortRange()
    {
        if (!canUseCards) return;
        
        Debug.Log("short Effect");
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
        
        //Debug.Log("long Effect");
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

    private const float radiusShootPoint = 0.35f;
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

                StartCoroutine(LaunchCardCD(1));
            }
        }
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
                Destroy(iceCardGrounded = Instantiate(iceCard, transform.position + shootPointPos * radiusShootPoint, Quaternion.identity), 5);
                iceCardGrounded.GetComponent<Rigidbody>().velocity = shootPointPos * Time.deltaTime * projectileSpeed;
                isIceGround = true;
            }
            else
            {
                iceCardGrounded.GetComponent<BlueCardLongRange>().IceCardLongEffet();
                isIceGround = false;
            }
        }
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
                Destroy(wallCardGrounded = Instantiate(wallCard, transform.position + shootPointPos * radiusShootPoint, Quaternion.identity), 5);
                wallCardGrounded.GetComponent<Rigidbody>().velocity = shootPointPos * Time.deltaTime * projectileSpeed;
                isWallGround = true;
            }
            else
            {
                wallCardGrounded.GetComponent<WallCardLongRange>().WallCardLongEffect();
                isWallGround = false;
            }
        }
    }
    
    private void WindLongRange()
    {
        if (canUseWindCard)
        {
            if (!isWindGround)
            {
                Vector3 shootPointPos = (controller.pointerPosition - transform.position);
                shootPointPos.Normalize();
                Destroy(windCardGrounded = Instantiate(windCard, transform.position + shootPointPos * radiusShootPoint, Quaternion.identity), 5);
                windCardGrounded.GetComponent<Rigidbody>().velocity = shootPointPos * Time.deltaTime * projectileSpeed;
                windCardGrounded.GetComponent<WindCardLongRange>().velocity = shootPointPos * Time.deltaTime * projectileSpeed;
                isWindGround = true;
            }
            else
            {
                windCardGrounded.GetComponent<WindCardLongRange>().WindCardLongEffect();
                isWindGround = false;
            }
        }
    }
    
    #endregion
    
    #region EffectCardInstant
    
    // Fire Card 
    private void FireballShortRange()
    {
        if (canUseFireCard)
        {
            fireCardGrounded = Instantiate(fireBall, new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z), Quaternion.Euler(0,0,0f));
        }
    }
    
    // Ice Card
    private void IceShortRange()
    {
        if (canUseIceCard)
        {
            ActivateIceGroundEffect();
        }   
    }

    // Wall Card
    private void WallShortRange()
    {
        if (canUseWallCard)
        {
            ActivateWallGroundEffect();
        }
    }
    
    // Wind Card
    private void WindShortRange()
    {
        if (canUseWindCard)
        {
            ActivateWindGroundEffect();
        }
    }
    #endregion

    
    
    public void ActivateIceGroundEffect()
    {
        float zTransform = transform.position.z;
        float xTransform = transform.position.x;
        float yTransform = transform.position.y;
        
        Debug.Log("Ice Spike Launch");
        Collider[] cols = Physics.OverlapCapsule(new Vector3(xTransform, yTransform, zTransform + 3.7f), new Vector3(xTransform, yTransform, zTransform + 8), 2.5f, Ennemy);
        foreach (var ennemy in cols)
        {
            // Effect
        }
    }
    
    
    public void ActivateWallGroundEffect()
    {
        float zTransform = transform.position.z;
        float xTransform = transform.position.x;
        float yTransform = transform.position.y;

        Debug.Log("Wall Short Range Launched");
        GameObject wall = Instantiate(WallSR, new Vector3(xTransform, yTransform - 1.3f, zTransform), Quaternion.identity);
        wall.transform.DOMove(new Vector3(xTransform, yTransform + .4f, zTransform), 1.8f);
        Destroy(wall, 4f);
    }
    
    public void ActivateWindGroundEffect()
    {
        repulsivePoint = transform.position;
        Collider[] cols = Physics.OverlapSphere(transform.position, 3, interactMask);
        foreach (var col in cols)
        {
            if (col.transform.CompareTag("Interactable"))
            {
                if (col.GetComponent<GemWindPuzzle>() != null)
                {
                    col.GetComponent<GemWindPuzzle>().WindInteract();
                }
            }
            else 
            {
                col.gameObject.GetComponent<Rigidbody>()
                    .AddExplosionForce(repulsivePower, transform.position, repulsiveRadius, 1.7f);
            }
        }
    }
    
    
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