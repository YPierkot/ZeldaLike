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
      script.Teleport();
   }

   public void FinishTP()
   {
      script.EndTeleport();
   }

   public void EndLaserCast()
   {
      script.castingLaser = false;
   }

   public void EndIdle()
   {
      script.EndIdle();
   }

}
