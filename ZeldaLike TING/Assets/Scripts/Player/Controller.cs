using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    #region CLASS & Other

    public enum ControlType
    {
        MoveStopShoot, HoldForLong, ChangeSideControl
    }
    
    [Serializable]
    class SpriteAngle
    {
        public Interval angleInterval;
        
        public Sprite sprite;
    }
    
    [Serializable]
    struct Interval
    {
        public float min;
        public float max;
    }

    #endregion
    
    // --- COMPONENTS ---
    private SpriteRenderer sprite;
    [HideInInspector] public Rigidbody rb;
    private PlayerInput _playerInput;
    public ControlType _controlType;
    private ControlType lastControlType;
    [SerializeField] private Animator animatorPlayer;
    [SerializeField] private Animator animatorMovePlayer;
    
    private CardsController cardControl;
    public static Controller instance;
    
    [Header("--- PLAYER STATES ---")] [Space(10)] 
   // --- STATES ---
   [SerializeField] private bool moving;
   [SerializeField] private bool dashing;
   [SerializeField] public bool inAttack;
   [SerializeField] private bool inAttackAnim;
   [SerializeField] private bool holdingForCard;

   private bool moveHoldCard;

    private float dashTimer;
    private float holdTimer; 
    private int dashAvailable; 
    private float dashCDtimer; 

    [SerializeField] public bool canMove = true;
    
    [SerializeField] private SpriteAngle[] spriteArray;
    private System.Collections.Generic.Dictionary<Func<float, bool>, SpriteAngle> spriteDictionary = new System.Collections.Generic.Dictionary<Func<float, bool>, SpriteAngle>();


    private PlayerInputMap InputMap;
    [SerializeField] private LayerMask pointerMask;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDistance;
    public Transform moveCardTransform;
    public Transform movePlayerTransform;
    private Vector3 lastDir;
    
    [Header("--- CAMERA ---")] 
    [SerializeField] private CameraController camera;
    public Transform PlayerCameraPoint;
    private Vector3 cameraOffset;
    private bool cameraOnPlayer = true;
    private bool dashCamera;

    [NonSerialized] public Vector3 pointerPosition;
    private float angleView;
    private Interval currentInterval = new Interval{ min=61, max=120 };
    
    [Header("--- ATTAQUE ---")] 
    public int attackCounter;
    [SerializeField] public bool setNextCombo;
    [SerializeField] private GameObject[] keybordAttackZones;
    [SerializeField] private GameObject[] controllerAttackZones;

    private bool comboWaiting;

    [Header("--- PARAMETRES ---")] 
    [SerializeField] private float moveSpeed;
    [SerializeField] private AnimationCurve dashCurve;
    [SerializeField] int maxDash; 
    [SerializeField] private float dashCD = 2f;
    
    [Header("--- DEBUG ---")] 
    [SerializeField] private TMPro.TextMeshProUGUI Debugger;
    [SerializeField] private Transform transformDebugger;
    
    
    void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
        
        SetInputMap();
        
    }
    #region Input Methode

    void SetInputMap()
    {
        if (InputMap != null) InputMap.Disable();
        InputMap = new PlayerInputMap();
        InputMap.Enable();
        InputMap.Movement.Rotation.performed += RotationOnperformed;
        InputMap.Movement.Dash.performed += context => Dash();
        InputMap.Movement.Position.started += context => moving = true;
        InputMap.Movement.Position.canceled += context => moving = false;

        InputMap.Action.shortCard.performed += context => cardControl.ShortRange();
        InputMap.Action.longCard.performed += context => cardControl.LongRange();
        InputMap.Action.Attack.performed += context => Attack();
        if (_controlType == ControlType.HoldForLong)
        {
            InputMap.HoldForLong.CardHolder.started += context => holdingForCard = true; 
            InputMap.HoldForLong.CardHolder.canceled += CardHolderOncanceled; 
            InputMap.HoldForLong.cardActivator.performed += context => cardControl.LongRangeRecast(); 
        }
        else if (_controlType == ControlType.MoveStopShoot)
        {
            InputMap.MoveStopShoot.holdForShoot.performed += context =>
            {
                moveHoldCard = true;
                canMove = false;
            };
            InputMap.MoveStopShoot.holdForShoot.canceled += context =>
            {
                moveHoldCard = false;
                canMove = true;
            };
            InputMap.MoveStopShoot.shootHold.canceled += CardHolderOncanceled;
            InputMap.MoveStopShoot.cardActivatorHold.performed += context => cardControl.LongRangeRecast(); 
            
        }
        else if (_controlType == ControlType.ChangeSideControl)
        {
            InputMap.ChangeSideControl.ChangeCard.performed += context =>
            {
                cardControl.rectoSide = !cardControl.rectoSide;
               if(!cardControl.rectoSide) cardControl.fireRectoUse = cardControl.iceRectoUse = cardControl.wallRectoUse = cardControl.windRectoUse = true;
               else cardControl.fireRectoUse = cardControl.iceRectoUse = cardControl.wallRectoUse = cardControl.windRectoUse = false;
               UIManager.Instance.UpdateCardUI();
            };

            InputMap.ChangeSideControl.Shoot.performed += context =>
            {
                if (cardControl.rectoSide) cardControl.ShortRange();
                else cardControl.LongRange();
            };
        }
        
        InputMap.Menu.CardMenu.performed += SwitchCard;
    }
    
    private void CardHolderOncanceled(InputAction.CallbackContext obj) 
    {
        if (_controlType == ControlType.HoldForLong && holdingForCard)
        {
            Debug.Log("cast Card :" + holdTimer);
            holdingForCard = false; 
            moveCardTransform.gameObject.SetActive(false);
            if (holdTimer < 0.5f && _controlType == ControlType.HoldForLong) cardControl.ShortRange();
            else cardControl.LongRange(); 
            
            holdTimer = 0;
        }
        else if (_controlType == ControlType.MoveStopShoot)
        {
            if(moveHoldCard) cardControl.LongRange(); 
            else cardControl.ShortRange();
        }
    } 
    
    private void SwitchCard(InputAction.CallbackContext obj) 
    { 
        Debug.Log("change card");
        if(obj.ReadValue<float>() == -1) UIManager.Instance.ChangeCard(-1); 
        else UIManager.Instance.ChangeCard(1); 
    }

    private void RotationOnperformed(InputAction.CallbackContext obj)
    {
        Vector2 rotation = obj.ReadValue<Vector2>().normalized;
        Rotate(rotation);
    }
    #endregion
    
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
        cardControl = GetComponent<CardsController>();
        
        foreach (SpriteAngle SA in spriteArray)
        {
            spriteDictionary.Add(x => x < SA.angleInterval.max, SA);
        }

        dashAvailable = maxDash;
    }

    private void Update()
    {
        if (GameManager.Instance.currentContorller == GameManager.controller.Keybord)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, Mathf.Infinity, pointerMask);
            //transformDebugger.position = hit.point;
            pointerPosition = hit.point;
            Vector2 vector = (new Vector2(hit.point.x, hit.point.z) - new Vector2(transform.position.x, transform.position.z)).normalized;
            Rotate(vector);
        }

        if (lastControlType != _controlType)
        {
            SetInputMap();
            lastControlType  = _controlType;
        }
        
        if (holdingForCard && (_controlType == ControlType.HoldForLong || (_controlType == ControlType.MoveStopShoot && moveHoldCard))) 
        { 
            holdTimer += Time.deltaTime; 
            if(holdTimer > 0.5f) moveCardTransform.gameObject.SetActive(true); 
        } 
        
        if (dashing) 
        {
            if (dashTimer > dashCurve[dashCurve.length-1].time)
            {
                dashing = false;
                canMove = true;
                camera.dashing = false;
                dashCamera = false;
            }

            if(dashCamera) camera.transform.position = transform.position + cameraOffset;
            rb.velocity = (lastDir*dashCurve.Evaluate(dashTimer)*moveSpeed); 
            dashTimer += Time.deltaTime;
        }

        if (dashAvailable < maxDash)
        {
            dashCDtimer += Time.deltaTime;
            if (dashCDtimer >= dashCD)
            {
                dashCDtimer = 0;
                dashAvailable++;
            }
        }

        //if (Debugger != null) Debugger.text = $"Dash Available : {dashAvailable}, CD : {dashCDtimer}";
        
        if(Input.GetAxis("Mouse ScrollWheel")> 0f) UIManager.Instance.ChangeCard(1);
        if(Input.GetAxis("Mouse ScrollWheel")< 0f) UIManager.Instance.ChangeCard(-1);
        
    }
    
    private void FixedUpdate()
    {
        Vector3 dir = new Vector3(InputMap.Movement.Position.ReadValue<Vector2>().x, 0, InputMap.Movement.Position.ReadValue<Vector2>().y).normalized;
        lastDir = dir;
        
        if (canMove)
        {
            if (moving)
            {
                Move();

                if (GameManager.Instance.currentContorller.ToString() != _playerInput.currentControlScheme)
                {
                    switch (_playerInput.currentControlScheme)
                    {
                        case "Xbox" :
                            GameManager.Instance.currentContorller = GameManager.controller.Xbox;
                            break;
                        
                        case "Keybord" : 
                            GameManager.Instance.currentContorller = GameManager.controller.Keybord;
                            break;
                        
                        default: Debug.Log($"Add {_playerInput.currentControlScheme} to controller enum in GameManager and Here "); 
                            break;
                    }
                } // Change current Controller in GameManager
            }
            
        }
        else if (moveHoldCard)
        {
            float anglePlayerView = -(Mathf.Atan2(dir.z, dir.x)*Mathf.Rad2Deg); 
            if (angleView < 0) anglePlayerView += 360 ; 
            movePlayerTransform.rotation = Quaternion.Euler(0, anglePlayerView-90, 0);
        }
        
        Animations();

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit groundHit, groundDistance, groundMask)) transform.position = groundHit.point + new Vector3(0, groundDistance - 0.05f, 0);
        else transform.position += new Vector3(0, -0.1f, 0);
        
    }
    
    private void OnDrawGizmos()
    {
       if(!CustomLDData.showGizmos || !CustomLDData.showGizmosGameplay) return;
       Debug.DrawRay(transform.position, Vector3.down*groundDistance, Color.blue);
    }

    void Move()
    {
        Vector3 dir = lastDir;
        float anglePlayerView = -(Mathf.Atan2(dir.z, dir.x)*Mathf.Rad2Deg); 
        if (angleView < 0) anglePlayerView += 360 ; 
        movePlayerTransform.rotation = Quaternion.Euler(0, anglePlayerView-90, 0); 
        
        rb.AddForce(dir * moveSpeed);
    }

    public void ForceMove(Vector3 target)
    {
        if (Vector3.Distance(transform.position, target) >= 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, 5 * Time.deltaTime);
        }

    }
    void Dash()
    {
        if (!dashing && (canMove || animatorPlayer.GetCurrentAnimatorClipInfo(0)[0].clip.name == "waitAttackState") && dashAvailable > 0)
        {
            animatorPlayer.SetBool("attackFinish", true);
            dashAvailable--;
            dashing = true;
            dashTimer = 0;
            canMove = false;
            
            inAttack = false;
            attackCounter = 0;
            DesactiveAttackZone();
            if (cameraOnPlayer)
            {
                dashCamera = true;
                cameraOffset = camera.transform.position - transform.position;
            }
            camera.dashing = true;
        }
    }
    void Attack()
    {
        if (attackCounter < 3)
        {
            if (!inAttack)
            {
                animatorPlayer.SetBool("attackFinish", false);
                setNextCombo = true;
                StopCoroutine(ComboWait());
                comboWaiting = false;
                Debug.Log("STOP Wait");
                canMove = false;
                inAttack = true;
                //nextCombo = false;
                attackCounter++;
            }
            else if(!setNextCombo && inAttackAnim)
            {
                setNextCombo = true;
                attackCounter++;
                Debug.Log("Set NextCombo " + attackCounter);
            }
        }
    }
    void Rotate(Vector2 rotation)
    {
        if (!inAttack)
        {
            angleView = -(Mathf.Atan2(rotation.y, rotation.x)*Mathf.Rad2Deg);
            if (angleView < 0) angleView = 360 + angleView;
            if (Debugger != null) Debugger.text = angleView.ToString();
            
            moveCardTransform.rotation = Quaternion.Euler(0, angleView-90, 0);
        }
    }

    void UpdateSprite()
    {
        if (angleView>currentInterval.max || angleView < currentInterval.min)
        {
            SpriteAngle newSA = spriteDictionary.First(sw => sw.Key(angleView)).Value;
            sprite.sprite = newSA.sprite;
            currentInterval = newSA.angleInterval;
        }  
    }
    public void UpdateStats()
    {
        moveSpeed = GetComponent<PlayerStat>().moveSpeedValue;
    }
    
    private void Animations()
    {
            Vector3 animDir;
            if(GameManager.Instance.currentContorller == GameManager.controller.Keybord) animDir = (pointerPosition - transform.position).normalized;
            else animDir = -movePlayerTransform.forward ;

            AnimatorClipInfo animInfo = animatorPlayer.GetCurrentAnimatorClipInfo(0)[0];
            if (animInfo.clip.name == "waitAttackState") inAttack = false;
            animatorPlayer.SetBool("inCombo", setNextCombo);
            animatorPlayer.SetInteger("attackCounter", attackCounter);
            if ((animInfo.clip.name.Contains("SLASH") || animInfo.clip.name.Contains("SPIN"))  && !inAttackAnim)
            {
                if (animInfo.clip.name.Contains("SLASH"))
                {
                    if (GameManager.Instance.currentContorller == GameManager.controller.Keybord) rb.AddForce(moveCardTransform.forward * -500);
                    else rb.AddForce(movePlayerTransform.forward * -500);
                }
                if(GameManager.Instance.currentContorller == GameManager.controller.Keybord) keybordAttackZones[attackCounter-1].SetActive(true);
                else
                {
                    Debug.Log(attackCounter);
                    controllerAttackZones[attackCounter-1].SetActive(true);
                }
                //Debug.Log($"Attack {attackCounter}, GO :{attackZones[attackCounter-1].name}");
                inAttack = true;
                inAttackAnim = true;
                setNextCombo = false;
                //Debug.Log("Begin Attack");
                StopCoroutine(ComboWait());
                comboWaiting = false;
            }
            else if (animInfo.clip.name == "waitAttackState" && inAttackAnim)
            {
                DesactiveAttackZone();
                
                if (!setNextCombo && !comboWaiting)
                {
                    comboWaiting = true;
                    StartCoroutine(ComboWait()); 
                    Debug.Log("Combo Wait");
                }
                    
                inAttackAnim = false;
            }
            else if (animInfo.clip.name == "waitAttackState" && !inAttackAnim && !inAttack && !comboWaiting) 
            { 
                comboWaiting = true; 
                StartCoroutine(ComboWait());
                Debug.Log("Combo Wait");
            }
            
            if (!inAttack && canMove)
            {
                animatorPlayer.SetFloat("X-Axis", lastDir.x);
                animatorPlayer.SetFloat("Z-Axis", lastDir.z);
                animatorPlayer.SetBool("isAttack", inAttack);
                animatorPlayer.SetBool("isRun", moving);
                animatorMovePlayer.SetBool("isWalk", moving); // Il est différant donc repoussé par la société
            }
            else
            {
                if (dashing)
                {
                    animatorPlayer.SetFloat("X-Axis", lastDir.x);
                    animatorPlayer.SetFloat("Z-Axis", lastDir.z);  
                }
                else
                {
                    animatorPlayer.SetFloat("X-Axis", animDir.x);
                    animatorPlayer.SetFloat("Z-Axis", animDir.z);
                }
                animatorPlayer.SetBool("isAttack", inAttack);
                animatorPlayer.SetBool("isRun", moving);
            }
    }

    void DesactiveAttackZone()
    {
        GameObject[] attackZones;
        if(GameManager.Instance.currentContorller == GameManager.controller.Keybord) attackZones = keybordAttackZones;
        else attackZones = controllerAttackZones;
        foreach (var GO in attackZones)
        {
            GO.SetActive(false);
            //Debug.Log("Desactive Attack Zone");
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Camera"))
        {
            camera.ChangePoint(other.transform);
            cameraOnPlayer = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Camera"))
        {
            camera.ChangePoint(PlayerCameraPoint, true);
            cameraOnPlayer = true;
        }
    }

    public IEnumerator ComboWait()
    {
        yield return new WaitForSeconds(0.15f);
        if (!inAttack)
        {
            animatorPlayer.SetBool("attackFinish", true);
            attackCounter = 0;
            Debug.Log("Attack Finish");
            canMove = true;
        }
    }
}
