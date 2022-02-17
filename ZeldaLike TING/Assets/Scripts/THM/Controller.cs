using System;
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
    
    
    private SpriteRenderer sprite;
    private Rigidbody rb;
    private PlayerInput _playerInput;
    
  
    private bool moving = false;
    private bool canMove = true;
    
    [SerializeField] private SpriteAngle[] spriteArray;
    private Dictionary<Func<float, bool>, SpriteAngle> spriteDictionary = new Dictionary<Func<float, bool>, SpriteAngle>();


    private PlayerInputMap InputMap;
    [SerializeField] Transform moveTransform;

    private float angleView;
    private Interval currentInterval = new Interval{ min=61, max=120 };

    [Header("--- PARAMETRES ---")] 
    [SerializeField] private float moveSpeed;
    
    [SerializeField] private TextMeshProUGUI Debugger;
    [SerializeField] private Transform transformDebugger;

    void Awake()
    {
        InputMap = new PlayerInputMap();
        InputMap.Enable();
        InputMap.Movement.Rotation.performed += RotationOnperformed;
        InputMap.Movement.Position.started += context => moving = true;
        InputMap.Movement.Position.canceled += context => moving = false;
    }
    #region Input Methode

    private void RotationOnperformed(InputAction.CallbackContext obj)
    {
        Vector2 rotation = obj.ReadValue<Vector2>().normalized;
        angleView = -(Mathf.Atan2(rotation.y, rotation.x)*Mathf.Rad2Deg);
            if (angleView < 0) angleView = 360 + angleView;
            if (Debugger != null)
                Debugger.text = angleView.ToString();
            
            moveTransform.rotation = Quaternion.Euler(0, angleView-90, 0);
        
        UpdateSprite();
    }


    #endregion

    
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();

        foreach (SpriteAngle SA in spriteArray)
        {
            spriteDictionary.Add(x => x < SA.angleInterval.max, SA);
        }
        
    }

    private void FixedUpdate()
    {
        transformDebugger.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
        Vector3 dir = new Vector3(InputMap.Movement.Position.ReadValue<Vector2>().x, 0, InputMap.Movement.Position.ReadValue<Vector2>().y).normalized * moveSpeed;
        rb.AddForce(dir);
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
    
}
