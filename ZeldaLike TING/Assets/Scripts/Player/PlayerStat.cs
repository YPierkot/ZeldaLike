using System;
using System.Collections;
using UnityEngine;
using Update = UnityEngine.PlayerLoop.Update;

public class PlayerStat : MonoBehaviour
{
   public static PlayerStat instance;
   private Controller _control;
   [SerializeField] private GameObject[] attackCol1;
   [SerializeField] private GameObject[] attackCol2;

   [Header("LIFE")]
   public int lifeMax;
   public int life;

   [SerializeField] private bool isImmune;
   private bool noDamage;
   
   [Header("Debug")]
   [SerializeField] private CameraShakeScriptable HitShake;

   
   [Header("MODULES")]
   [SerializeField] public int sharpnessModuleComposant;
   [SerializeField] public int longSwordModuleComposant;
   [SerializeField] public int knockbackModuleComposant;
   [SerializeField] public int toughnessModuleComposant;
   [SerializeField] public int thornModuleComposant;
   [SerializeField] public int rockModuleComposant;
   [SerializeField] public int swiftnessModuleComposant;
   [SerializeField] public int staminaModuleComposant;

   [HideInInspector] public moduleScriptableUI[] equippedModules;

   [Header("Stats for modules")] 
   public int money;
   
   [Header("Stats for modules")]
   [SerializeField] public float toughnessValue = 0.3f; // Duration u can't take dmg
   [SerializeField] public int attackDamageValue = 1;
   [SerializeField] public float moveSpeedValue = 100; // Player MS  
   [SerializeField] private float repulseForce = 25; // Player's KB when hitted
   [SerializeField] public float enemyKBForce; // Enemies KB when hitted
   
   
   private void Awake()
   {
      instance = this;
      _control = GetComponent<Controller>();
      life = lifeMax;
      toughnessValue = 0.3f;
      attackDamageValue = 1;
      moveSpeedValue = 100;
      repulseForce = 25;
      UpdateAttackStats();
   }

   private void Start()
   {
      UIManager.Instance.InitLife(life);
      equippedModules = new moduleScriptableUI[3];
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.O)) noDamage = !noDamage;
      if (Input.GetKeyDown(KeyCode.M)) TakeDamage();
      if (Input.GetKeyDown(KeyCode.R)) PlayerRespawn();
   }

   
   public void TakeDamage(int damage = 1)
   {
      if(!noDamage)StartCoroutine(TakeDamageCo(damage));
   }
   
   public IEnumerator TakeDamageCo(int damage)
   {
      yield return new WaitForSeconds(0.25f);
      if (!isImmune && !_control.dashing && life > 0)
      {
         life -= damage;

         if (life <= 0) 
            StartCoroutine(PlayerDeath());
         else
         {
            StartCoroutine(TakeDamageCD());
            UIManager.Instance.TakeDamageUI(life);
            CameraShake.Instance.AddShakeEvent(HitShake);
         }
      }
   }
   

   public void ChangeMoney(int amount)
   {
      money += amount;
      Debug.Log("Change money");
      UIManager.Instance.changingMoney = true;
   }

   public IEnumerator PlayerDeath() 
   {
      UIManager.Instance.TakeDamageUI(life);
      _control.animatorPlayer.SetBool("isDead", true);
      CardsController.instance.canUseCards = false;
      _control.FreezePlayer(true);
      
      yield return new WaitForSeconds(3f);
      
      PlayerRespawn();
   }

   private void PlayerRespawn()
   {
      SaveManager _saveManager = SaveManager.instance;
      Controller.instance.transform.position = _saveManager.spawnPointSave.position;
      _control.animatorPlayer.SetBool("isDead", false);
      life = _saveManager.lifeSave;
      money = _saveManager.moneySave;
      _control.FreezePlayer(false);
      Time.timeScale = 1f;
      UIManager.Instance.InitLife(life);
      //UIManager.Instance.RecoverAllLife(life);
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
      Debug.Log("Toughness just got upgraded");
      switch (level)
      {
         case 0: toughnessValue = 0f;
            break;
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
      Debug.Log("Enemy knockback just got upgraded");
      switch (level)
      {
         case 0: enemyKBForce = 0f;
            break;
         case 1: enemyKBForce = 9f;
            break;
         case 2: enemyKBForce = 17f;
            break;
         case 3: enemyKBForce = 25f;
            break;
         default:
            break;
      }
      
      UpdateAttackStats();
   }

   public void UpgradeSharpness(int level)
   {
      Debug.Log("Sharpness just got upgraded");
      switch (level)
      {
         case 0: attackDamageValue = 0;
            break;
         case 1: attackDamageValue = 1;
            break;
         case 2: attackDamageValue = 2;
            break;
         case 9: attackDamageValue = 3;
            break;
         default:
            break;
      }

      UpdateAttackStats();
   }

   public void UpgradeSwiftness(int level)
   {
      Debug.Log("Swiftness just got upgraded");
      switch (level)
      {
         case 0: moveSpeedValue = 0;
            break;
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
      Debug.Log("Rockness just got upgraded");
      switch (level)
      {
         case 0: repulseForce = 0;
            break;
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

   public void UpgradeStamina(int level)
   {
      
   }

   public void UpdgradeThorn(int level)
   {
      
   }

   public void UpgradeLongSword(int level)
   {
      Debug.Log("Longsword module just got upgraded");
      switch (level)
      {
         // LEVEL 1
         case 1:
            for (int i = 0; i < attackCol1.Length; i++)
            {
               switch (i)
               {
                  case 0:
                     attackCol1[i].transform.localPosition = new Vector3(attackCol1[i].transform.localPosition.x,
                        attackCol1[i].transform.localPosition.y,  -0.8f);
                     attackCol1[i].transform.localScale = new Vector3(attackCol1[i].transform.localScale.x, 9.207f, attackCol1[i].transform.localScale.z);
                     break;
                  case 1: 
                     attackCol1[i].transform.localPosition = new Vector3(attackCol1[i].transform.localPosition.x,
                        attackCol1[i].transform.localPosition.y, -0.95f);
                     attackCol1[i].transform.localScale = new Vector3(attackCol1[i].transform.localScale.x, 9.9f, attackCol1[i].transform.localScale.z);
                     break;
                  case 2: attackCol1[i].transform.localScale = new Vector3(16.5f, 16.5f, attackCol1[i].transform.localScale.z);
                     break;
               }
            }
            for (int i = 0; i < attackCol2.Length; i++)
            {
               switch (i)
               {
                  case 0:
                     attackCol2[i].transform.localPosition = new Vector3(attackCol2[i].transform.localPosition.x,
                        attackCol2[i].transform.localPosition.y,  -0.8f);
                     attackCol2[i].transform.localScale = new Vector3(attackCol2[i].transform.localScale.x, 9.207f, attackCol2[i].transform.localScale.z);
                     break;
                  case 1: 
                     attackCol2[i].transform.localPosition = new Vector3(attackCol2[i].transform.localPosition.x,
                        attackCol2[i].transform.localPosition.y, -0.95f);
                     attackCol2[i].transform.localScale = new Vector3(attackCol2[i].transform.localScale.x, 9.9f, attackCol2[i].transform.localScale.z);
                     break;
                  case 2: attackCol2[i].transform.localScale = new Vector3(16.5f, 16.5f, attackCol2[i].transform.localScale.z);
                     break;
               }
            }
            break;
         // LEVEL 2
         case 2:
             for (int i = 0; i < attackCol1.Length; i++)
            {
               switch (i)
               {
                  case 0:
                     attackCol1[i].transform.localPosition = new Vector3(attackCol1[i].transform.localPosition.x,
                        attackCol1[i].transform.localPosition.y, -0.953f);
                     attackCol1[i].transform.localScale = new Vector3(attackCol1[i].transform.localScale.x, 10.4625f , attackCol1[i].transform.localScale.z);
                     break;
                  case 1: 
                     attackCol1[i].transform.localPosition = new Vector3(attackCol1[i].transform.localPosition.x,
                        attackCol1[i].transform.localPosition.y, -1.01f);
                     attackCol1[i].transform.localScale = new Vector3(attackCol1[i].transform.localScale.x, 11.25f, attackCol1[i].transform.localScale.z);
                     break;
                  case 2: attackCol1[i].transform.localScale = new Vector3(18.75f, 18.75f, attackCol1[i].transform.localScale.z);
                     break;
               }
            }
            for (int i = 0; i < attackCol2.Length; i++)
            {
               switch (i)
               {
                  case 0:
                     attackCol2[i].transform.localPosition = new Vector3(attackCol2[i].transform.localPosition.x,
                        attackCol2[i].transform.localPosition.y, -0.953f);
                     attackCol2[i].transform.localScale = new Vector3(attackCol2[i].transform.localScale.x, 10.4625f , attackCol2[i].transform.localScale.z);
                     break;
                  case 1: 
                     attackCol2[i].transform.localPosition = new Vector3(attackCol2[i].transform.localPosition.x,
                        attackCol2[i].transform.localPosition.y, -1.01f);
                     attackCol2[i].transform.localScale = new Vector3(attackCol2[i].transform.localScale.x, 11.25f, attackCol2[i].transform.localScale.z);
                     break;
                  case 2: attackCol2[i].transform.localScale = new Vector3(18.75f, 18.75f, attackCol2[i].transform.localScale.z);
                     break;
               }
            }
            break;
         // LEVEL 3
         case 3:
             for (int i = 0; i < attackCol1.Length; i++)
            {
               switch (i)
               {
                  case 0:
                     attackCol1[i].transform.localPosition = new Vector3(attackCol1[i].transform.localPosition.x,
                        attackCol1[i].transform.localPosition.y, -1.0323f);
                     attackCol1[i].transform.localScale = new Vector3(attackCol1[i].transform.localScale.x, 12.555f , attackCol1[i].transform.localScale.z);
                     break;
                  case 1: 
                     attackCol1[i].transform.localPosition = new Vector3(attackCol1[i].transform.localPosition.x,
                        attackCol1[i].transform.localPosition.y, -1.19f);
                     attackCol1[i].transform.localScale = new Vector3(attackCol1[i].transform.localScale.x, 13.5f, attackCol1[i].transform.localScale.z);
                     break;
                  case 2: attackCol1[i].transform.localScale = new Vector3(22.5f, 22.5f, attackCol1[i].transform.localScale.z);
                     break;
               }
            }
            for (int i = 0; i < attackCol2.Length; i++)
            {
               switch (i)
               {
                  case 0:
                     attackCol2[i].transform.localPosition = new Vector3(attackCol2[i].transform.localPosition.x,
                        attackCol2[i].transform.localPosition.y, -1.0323f);
                     attackCol2[i].transform.localScale = new Vector3(attackCol2[i].transform.localScale.x, 12.555f , attackCol2[i].transform.localScale.z);
                     break;
                  case 1: 
                     attackCol2[i].transform.localPosition = new Vector3(attackCol2[i].transform.localPosition.x,
                        attackCol2[i].transform.localPosition.y, -1.19f);
                     attackCol2[i].transform.localScale = new Vector3(attackCol2[i].transform.localScale.x, 13.5f, attackCol2[i].transform.localScale.z);
                     break;
                  case 2: attackCol2[i].transform.localScale = new Vector3(22.5f, 22.5f, attackCol2[i].transform.localScale.z);
                     break;
               }
            }
            break;
      } 
   }
   #endregion

   public void UpdateAttackStats()
   {
      for (int i = 0; i < attackCol1.Length; i++)
      {
         AttackControl attackControl = attackCol1[i].GetComponent<AttackControl>();
         attackControl.playerDamage = attackDamageValue;
         attackControl.repusleEnnemyForce = enemyKBForce;
      }

      for (int i = 0; i < attackCol2.Length; i++)
      {
         AttackControl attackControl = attackCol2[i].GetComponent<AttackControl>();
         attackControl.playerDamage = attackDamageValue;
         attackControl.repusleEnnemyForce = enemyKBForce;
      }
      //Debug.Log("AttackStats is at date");
   }
}
