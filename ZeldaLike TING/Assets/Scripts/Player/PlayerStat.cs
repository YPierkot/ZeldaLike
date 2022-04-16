using System;
using System.Collections;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
   public static PlayerStat instance;
   private Controller control;
   
   
   [Header("LIFE")]
   [SerializeField] private int lifeMax;
   [SerializeField] private int life;
   
   
   [Header("Debug")] 
   
   [SerializeField] private CameraShakeScriptable HitShake;

   
   [Header("MODULES")]
   [SerializeField] private int sharpnessModuleLevel;
   [SerializeField] private int longSwordModuleLevel;
   [SerializeField] private int knockbackModuleLevel;
   [SerializeField] private int toughnessModuleLevel;
   [SerializeField] private int thornModuleLevel;
   [SerializeField] private int rockModuleLevel;
   [SerializeField] private int swiftnessModuleLevel;
   [SerializeField] private int staminaModuleLevel;

   [Header("Stats for modules")] 
   [SerializeField] public float toughnessValue = 0.3f; // Duration u can't take dmg
   [SerializeField] public int attackDamageValue = 1;
   [SerializeField] public float moveSpeedValue = 100;
   [SerializeField] private float repulseForce = 25;
   
   private bool isImmune;

   private void Awake()
   {
      control = GetComponent<Controller>();
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
         control.canMove = false;
         Vector3 repulse = (transform.position - other.contacts[0].point).normalized * repulseForce;
         control.rb.velocity = repulse;
      }
   }

   IEnumerator HitCD()
   {
      control.canMove = false;
      yield return new WaitForSeconds(0.3f);
      control.canMove = true;
   }
   
   IEnumerator TakeDamageCD()
   {
      isImmune = true;
      yield return new WaitForSeconds(toughnessValue);
      isImmune = false;
   }

   #region UpgradesBuyables

   public void UpgradeToughness()
   {
      switch (toughnessModuleLevel)
      {
         case 3: toughnessValue = 1f;
            break;
         case 6: toughnessValue = 2f;
            break;
         case 9: toughnessValue = 4f;
            break;
         default:
            break;
      }
   }
   
   public void UpgradeSharpness()
   {
      switch (sharpnessModuleLevel)
      {
         case 3: toughnessValue = 1;
            break;
         case 6: toughnessValue = 2;
            break;
         case 9: toughnessValue = 3;
            break;
         default:
            break;
      }
      AttackControl.Instance.UpdateDMG();
   }

   public void UpgradeSwiftness()
   {
      switch (swiftnessModuleLevel)
      {
         case 3: moveSpeedValue = 100;
            break;
         case 6: toughnessValue = 120;
            break;
         case 9: toughnessValue = 135;
            break;
         default:
            break;
      }
      control.UpdateStats();
   }
   
   public void UpgradeRockness()
   {
      switch (rockModuleLevel)
      {
         case 3: repulseForce = 17;
            break;
         case 6: repulseForce = 9;
            break;
         case 9: repulseForce = 0;
            break;
         default:
            break;
      }
   }
   

   #endregion
   
}
