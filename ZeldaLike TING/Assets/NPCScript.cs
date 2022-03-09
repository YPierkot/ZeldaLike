using UnityEngine;

public class NPCScript : MonoBehaviour
{

    [SerializeField] private DialogueScriptable dialogue;
    private bool playerIn;
    private void OnTriggerEnter(Collider other)
    {
        playerIn = true;
    }

    private void OnTriggerExit(Collider other)
    {
        playerIn = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            DialogueManager.Instance.AssignDialogue(dialogue.dialogue);
        }
    }
}
