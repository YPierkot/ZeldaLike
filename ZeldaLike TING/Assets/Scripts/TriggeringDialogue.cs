using System.Linq;
using UnityEngine;

public class TriggeringDialogue : MonoBehaviour
{
    [SerializeField] private DialogueScriptable dialogue;
    private bool hasGivenDialogue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasGivenDialogue)
        {
            hasGivenDialogue = true;
            Debug.Log(DialogueManager.Instance);
            DialogueManager.Instance.AssignDialogue(dialogue.dialogue.ToList());
        }
    }
}
