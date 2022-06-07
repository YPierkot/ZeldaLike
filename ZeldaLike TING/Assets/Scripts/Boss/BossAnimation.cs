using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimation : MonoBehaviour
{
   private BossManager script;

   private void Start()
   {
      script = GetComponentInParent<BossManager>();
   }

   public void ChangePosition()
   {
      if (!DialogueManager.Instance.isCinematic)
      {
         script.Teleport();
      }
   }

   public void FinishTP()
   {
      if (!DialogueManager.Instance.isCinematic)
      {
         script.EndTeleport();
      }
   }

   public void EndLaserCast()
   {
      if (!DialogueManager.Instance.isCinematic) script.castingLaser = false;
   }

   public void EndIdle()
   {
      if (!DialogueManager.Instance.isCinematic) script.EndIdle();
   }

}
