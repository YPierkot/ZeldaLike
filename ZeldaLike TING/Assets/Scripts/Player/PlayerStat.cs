using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
   [Header("LIFE")]
   [SerializeField] private int lifeMax;
   [SerializeField] private int life;

   [Header("Debug")] [SerializeField] private float repulseForce = 1;

   private Controller control;

   private void Start()
   {
      control = GetComponent<Controller>();
      
      life = lifeMax;
      UIManager.Instance.UpdateLife(life);
   }
   
   void TakeDamage(int damage = 1)
   {
      life -= damage;
      UIManager.Instance.UpdateLife(life);
   }

   private void OnCollisionEnter(Collision other)
   {
      Debug.Log("Collide");
      if (other.transform.CompareTag("Ennemy")) 
      {
         Debug.Log("Ennemy");
         TakeDamage();
         Vector3 repulse = (transform.position - other.contacts[0].point).normalized*repulseForce;
         control.rb.velocity = repulse;
      }
   }
}
