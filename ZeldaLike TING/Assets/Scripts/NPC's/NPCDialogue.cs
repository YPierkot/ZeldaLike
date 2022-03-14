using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;

public class NPCDialogue : MonoBehaviour
{
    public List<DialogueScriptable> dialogues;
    [SerializeField] private DialogueScriptable[] fillingDialogues;
    private bool playerIn;
    [SerializeField] private int currentDialogue = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIn = true;
            Debug.Log("You can interact");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIn = false;
            Debug.Log("You can't interact anymore");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIn)
        {
            if (currentDialogue > dialogues.Count || dialogues == null)
            {
                AssignDialogue(fillingDialogues[Range(0, fillingDialogues.Length)]);
            }
            else
            {
                AssignDialogue(dialogues[currentDialogue]);
                currentDialogue++;
            }
        }
    }

    private void AssignDialogue(DialogueScriptable dialogueToAdd)
    {
        DialogueManager.Instance.AssignDialogue(dialogueToAdd.dialogue);
    }
}
