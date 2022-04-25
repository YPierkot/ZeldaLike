using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeverDestroy : MonoBehaviour
{
   public static NeverDestroy Instance;

   private void Awake()
   {
      if (Instance == null) Instance = this;
      else Destroy(this);
      
      DontDestroyOnLoad(gameObject);
   }

   public Spawnpoint currentSpawnpoint;
   private Vector3 currentSpawnpointPos;
   
   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.K))
      {
         Spawn();
      }
   }

   public void Spawn()
   {
      //Controller.instance.transform.position = currentSpawnpoint.transform.position;
      Controller.instance.transform.position = currentSpawnpointPos;
   }
   
   public void SetSpawnPoint(Spawnpoint spawner)
   {
      Debug.Log($"Set Spawn at {transform.position}, {transform.name}");
      //if(currentSpawnpoint != null) DontDestroyOnLoad();
      currentSpawnpoint = spawner;
      currentSpawnpointPos = currentSpawnpoint.transform.position;
   }



   

}
