using System;
using System.Collections;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
   public static PlayerStat instance;
   private Controller _control;
   private AttackControl _atkControl;
   
   
   [Header("LIFE")]
   [SerializeField] private int lifeMax;
   [SerializeField] private int life;
   
   
   [Header("Debug")] 
   
   [SerializeField] private CameraShakeScriptable HitShake;

   
   [Header("MODULES")]
   [SerializeField] private int sharpnessModuleComposant;
   [SerializeField] private int longSwordModuleComposant;
   [SerializeField] private int knockbackModuleComposant;
   [SerializeField] private int toughnessModuleComposant;
   [SerializeField] private int thornModuleComposant;
   [SerializeField] private int rockModuleComposant;
   [SerializeField] private int swiftnessModuleComposant;
   [SerializeField] private int staminaModuleComposant;

   [Header("Stats for modules")] 
   [SerializeField] public float toughnessValue = 0.3f; // Duration u can't take dmg
   [SerializeField] public int attackDamageValue = 1;
   [SerializeField] public float moveSpeedValue = 100; // Player MS
   [SerializeField] private float repulseForce = 25; // Player's KB when hitted
   [SerializeField] public float enemyKBForce = 3; // Enemies KB when hitted
   
   private bool isImmune;

   private void Awake()
   {
      _control = GetComponent<Controller>();
      life = lifeMax;
      toughnessValue = 0.3f;
      attackDamageValue = 1;
      moveSpeedValue = 100;
      repulseForce = 25;
   }

   private void Start()
   {
      UIManager.Instance.UpdateLife(life);
   }
   
   public void TakeDamage(int damage = 1)
   {
      if (!isImmune)
      {
         life -= damage;
         StartCoroutine(TakeDamageCD());
         UIManager.Instance.UpdateLife(life);
         CameraShake.Instance.AddShakeEvent(HitShake);
      }
   }

   private void OnCollisionEnter(Collision other)
   {
      if (other.transform.CompareTag("Ennemy")) 
      {
         TakeDamage();
         StartCoroutine(HitCD());
         _control.canMove = false;
         Vector3 repulse = (transform.position - other.contacts[0].point).normalized * repulseForce;
         _control.rb.velocity = repulse;
      }
   }

   IEnumerator HitCD()
   {
      _control.canMove = false;
      yield return new WaitForSeconds(0.3f);
      _control.canMove = true;
   }
   
   IEnumerator TakeDamageCD()
   {
      isImmune = true;
      yield return new WaitForSeconds(toughnessValue);
      isImmune = false;
   }

   #region UpgradesBuyables

   public void UpgradeToughness(int level)
   {
      switch (level)
      {
         case 1: toughnessValue = 1f;
            break;
         case 2: toughnessValue = 2f;
            break;
         case 3: toughnessValue = 4f;
            break;
         default:
            break;
      }
   }

   public void UpdateKB(int level)
   {
      switch (level)
      {
         case 1: enemyKBForce = 9f;
            break;
         case 2: enemyKBForce = 17f;
            break;
         case 3: enemyKBForce = 25f;
            break;
         default:
            break;
      }
   }

   public void UpgradeSharpness(int level)
   {
      switch (level)
      {
         case 1: attackDamageValue = 1;
            break;
         case 2: attackDamageValue = 2;
            break;
         case 9: attackDamageValue = 3;
            break;
         default:
            break;
      }
      _atkControl.UpdateAttackStats();
   }

   public void UpgradeSwiftness(int level)
   {
      switch (level)
      {
         case 1: moveSpeedValue = 100;
            break;
         case 2: moveSpeedValue = 120;
            break;
         case 3: moveSpeedValue = 135;
            break;
         default:
            break;
      }
      _control.UpdateStats();
   }
   
   public void UpgradeRockness(int level)
   {
      switch (level)
      {
         case 1: repulseForce = 17;
            break;
         case 2: repulseForce = 9;
            break;
         case 3: repulseForce = 3;
            break;
         default:
            break;
      }
   }
   #endregion
   
}
