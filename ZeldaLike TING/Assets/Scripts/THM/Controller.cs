using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class Controller : MonoBehaviour
{
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
    
    private SpriteRenderer sprite;
    private Rigidbody rb;
    
    [SerializeField] private SpriteAngle[] spriteArray;
    private Dictionary<Func<float, bool>, SpriteAngle> spriteDictionary = new Dictionary<Func<float, bool>, SpriteAngle>();


    private PlayerInput Input;
    [SerializeField] Transform moveTransform;

    private float angleView;
    private Interval currentInterval = new Interval{ min=61, max=120 };

    [Header("--- PARAMETRES ---")] 
    [SerializeField] private float moveSpeed;
    
    [SerializeField] private TextMeshProUGUI Debugger;
    private Sprite defaulft;

    void Awake()
    {
        Input = new PlayerInput();
        Input.Enable();
        Input.Movement.Rotation.performed += RotationOnperformed;
        Input.Movement.Position.performed += Move;
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

    void Move(InputAction.CallbackContext obj)
    {
        Vector3 dir = new Vector3(obj.ReadValue<Vector2>().x, 0, obj.ReadValue<Vector2>().y).normalized * moveSpeed;
        rb.AddForce(dir);
    }
    
    void CheckController(InputUser user, InputUserChange change, InputDevice device) {
        if (change == InputUserChange.ControlSchemeChanged) {
            Debug.Log(user.controlScheme.Value.name);
            switch (user.controlScheme.Value.name)
            {
                
            }
        }
    }
    
    #endregion

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
        
        InputUser.onChange += CheckController;
        foreach (SpriteAngle SA in spriteArray)
        {
            spriteDictionary.Add(x => x < SA.angleInterval.max, SA);
        }
    }

    void Update()
    {
         
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
