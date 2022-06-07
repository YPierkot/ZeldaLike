using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CardsController : MonoBehaviour
{
    public static CardsController instance;
    private Controller controller;

    #region Cards Variables
    // Cards Variables 
    public bool canUseCards;
    [HideInInspector] public bool rectoSide;
    
    [Header("FireCard")]
    public bool canUseFireCard;
    public bool fireCardUnlock = true; 
    public static bool isFireGround;
    [SerializeField] GameObject fireCardGrounded;
    public GameObject firecircleFx;
     public bool fireRectoUse;
    
    // IceCard
    [Header("Ice Card")] 
    public bool canUseIceCard;
     public bool iceCardUnlock = true;
    public static bool isIceGround;
    public GameObject iceCardGrounded;
    public GameObject enemyFreezeFX;
     public bool iceRectoUse;
    
    [Space(10)]
    [Header("Wall Card")] // Wall Card
    public bool canUseWallCard;
    public bool wallCardUnlock = true;
    public static bool isWallGround;
    public GameObject wallCardGrounded;
    public GameObject WallSR;
     public bool wallRectoUse;
    
    [Space(10)] // Wind Card
    [Header("Wind Card")] [SerializeField]
    public bool canUseWindCard;
    public bool windCardUnlock = true;
    public static bool isWindGround;
    public GameObject windCardGrounded;
    [Space(5)]
    [SerializeField] private LayerMask interactMask;
    [SerializeField] float repulsiveRadius = 4.5f;
    [SerializeField] Vector3 repulsivePoint;
    public GameObject DebugWindSphere;
     public bool windRectoUse;
    
    [Space(10)] // Wind Card
    [Header("Utilities")] 
    public Transform m_tranform;
    public LayerMask Ennemy;
    public int projectileSpeed;

    [Space(10)] // Wind Card FX's
    [Header("FX'S")]
    [SerializeField] private GameObject ShortFireFx;
    [SerializeField] private GameObject ShortIceFx;
    [SerializeField] private GameObject ShortWallFx;
    [SerializeField] private GameObject ShortWindFx;
    
    public enum CardsState
    {
        Null = 0, Fire, Ice, Wall, Wind
    }

    public LayerMask MaskForIce;
    public float cardCooldown;
    #endregion
    
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
        
#if !UNITY_EDITOR        
        canUseCards = false;
        canUseFireCard = canUseIceCard = canUseWallCard = canUseWindCard = false;
        isFireGround = isIceGround = isWallGround = isWindGround = false;
#endif
        //fireCardUnlock = iceCardUnlock = wallCardUnlock = windCardUnlock = false;
        UIManager.Instance.UpdateCardUI();
    }

    public void ShortRange()
    {
        if (!canUseCards) return;
        
        switch(State)
        {
            case CardsState.Null: break;
            case CardsState.Fire: FireballShortRange(); break;
            case CardsState.Ice:  IceShortRange(); break;
            case CardsState.Wall:  WallShortRange(); break;
            case CardsState.Wind:  WindShortRange(); break;
        }
        UIManager.Instance.UpdateCardUI();
    }
    
    public void LongRange()
    {
        if (!canUseCards) return;
        
        switch(State)
        {
            case CardsState.Null: break;
            case CardsState.Fire:  FireballLongRange(); break;
            case CardsState.Ice:  IceLongRange(); break;
            case CardsState.Wall:  WallLongRange(); break;
            case CardsState.Wind: WindLongRange(); break;
        }
        UIManager.Instance.UpdateCardUI();
    }
    
    public void LongRangeRecast()
    {
        if (!canUseCards) return;
        Debug.Log("recast Card " + canUseFireCard);
        switch(State)
        {
            case CardsState.Null: break;
            case CardsState.Fire: if(!canUseFireCard) FireballLongRange(); break;
            case CardsState.Ice:  if(!canUseIceCard)  IceLongRange(); break;
            case CardsState.Wall: if(!canUseWallCard) WallLongRange(); break;
            case CardsState.Wind: if(!canUseWindCard) WindLongRange(); break;
        }
        
        UIManager.Instance.UpdateCardUI();
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
                //UIManager.Instance.LaunchFireCardTween();
                UIManager.Instance.cardHandlesReference[0].animator.SetTrigger("ActivatedCard");
                canUseFireCard = false;
                Vector3 shootPointPos;
                if(GameManager.Instance.currentContorller == GameManager.controller.Keybord) shootPointPos = (controller.pointerPosition - transform.position).normalized;
                else if (controller.secondStick) shootPointPos =-controller.moveCardTransform.forward ;
                else shootPointPos =-controller.movePlayerTransform.forward ;
                
                fireCardGrounded = PoolManager.Instance.PoolInstantiate(PoolManager.Object.fireCard);
                fireCardGrounded.transform.position = transform.position + new Vector3(shootPointPos.x, 0, shootPointPos.y) * radiusShootPoint;
                fireCardGrounded.transform.rotation = Quaternion.Euler(0, Controller.instance.angleView - 90f + 180, 0);
                fireCardGrounded.GetComponent<Rigidbody>().velocity =
                    shootPointPos * projectileSpeed;
                isFireGround = true;
            }
        }
        else if (isFireGround) fireCardGrounded.GetComponent<FireCardLongRange>().FireCardLongEffect(); UIManager.Instance.UpdateCardUI(); SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.fireExplosion);
    }
    
    // Ice Card
    private void IceLongRange()
    {
        if (canUseIceCard)
        {
            if (!isIceGround)
            {
                //UIManager.Instance.LaunchFireIceTween();
                UIManager.Instance.cardHandlesReference[1].animator.SetTrigger("ActivatedCard");
                canUseIceCard = false;
                Vector3 shootPointPos;
                if(GameManager.Instance.currentContorller == GameManager.controller.Keybord) shootPointPos = (controller.pointerPosition - transform.position).normalized;
                else if (controller.secondStick) shootPointPos =-controller.moveCardTransform.forward ;
                else shootPointPos =-controller.movePlayerTransform.forward ;
                
                iceCardGrounded = PoolManager.Instance.PoolInstantiate(PoolManager.Object.iceCard);
                iceCardGrounded.transform.position = transform.position + new Vector3(shootPointPos.x, 0, shootPointPos.y) * radiusShootPoint;
                iceCardGrounded.transform.rotation = Quaternion.Euler(0, Controller.instance.angleView - 90f + 180, 0);
                iceCardGrounded.GetComponent<Rigidbody>().velocity = 
                    shootPointPos  * projectileSpeed;
                isIceGround = true;
                
                UIManager.Instance.UpdateCardUI();
            }
        }
        else if(isIceGround) iceCardGrounded.GetComponent<BlueCardLongRange>().IceCardLongEffet(); UIManager.Instance.UpdateCardUI(); SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.iceVerso);

    }
    
    // Wall Card
    private void WallLongRange()
    {
        if (canUseWallCard)
        {
            if (!isWallGround)
            {
                //UIManager.Instance.LaunchFireWallTween();
                UIManager.Instance.cardHandlesReference[2].animator.SetTrigger("ActivatedCard");
                canUseWallCard = false;
                Vector3 shootPointPos;
                if(GameManager.Instance.currentContorller == GameManager.controller.Keybord) shootPointPos = (controller.pointerPosition - transform.position).normalized;
                else if (controller.secondStick) shootPointPos =-controller.moveCardTransform.forward ;
                else shootPointPos =-controller.movePlayerTransform.forward ;
                    
                wallCardGrounded = PoolManager.Instance.PoolInstantiate(PoolManager.Object.wallCard);
                wallCardGrounded.transform.position = transform.position + new Vector3(shootPointPos.x, 0, shootPointPos.y) * radiusShootPoint;
                wallCardGrounded.transform.rotation = Quaternion.Euler(0, Controller.instance.angleView - 90f + 180, 0);
                wallCardGrounded.GetComponent<Rigidbody>().velocity = 
                    shootPointPos * projectileSpeed;
                isWallGround = true;
                
                wallCardGrounded.GetComponent<WallCardLongRange>().wallRotation = Quaternion.Euler(0, Controller.instance.angleView - 90f + 180, 0);
                
                UIManager.Instance.UpdateCardUI();
            }
        }
        else if (isWallGround) wallCardGrounded.GetComponent<WallCardLongRange>().WallCardLongEffect(); SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.groundWall);

    }
    
    private void WindLongRange()
    {
        if (canUseWindCard)
        {
            if (!isWindGround)
            {
                //UIManager.Instance.LaunchFireWindTween();
                UIManager.Instance.cardHandlesReference[3].animator.SetTrigger("ActivatedCard");
                canUseWindCard = false;
                Vector3 shootPointPos;
                if(GameManager.Instance.currentContorller == GameManager.controller.Keybord) shootPointPos = (controller.pointerPosition - transform.position).normalized;
                else if (controller.secondStick) shootPointPos =-controller.moveCardTransform.forward ;
                else shootPointPos =-controller.movePlayerTransform.forward ;
                
                windCardGrounded = PoolManager.Instance.PoolInstantiate(PoolManager.Object.windCard);
                windCardGrounded.transform.position = transform.position + new Vector3(shootPointPos.x, 0, shootPointPos.y) * radiusShootPoint;
                windCardGrounded.transform.rotation = Quaternion.Euler(0, Controller.instance.angleView - 90f + 180, 0);
                windCardGrounded.GetComponent<Rigidbody>().velocity = shootPointPos * projectileSpeed;
                isWindGround = true;
                
                UIManager.Instance.UpdateCardUI();
            }
        }
        else if(isWindGround) windCardGrounded.GetComponent<WindCardLongRange>().WindCardLongEffect(); SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.windAttract);
    }
    
    #endregion
    
    #region EffectCardInstant
    
    // Fire Card 
    private void FireballShortRange()
    {
        if (canUseFireCard)
        {
            ActivateFireShortEffect();
            //UIManager.Instance.LaunchFireCardTween();
            StartCoroutine(LaunchCardCDCo(1));
        }
    }

    // Ice Card
    private void IceShortRange()
    {
        if (canUseIceCard)
        {
            ActivateIceGroundEffect();
            //UIManager.Instance.LaunchFireIceTween();
            StartCoroutine(LaunchCardCDCo(2));
        }   
    }

    // Wall Card
    private void WallShortRange()
    {
        if (canUseWallCard)
        {
            ActivateWallGroundEffect();
            //UIManager.Instance.LaunchFireWallTween();
            StartCoroutine(LaunchCardCDCo(3));
        }
    }
    
    // Wind Card
    private void WindShortRange()
    {
        if (canUseWindCard)
        {
            ActivateWindGroundEffect();
            //UIManager.Instance.LaunchFireWindTween();
            StartCoroutine(LaunchCardCDCo(4));
        }
    }
    

    // EFFECTS CODE
    private void ActivateFireShortEffect() // OK
    {
        UIManager.Instance.cardHandlesReference[0].animator.SetTrigger("ActivatedCard");
        Vector3 shootPointPos;
        if(GameManager.Instance.currentContorller == GameManager.controller.Keybord) shootPointPos = (controller.pointerPosition - transform.position).normalized;
        else if (controller.secondStick) shootPointPos =-controller.moveCardTransform.forward ;
        else shootPointPos =-controller.movePlayerTransform.forward ;

        Destroy(Instantiate(ShortFireFx, transform.position + shootPointPos * (radiusShootPoint * 2.1f), Quaternion.Euler(0,Controller.instance.angleView -90f + 180,0)), 0.8f);
        SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.fireBall);
        GameObject fb = PoolManager.Instance.PoolInstantiate(PoolManager.Object.fireBall);
        fb.transform.position = transform.position + shootPointPos * radiusShootPoint;
        fb.GetComponent<Rigidbody>().velocity = shootPointPos * projectileSpeed * 2;
        
        Destroy(fb, 3f);
    }
    
    private const float rangeIceShot = 8f;
    private const float rangeStartIceShot = 1f;
    private const float radiusIceShot = 2.5f;
    public void ActivateIceGroundEffect()
    {
        UIManager.Instance.cardHandlesReference[1].animator.SetTrigger("ActivatedCard");
        Vector3 shootPointPos = (controller.pointerPosition - transform.position);
        shootPointPos.Normalize();

        var GoDir =  (transform.position + shootPointPos * radiusShootPoint) ;
        Debug.DrawRay(new Vector3(GoDir.x, transform.position.y, GoDir.z * rangeStartIceShot), 
            new Vector3(shootPointPos.x * rangeIceShot, controller.pointerPosition.y/2 + 2f, shootPointPos.z * rangeIceShot), Color.red, 3f);

        Destroy(Instantiate(ShortIceFx, new Vector3(GoDir.x, transform.position.y - 1.1f, GoDir.z * rangeStartIceShot),
            Quaternion.Euler(-90, Controller.instance.angleView - 90f + 180, 0)), 3f);
        SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.iceRecto); 
        
        Collider[] cols = Physics.OverlapCapsule(new Vector3(GoDir.x, transform.position.y, GoDir.z * rangeStartIceShot), 
            new Vector3(shootPointPos.x * rangeIceShot, controller.pointerPosition.y/2 + 2f, shootPointPos.z * rangeIceShot), radiusIceShot,MaskForIce);
        foreach (var ennemy in cols)
        {
            if (ennemy.CompareTag("Ennemy")) 
            {
                ennemy.transform.GetComponent<AI.AbstractAI>().LooseHp(1);
                ennemy.transform.GetComponent<AI.AbstractAI>().FreezeEnemy(3.5f);
                Instantiate(enemyFreezeFX, ennemy.transform.GetComponent<AI.AbstractAI>().SpawnFXPos.position, Quaternion.identity, ennemy.transform.GetComponent<AI.AbstractAI>().SpawnFXPos);
            }
            
            //if (ennemy.transform.CompareTag("Interactable")) ennemy.GetComponent<InteracteObject>().isFreeze = true;
            if (ennemy.transform.CompareTag("Interactable")) ennemy.GetComponent<InteracteObject>().Freeze(ennemy.transform.position);
        }
    }

    public void ActivateWallGroundEffect() // C'est OK 
    {
        UIManager.Instance.cardHandlesReference[2].animator.SetTrigger("ActivatedCard");
        float zTransform = transform.position.z;
        float xTransform = transform.position.x;
        float yTransform = transform.position.y;

        Debug.Log("Wall Short Range Launched");
        Destroy(Instantiate(ShortWallFx, transform.position + new Vector3(0, -1,0), Quaternion.identity), 3f);
        SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.groundPeak); 
        GameObject wall = Instantiate(WallSR, new Vector3(xTransform, yTransform - 2.9f, zTransform), Quaternion.identity);
        wall.transform.DOMove(new Vector3(xTransform, yTransform + .3f, zTransform), 1.5f);
        wall.GetComponent<WallDeseapear>().WallDeseapearFct();
    }
    
    private const float forceModifier = 1.6f;
    public void ActivateWindGroundEffect() // OK
    {
        UIManager.Instance.cardHandlesReference[3].animator.SetTrigger("ActivatedCard");
        Destroy(Instantiate(ShortWindFx, transform.position, Quaternion.identity), 3f);
        SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.WindThrow);
        repulsivePoint = transform.position;
        //if(DebugWindSphere != null)Destroy(Instantiate(DebugWindSphere, repulsivePoint, Quaternion.identity), 2f);
        Collider[] cols = Physics.OverlapSphere(transform.position, repulsiveRadius);
        foreach (var col in cols)
        {
            switch (col.transform.tag)
            {
                case "Interactable": 
                    if (col.GetComponent<GemWindPuzzle>() != null) col.GetComponent<GemWindPuzzle>().WindInteract();
                    if (col.GetComponent<InteracteObject>() != null) col.GetComponent<InteracteObject>().OnWindEffect(this);
                    break;

                case "Ennemy": EnnemyWindRepultion(col.gameObject); break;
                default: break;
            }
        }
    }
    
    private void EnnemyWindRepultion(GameObject enemy)
    {
        enemy.transform.DOKill();
                    
        var shootPointPos = (enemy.transform.position - transform.position);
        var targetPos = new Vector3((enemy.transform.position.x + shootPointPos.x), 
            enemy.transform.position.y,
            (enemy.transform.position.z + shootPointPos.z));
        
        enemy.transform.DOMove(targetPos, 0.3f).OnComplete(() => enemy.transform.DOKill());
        Debug.Log($"{enemy.name} got repulsed !");
    }
    #endregion
    
    public IEnumerator LaunchCardCDCo(byte cardType) // INT 1 = Fire / 2 = Ice / 3 = Wall / 4 = Wind
    {
        switch (cardType)
        {
            case 1: canUseFireCard = false; break;
            case 2: canUseIceCard = false; break;
            case 3: canUseWallCard = false; break;
            case 4: canUseWindCard = false; break;
            default: break;
        }

        yield return new WaitForSeconds(cardCooldown);
        switch (cardType)
        {
            case 1: canUseFireCard = true; break;
            case 2: canUseIceCard = true; break;
            case 3: canUseWallCard = true; break;
            case 4: canUseWindCard = true; break;
            default: break;
        }
        UIManager.Instance.UpdateCardUI();
    }
    public void LaunchCardCD(byte cardType) // INT 1 = Fire / 2 = Ice / 3 = Wall / 4 = Wind
    {
        switch (cardType)
        {
            case 1: StartCoroutine(LaunchCardCDCo(1)); break;
            case 2: StartCoroutine(LaunchCardCDCo(2)); break;
            case 3: StartCoroutine(LaunchCardCDCo(3)); break;
            case 4: StartCoroutine(LaunchCardCDCo(4)); break;
        }
    }
}