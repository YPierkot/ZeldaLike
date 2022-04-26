using System.Linq;
using UnityEngine;

public class TriggeringDialogue : MonoBehaviour
{
    [SerializeField] private DialogueScriptable dialogue;
    private bool hasGivenDialogue;
    public bool isTutorial;
    [SerializeField] private TutorialManager tutorialManager;
    public bool isCinematic;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasGivenDialogue && !isTutorial)
        {
            hasGivenDialogue = true;
            DialogueManager.Instance.AssignDialogue(dialogue.dialogue.ToList());
        }
        else if (other.CompareTag("Player") && !hasGivenDialogue && isTutorial)
        {
            hasGivenDialogue = true;
            tutorialManager.EnqueueDialogue();
            if (isCinematic)
            {
                Controller.instance.canMove = false;
            }
        }
    }
}
