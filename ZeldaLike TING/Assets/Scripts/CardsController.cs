using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CardsController : MonoBehaviour
{
    // Cards Variables 
    public bool canUseCards;
    
    [Space(10)]
    [Header("Fire Card")]
    public GameObject fireCard;
    public bool isFireGround;
    public GameObject groundFireCard;
    public GameObject fireCardGrounded;
    public bool canUseFireCard;
    public bool canUseLongFireCard;
    
    [Space(10)]
    [Header("Ice Card")] // IceCard
    public GameObject iceCard;
    public bool isIceGround;
    public GameObject groundIceCard;
    public GameObject iceCardGrounded;
    public bool canUseIceCard;
    public bool canUseLongIceCard;
    
    [Space(10)]
    [Header("Wall Card")] // Wall Card
    public GameObject wallCard;
    public GameObject wallCardGrounded;
    public bool isWallGround;
    public GameObject groundWallCard;
    public bool canUseWallCard;
    public bool canUseLongWallCard;
    
    [Space(10)] // Wind Card
    [Header("Wind Card")]
    public bool isWindGround;
    public GameObject windCard;
    public GameObject windCardGrounded;
    public GameObject groundWindCard;
    public bool canUseWindCard;
    public bool canUseLongWindCard;
    
    
    public Transform m_tranform;
    public LayerMask Ennemy;
    public int projectileSpeed;
    public Transform DebugTransform;
    
    public enum CardsState
    {
        Null = 0, Fire, Ice, Wall, Wind
    }
    
    public CardsState State = CardsState.Null;
    
    private void Start()
    {
        canUseCards = true;
        canUseFireCard = canUseIceCard = canUseWallCard = canUseWindCard = canUseLongFireCard = canUseLongIceCard = canUseLongWallCard = canUseLongWindCard = true;
        isFireGround = isIceGround = isWallGround = isWindGround = false;
    }

    // Update is called once per frame
    void Update()
    {
        LaunchCard();
    }

    private void LaunchCard()
    {
        if (!canUseCards) return;
        
        if (Input.GetKeyDown(KeyCode.Mouse0)) // LA C'est POSER UNE CARTE AU SOL
        {
            
        }

        if (Input.GetKeyDown(KeyCode.Mouse1)) // ICI C'EST POUR LE TRUC QUI VA LOIN
        {
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 4f);
    }

    public void ShortRange()
    {
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
        Debug.Log("long Effect");
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
        if (canUseLongFireCard)
        {
            if (!isFireGround)
            {
                Vector3 shootPointPos = (DebugTransform.position - transform.position);
                shootPointPos.Normalize();
                Destroy(fireCardGrounded = Instantiate(fireCard, transform.position + shootPointPos * radiusShootPoint, Quaternion.identity), 5);
                fireCardGrounded.GetComponent<Rigidbody>().velocity = shootPointPos * Time.deltaTime * projectileSpeed;
                isFireGround = true;
            }
            else
            {
                fireCardGrounded.GetComponent<RedCardLongRange>().FireCardLongEffect();
                isFireGround = false;
            }
        }
    }
    
    // Ice Card
    private void IceLongRange()
    {
        if (canUseLongIceCard)
        {
            if (!isIceGround)
            {
                Vector3 shootPointPos = (DebugTransform.position - transform.position);
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
                Vector3 shootPointPos = (DebugTransform.position - transform.position);
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
                Vector3 shootPointPos = (DebugTransform.position - transform.position);
                shootPointPos.Normalize();
                Destroy(windCardGrounded = Instantiate(windCard, transform.position + shootPointPos * radiusShootPoint, Quaternion.identity), 5);
                windCardGrounded.GetComponent<Rigidbody>().velocity = shootPointPos * Time.deltaTime * projectileSpeed;
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
    
    #region EffectCardToGround
    
    // Fire Card 
    private void FireballShortRange()
    {
        if (canUseFireCard)
        {
            if (!isFireGround)
            {
                fireCardGrounded = Instantiate(groundFireCard, new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z), Quaternion.Euler(0,0,90f));
                isFireGround = true;
            }
            else
            {
                fireCardGrounded.GetComponent<RedCardGroundEffect>().ActivateRedGroundEffect();
                isFireGround = false;
            }
        }
    }
    
    // Ice Card
    private void IceShortRange()
    {
        if (canUseIceCard)
        {
            if (!isIceGround)
            {
                iceCardGrounded = Instantiate(groundIceCard, new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z), Quaternion.Euler(0,0,90f));
                isIceGround = true;
            }
            else
            {
                iceCardGrounded.GetComponent<IceCardGroundEffect>().ActivateIceGroundEffect();
                isIceGround = false;
            }
        }   
    }

    // Wall Card
    private void WallShortRange()
    {
        if (canUseWallCard)
        {
            if (!isWallGround)
            {
                wallCardGrounded = Instantiate(groundWallCard, new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z), Quaternion.Euler(0,0,90f));
                isWallGround = true;
            }
            else
            {
                wallCardGrounded.GetComponent<WallGroundCardEffect>().ActivateWallGroundEffect();
                isWallGround = false;
            }
        }
    }
    
    // Wind Card
    private void WindShortRange()
    {
        if (canUseWindCard)
        {
            if (!isWindGround)
            {
                windCardGrounded = Instantiate(groundWindCard, new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z), Quaternion.Euler(0,0,90f));
                isWindGround = true;
            }
            else
            {
                windCardGrounded.GetComponent<WindCardGroundEffect>().ActivateWindGroundEffect();
                isWindGround = false;
            }
        }
    }
    #endregion
    
}