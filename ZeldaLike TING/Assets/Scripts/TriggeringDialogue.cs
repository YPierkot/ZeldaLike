using System;
using UnityEngine;

public class TriggeringDialogue : MonoBehaviour
{
    [SerializeField] private DialogueScriptable dialogue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager.Instance.AssignDialogue(dialogue.dialogue);
        }
    }
}
