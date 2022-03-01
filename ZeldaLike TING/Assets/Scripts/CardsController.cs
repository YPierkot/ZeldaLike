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
    
    [Space(10)]
    [Header("Ice Card")] // IceCard
    public GameObject iceCard;
    public bool isIceGround;
    public GameObject groundIceCard;
    public GameObject iceCardGrounded;
    public bool canUseIceCard;
    
    [Space(10)]
    [Header("Wall Card")] // Wall Card
    public GameObject wallCard;
    public GameObject wallCardGrounded;
    public bool isWallGround;
    public GameObject groundWallCard;
    public bool canUseWallCard;
    
    [Space(10)] // Wind Card
    [Header("Wind Card")]
    public bool isWindGround;
    public GameObject windCard;
    public GameObject windCardGrounded;
    public GameObject groundWindCard;
    public bool canUseWindCard;
    

    public Transform m_tranform;
    public LayerMask Ennemy;
    public int projectileSpeed;
    
    public enum CardsState
    {
        Null = 0, Fire, Ice, Wall, Wind
    }
    
    public CardsState State = CardsState.Null;
    
    private void Start()
    {
        canUseCards = true;
        canUseFireCard = canUseIceCard = canUseWallCard = canUseWindCard = true;
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

    // Fire Card
    private void FireballLongRange()
    {
        Debug.Log("Fireball Long Range Shoot");
        GameObject cd = Instantiate(fireCard, new Vector3(m_tranform.position.x, m_tranform.position.y, m_tranform.position.z), Quaternion.identity);
        cd.GetComponent<Rigidbody>().velocity = Vector3.forward * Time.deltaTime * projectileSpeed;
    }
    
    // Ice Card
    private void IceLongRange()
    {
        Debug.Log("IceBall Long Range Shoot");
        GameObject cd = Instantiate(iceCard, new Vector3(m_tranform.position.x, m_tranform.position.y, m_tranform.position.z), Quaternion.identity);
        cd.GetComponent<Rigidbody>().velocity = Vector3.forward * Time.deltaTime * projectileSpeed;
    }
    
    // Wall Card
    private void WallLongRange()
    {
        
    }
    
    private void WindLongRange()
    {
        Debug.Log("IceBall Long Range Shoot");
        GameObject wCd = Instantiate(windCard, new Vector3(m_tranform.position.x, m_tranform.position.y, m_tranform.position.z), Quaternion.identity);
        wCd.GetComponent<Rigidbody>().velocity = Vector3.forward * Time.deltaTime * projectileSpeed;
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

