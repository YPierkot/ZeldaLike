using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class Controller : MonoBehaviour
{
    #region CLASS & Other

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
    [SerializeField] private Animator animatorPlayer;
    [SerializeField] private Animator animatorMovePlayer;
    
    private CardsController cardControl;
    public static Controller instance;
    
   // --- STATES ---
    private bool moving;
    private bool dashing;
    private bool inAttack;

    private float dashTimer;
    [SerializeField] public bool canMove = true;
    
    [SerializeField] private SpriteAngle[] spriteArray;
    private Dictionary<Func<float, bool>, SpriteAngle> spriteDictionary = new Dictionary<Func<float, bool>, SpriteAngle>();


    private PlayerInputMap InputMap;
    [SerializeField] private LayerMask pointerMask;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDistance;
    [SerializeField] Transform moveTransform;
    private Vector3 lastDir;
    
    [Header("--- CAMERA ---")] 
    [SerializeField] private CameraController camera;
    [SerializeField] private Transform PlayerCameraPoint;
    private Vector3 cameraOffset;
    private bool cameraOnPlayer = true;
    private bool dashCamera;

    [NonSerialized] public Vector3 pointerPosition;
    private float angleView;
    private Interval currentInterval = new Interval{ min=61, max=120 };
    
    [Header("--- ATTAQUE ---")] 
    [SerializeField] private Animator attackZone;
    public int attackCounter;
    [SerializeField] public bool nextCombo;
    [SerializeField] public int attackDamage;

    [Header("--- PARAMETRES ---")] 
    [SerializeField] private float moveSpeed;
    [SerializeField] private AnimationCurve dashCurve;

    
    [Header("--- DEBUG ---")] 
    [SerializeField] private TextMeshProUGUI Debugger;
    [SerializeField] private Transform transformDebugger;
    
    
    void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
        
        InputMap = new PlayerInputMap();
        InputMap.Enable();
        InputMap.Movement.Rotation.performed += RotationOnperformed;
        InputMap.Movement.Dash.performed += context => Dash();
        InputMap.Movement.Position.started += context => moving = true;
        InputMap.Movement.Position.canceled += context => moving = false;

        InputMap.Action.shortCard.performed += context => cardControl.ShortRange();
        InputMap.Action.longCard.performed += context => cardControl.LongRange();
        InputMap.Action.Attack.performed += context => Attack();

        InputMap.Menu.CardMenu.performed += context => Debug.Log("scroll");
    }


    #region Input Methode

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
        
        if(Input.GetAxis("Mouse ScrollWheel")> 0f) UIManager.Instance.ChangeCard(1);
        if(Input.GetAxis("Mouse ScrollWheel")< 0f) UIManager.Instance.ChangeCard(-1);
        
    }

    
    private void FixedUpdate()
    {
        
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
        Vector3 dir = new Vector3(InputMap.Movement.Position.ReadValue<Vector2>().x, 0, InputMap.Movement.Position.ReadValue<Vector2>().y).normalized;
        lastDir = dir;
        rb.AddForce(dir * moveSpeed);
    }

    void Dash()
    {
        if (!dashing && canMove)
        {
            dashing = true;
            dashTimer = 0;
            canMove = false;
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
                StopCoroutine(ComboWait());
                canMove = false;
                inAttack = true;
                nextCombo = false;
                //attackZone.collider.enabled = true;
                attackCounter++;
                attackZone.Play($"Attack{attackCounter}");
                if (attackCounter != 3)
                {
                    rb.AddForce(moveTransform.forward*-700);
                }
            }
            else
            {
                nextCombo = true;
            }
        }
    }

    public void CheckAttack()
    {
        inAttack = false;
        if (!nextCombo || attackCounter == 3)
        {
            StartCoroutine(("ComboWait"));
        }
        else
        {
            Attack();
        }
    }

    void Rotate(Vector2 rotation)
    {
        if (!inAttack)
        {
            angleView = -(Mathf.Atan2(rotation.y, rotation.x)*Mathf.Rad2Deg);
            if (angleView < 0) angleView = 360 + angleView;
            if (Debugger != null)
                Debugger.text = angleView.ToString();
            
            moveTransform.rotation = Quaternion.Euler(0, angleView-90, 0);
            //UpdateSprite();
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


    public IEnumerator ComboWait()
    {
        yield return new WaitForSeconds(0.15f);
        if (!inAttack)
        {
            attackCounter = 0;
            canMove = true;
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

    private void Animations()
    {
        var animDir = pointerPosition - transform.position;
        animDir.Normalize();

        if (!inAttack)
        {
            animatorPlayer.SetFloat("X-Axis", animDir.x);
            animatorPlayer.SetFloat("Z-Axis", animDir.z);
            animatorPlayer.SetBool("isAttack", inAttack);
            animatorPlayer.SetBool("isRun", moving);
            animatorMovePlayer.SetBool("isWalk", moving);
        }
        else
        {
            animatorPlayer.SetFloat("X-Axis", animDir.x);
            animatorPlayer.SetFloat("Z-Axis", animDir.z);
            animatorPlayer.SetBool("isAttack", inAttack);
            animatorPlayer.SetBool("isRun", moving);
        }
    }
}
