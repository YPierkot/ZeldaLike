using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocatePlayer : MonoBehaviour
{
    public string location;

    private void OnTriggerEnter(Collider other)
    {
        DialogueManager.Instance.playerLocation = location;
    }
}
