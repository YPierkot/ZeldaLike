using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpike : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
       if(collision.transform.CompareTag("Player"))  PlayerStat.instance.TakeDamage();
    }
}
