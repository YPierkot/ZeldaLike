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
    
    private CardsController cardControl;
    [SerializeField] private Animator attackZone;
    
   // --- STATES ---
    private bool moving;
    private bool dashing;
    private bool inAttack;

    private float dashTimer;
    public int attackCounter;
    [SerializeField] public bool nextCombo;
    [SerializeField] public bool waitForCombo;
    [SerializeField] public bool canMove = true;
    
    [SerializeField] private SpriteAngle[] spriteArray;
    private Dictionary<Func<float, bool>, SpriteAngle> spriteDictionary = new Dictionary<Func<float, bool>, SpriteAngle>();


    private PlayerInputMap InputMap;
    [SerializeField] Transform moveTransform;
    private Vector3 lastDir;

    private float angleView;
    private Interval currentInterval = new Interval{ min=61, max=120 };

    [Header("--- PARAMETRES ---")] 
    [SerializeField] private float moveSpeed;
    [SerializeField] private AnimationCurve dashCurve;
    
    
    [Header("--- DEBUG ---")] 
    [SerializeField] private TextMeshProUGUI Debugger;
    [SerializeField] private Transform transformDebugger;

    public static Controller instance;
    
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
            Physics.Raycast(ray, out hit);
            transformDebugger.position = hit.point;
            Vector2 vector = (new Vector2(hit.point.x, hit.point.z) - new Vector2(transform.position.x, transform.position.z)).normalized;
            Rotate(vector);
        }
        
        if (dashing) 
        {
            if (dashTimer > dashCurve[dashCurve.length-1].time)
            {
                dashing = false;
                canMove = true;
            }
            
            rb.velocity = (lastDir*dashCurve.Evaluate(dashTimer)*moveSpeed); 
            dashTimer += Time.deltaTime;
        }
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

        ;
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
        }
    }

    void Attack()
    {
        if (attackCounter < 3)
        {
            if (!inAttack)
            {
                StopCoroutine("ComboWait");
                canMove = false;
                inAttack = true;
                nextCombo = false;
                Debug.Log("Attack");
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
            StartCoroutine("ComboWait");
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
            UpdateSprite();
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
        attackCounter = 0;
        canMove = true;
    }
    
}
