using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
   #region INSTANCE
      public static UIManager Instance;

      private void Awake()
      {
         Instance = this;
      }
   #endregion

   [Header("--- LIFE & STAT")] 
   [SerializeField] private Image[] lifeArray;

   public void UpdateLife(int life)
   {
      for (int i = 0; i < lifeArray.Length; i++)
      {
         if (i >= life)
         {
            lifeArray[i].enabled = false;
         }
      }
   }
   
}
