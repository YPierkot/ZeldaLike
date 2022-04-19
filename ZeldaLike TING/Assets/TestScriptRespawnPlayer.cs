using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptRespawnPlayer : MonoBehaviour
{
    public Transform respawnPoint;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = respawnPoint.position;
        }
    }
}
