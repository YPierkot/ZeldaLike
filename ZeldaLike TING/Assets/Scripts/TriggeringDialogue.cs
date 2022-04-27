using System.Linq;
using UnityEngine;

public class TriggeringDialogue : MonoBehaviour
{
    [SerializeField] private DialogueScriptable dialogue;
    private bool hasGivenDialogue;
    public bool isTutorial;
    [SerializeField] private TutorialManager tutorialManager;
    public bool isCinematic;
    [SerializeField] private bool enemiesIsCondition;
    [SerializeField] private Transform enemiesParent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasGivenDialogue && !isTutorial && !enemiesIsCondition)
        {
            hasGivenDialogue = true;
            DialogueManager.Instance.AssignDialogue(dialogue.dialogue.ToList());
        }
        else if (other.CompareTag("Player") && !hasGivenDialogue && isTutorial && !enemiesIsCondition)
        {
            hasGivenDialogue = true;
            tutorialManager.EnqueueDialogue();
            if (isCinematic)
            {
                Controller.instance.canMove = false;
            }
        }
        else if(other.CompareTag("Player") && !hasGivenDialogue && isTutorial && enemiesIsCondition)
        {
            if (enemiesParent.childCount == 0)
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
}
