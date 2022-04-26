using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevetorPlatform : MonoBehaviour
{
   private Elevetor elevator;

   private void Start()
   {
      elevator = GetComponentInParent<Elevetor>();
   }
   
   
   private void OnTriggerEnter(Collider other)
   {
      elevator.eleveteList.Add(other.transform);
   }

   private void OnTriggerExit(Collider other)
   {
      elevator.eleveteList.Remove(other.transform);
   }
}
