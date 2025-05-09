using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class 
    Controller : MonoBehaviour
{
    #region CLASS & Other

    public enum ControlType
    {
        MoveStopShoot, HoldForLong, ChangeSideControl, FastCast
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
    public PlayerInput _playerInput;
    public ControlType _controlType;
    public bool secondStick;
    private ControlType lastControlType;
    public Animator animatorPlayer;
    [SerializeField] public Animator animatorPlayerHand;
    [SerializeField] private Animator animatorMovePlayer;
    
    private CardsController cardControl;
    public static Controller instance;
    
    [Header("--- PLAYER STATES ---")] [Space(10)] 
   // --- STATES ---
   public bool moving;
   public bool dashing;
   public bool isSlow; 
   [SerializeField] public bool inAttack;
   [SerializeField] private bool launchAttack;
   [SerializeField] private bool inAttackAnim;
   [SerializeField] private bool holdingForCard;
   public bool canDash;
   
   private bool moveHoldCard;
   
   private float dashTimer;
   private float holdTimer; 
   public int dashAvailable; 
   private float dashCDtimer;
   
   [SerializeField] public bool canMove = true;
    
   [SerializeField] private SpriteAngle[] spriteArray;
   private System.Collections.Generic.Dictionary<Func<float, bool>, SpriteAngle> spriteDictionary = new System.Collections.Generic.Dictionary<Func<float, bool>, SpriteAngle>();


    private PlayerInputMap InputMap;
    public delegate void Interaction();
    public Interaction playerInteraction;
    public Interaction pause;
    
    
    [SerializeField] private LayerMask pointerMask;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDistance;
    public Transform moveCardTransform;
    public Transform movePlayerTransform;
    public Vector3 lastDir;
    private float anglePlayerView;
    
    [Header("--- CAMERA ---")] 
    [SerializeField] private CameraController camera;
    public Transform PlayerCameraPoint;
    private Vector3 cameraOffset;
    private bool cameraOnPlayer = true;
    private bool dashCamera;

    [NonSerialized] public Vector3 pointerPosition;
    [HideInInspector] public float angleView;
    private Interval currentInterval = new Interval{ min=61, max=120 };
    
    [Header("--- ATTAQUE ---")] 
    public int attackCounter;
    [SerializeField] public bool setNextCombo;
    [SerializeField] private GameObject[] keybordAttackZones;
    [SerializeField] private GameObject[] controllerAttackZones;

    private bool comboWaiting;

    [Header("--- PARAMETRES ---")] 
    [SerializeField] private float joystickDeadZone;
    [SerializeField] private float moveSpeed;
    [SerializeField] private AnimationCurve dashCurve;
    public int maxDash; 
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

        InputMap.Action.Interaction.performed += InteractionPerformed;
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
                cardControl.fireRectoUse = cardControl.iceRectoUse = cardControl.wallRectoUse = cardControl.windRectoUse = true;
            };
            InputMap.MoveStopShoot.holdForShoot.canceled += context =>
            {
                moveHoldCard = false;
                canMove = true;
                cardControl.fireRectoUse = cardControl.iceRectoUse = cardControl.wallRectoUse = cardControl.windRectoUse = false;
            };
            InputMap.MoveStopShoot.shoot.canceled += CardHolderOncanceled;
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
        else if (_controlType == ControlType.FastCast)
        {
            InputMap.FastCast.ShortShoot.performed += context => cardControl.ShortRange();
            InputMap.FastCast.LongShoot.performed += context => cardControl.LongRange();
        }
        
        InputMap.Menu.CardMenu.performed += SwitchCard;
        InputMap.Menu.Pause.performed += context => {if (pause != null) pause(); Debug.Log("Pause Press");};
    }

    private void InteractionPerformed(InputAction.CallbackContext obj)
    {
        if(playerInteraction != null) playerInteraction();
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
        if(obj.ReadValue<float>() == -1) UIManager.Instance.ChangeCard(1); 
        else UIManager.Instance.ChangeCard(-1); 
    }

    private void RotationOnperformed(InputAction.CallbackContext obj)
    {
        if (!DialogueManager.Instance.isCinematic && Vector2.Distance(Vector2.zero,obj.ReadValue<Vector2>()) > joystickDeadZone)
        {
          Vector2 rotation = obj.ReadValue<Vector2>().normalized;
          Rotate(rotation);  
        }

        
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
            Vector3 vect = Input.mousePosition;
            vect.z = Vector3.Distance(camera.transform.position, transform.position);
            pointerPosition = Camera.main.ScreenToWorldPoint(vect);
            if (!DialogueManager.Instance.isCinematic)
            {
                Vector2 vector = (new Vector2(pointerPosition.x, pointerPosition.z) - new Vector2(transform.position.x, transform.position.z)).normalized;
                Rotate(vector);
            }
        }

        if (lastControlType != _controlType)
        {
            SetInputMap();
            if (_controlType == ControlType.MoveStopShoot) secondStick = false;
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
                UIManager.Instance.UpdateDash(dashAvailable, true);
            }
        }

        //if (Debugger != null) Debugger.text = $"Dash Available : {dashAvailable}, CD : {dashCDtimer}";
        
        if(Input.GetAxis("Mouse ScrollWheel")> 0f) UIManager.Instance.ChangeCard(1);
        if(Input.GetAxis("Mouse ScrollWheel")< 0f) UIManager.Instance.ChangeCard(-1);
        
    }
    
    private void FixedUpdate()
    {
        Vector3 dir = Vector3.zero;
        if (!DialogueManager.Instance.isCinematic)
        {
            dir = new Vector3(InputMap.Movement.Position.ReadValue<Vector2>().x, 0, InputMap.Movement.Position.ReadValue<Vector2>().y).normalized;
            lastDir = dir;
            
        }
        
        
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
            anglePlayerView = -(Mathf.Atan2(dir.z, dir.x)*Mathf.Rad2Deg); 
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
        anglePlayerView = -(Mathf.Atan2(dir.z, dir.x)*Mathf.Rad2Deg); 
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
        if (!dashing && (canMove || animatorPlayer.GetCurrentAnimatorClipInfo(0)[0].clip.name == "waitAttackState") && dashAvailable > 0 && canDash)
        {
            animatorPlayer.SetBool("attackFinish", true);
            SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.dash, .2f); 
            dashAvailable--;
            dashing = true;
            dashTimer = 0;
            dashCDtimer = 0;
            canMove = false;
            inAttack = false;
            attackCounter = 0;
            UIManager.Instance.UpdateDash(dashAvailable);
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
                launchAttack = true;
                canMove = false;
                attackCounter++;
                inAttack = true;

                CancelInvoke("ComboWait");
                
            }
        }
    }
    public void Rotate(Vector2 rotation)
    {
        if (!inAttack)
        {
            if (DialogueManager.Instance.isCinematic) lastDir = new Vector3(rotation.x, 0, rotation.y);
            angleView = -(Mathf.Atan2(rotation.y, rotation.x)*Mathf.Rad2Deg);
            if (angleView < 0) angleView = 360 + angleView;
            if (Debugger != null) Debugger.text = angleView.ToString();
            
            moveCardTransform.rotation = Quaternion.Euler(0, angleView-90, 0);
        }
    }
    
    public void UpdateStats()
    {
        moveSpeed = GetComponent<PlayerStat>().moveSpeedValue;
    }
    
    private void Animations()
    {
        DialogueManager _dialogueManager = DialogueManager.Instance; 
        Vector3 animDir;
        if(GameManager.Instance.currentContorller == GameManager.controller.Keybord) animDir = (pointerPosition - transform.position).normalized;
        else animDir = -movePlayerTransform.forward ;

        animatorPlayer.SetInteger("attackCounter", attackCounter);
        animatorPlayer.SetBool("isAttack", launchAttack);
        animatorPlayerHand.SetInteger("attackCounter", attackCounter);
        animatorPlayerHand.SetBool("isAttack", launchAttack);
            
        AnimatorClipInfo animInfo = animatorPlayer.GetCurrentAnimatorClipInfo(0)[0];
        if ((animInfo.clip.name.Contains("Idle") || animInfo.clip.name.Contains("Run")) && inAttackAnim)
        {
            foreach (var zone in keybordAttackZones   ) zone.SetActive(false);
            foreach (var zone in controllerAttackZones) zone.SetActive(false);
            
            inAttack = false;
            inAttackAnim = false;
            canMove = true;
            Invoke("ComboWait", 1f);
        }
        else if((animInfo.clip.name.Contains("SLASH") || animInfo.clip.name.Contains("SPIN")))
        {
            if (!inAttackAnim)
            {
                SoundEffectManager _soundEffectManager = SoundEffectManager.Instance;
                switch (attackCounter)
                {
                    case 1: _soundEffectManager.PlaySound(_soundEffectManager.sounds.attack1); break;
                    case 2: _soundEffectManager.PlaySound(_soundEffectManager.sounds.attack2); break;
                    case 3: _soundEffectManager.PlaySound(_soundEffectManager.sounds.attack3); break;
                }
                if (GameManager.Instance.currentContorller == GameManager.controller.Keybord)
                {
                    rb.AddForce(moveCardTransform.forward * -500);
                    keybordAttackZones[attackCounter-1].SetActive(true);
                }
                else
                {
                    rb.AddForce(movePlayerTransform.forward * -500);
                    controllerAttackZones[attackCounter-1].SetActive(true);
                }
            }
            launchAttack = false;
            inAttackAnim = true;
            
            animatorPlayer.SetFloat("X-Axis", animDir.x);
            animatorPlayer.SetFloat("Z-Axis", animDir.z);
        }
        if (!inAttack && canMove && moving)
        {
            animatorPlayer.SetFloat("X-Axis", lastDir.x);
            animatorPlayer.SetFloat("Z-Axis", lastDir.z);

            if (!_dialogueManager.isCinematic)
            {
                animatorPlayer.SetBool("isRun", moving);
                animatorPlayerHand.SetBool("isRun", moving);
                animatorMovePlayer.SetBool("isWalk", moving);
            } // Il est différent donc repoussé par la société
        }

        if (_dialogueManager.isCinematic)
        {
            animatorPlayer.SetFloat("X-Axis", lastDir.x);
            animatorPlayer.SetFloat("Z-Axis", lastDir.z);

            if (moving && _dialogueManager.playerCanMove)
            {
                Debug.Log("Set Run");
                animatorPlayer.SetBool("isRun", moving);
                animatorPlayerHand.SetBool("isRun", moving);
                animatorMovePlayer.SetBool("isWalk", moving);
            }
        }
        else
        {
                
            if (moving)
            {
                if (dashing || _dialogueManager.isCinematic)
                {
                    animatorPlayer.SetFloat("X-Axis", lastDir.x);
                    animatorPlayer.SetFloat("Z-Axis", lastDir.z);  
                }
            }

            if (!_dialogueManager.isCinematic)
            {
                animatorPlayer.SetBool("isRun", moving);
                animatorPlayerHand.SetBool("isRun", moving);
            }
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

    public void ChangeControleType(int value)
    {
        _controlType = (ControlType) value;
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

    private int waitIndex = 0;
    public void ComboWait()
    {
        
        if (!inAttack)
        {
            attackCounter = 0;
            //canMove = true;
        }
    }

    public void FreezePlayer(bool freeze, string toFreeze = "All")
    {
        DialogueManager _dialogueManager = DialogueManager.Instance;
        CardsController _cardsController = CardsController.instance;
        
        if (freeze)
        {
            _dialogueManager.playerCanMove = false;
            switch (toFreeze)
            {
                case "Dash":
                    canDash = false;
                    break;
                case "Attack":
                    attackCounter = 3;
                    inAttack = false;
                    break;
                case "All":
                    canMove = false;
                    inAttack = false;
                    canDash = false;
                    _cardsController.canUseCards = false;
                    attackCounter = 3;
                    break;
                case "Cards":
                    _cardsController.canUseCards = false;
                    break;
                case "DashAttack":
                    inAttack = false;
                    canDash = false;
                    attackCounter = 3;
                    break;
            }
        }
        else
        {
            _dialogueManager.playerCanMove = false;
            canMove = true;
            _cardsController.canUseCards = true;
            canDash = true;
            attackCounter = 0;
            inAttack = false;
        }
    }
}
