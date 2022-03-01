using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{

    [SerializeField] private DialogueScriptable dialogue;
    private bool playerIn;
    private void OnTriggerEnter(Collider other)
    {
        playerIn = true;
        Debug.Log("je suis dedans");
    }

    private void OnTriggerExit(Collider other)
    {
        playerIn = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("ca marche wsh");
            DialogueManager.Instance.AssignDialogue(dialogue.dialogue);
        }
    }
}
