using System.Linq;
using UnityEngine;

public class NPCDialogue : MonoBehaviour{
    public System.Collections.Generic.List<DialogueScriptable> dialogues;
    [SerializeField] private DialogueScriptable[] fillingDialogues;
    public bool playerIn;
    [SerializeField] private int currentDialogue = 0;
    [SerializeField] private DialogueScriptable testDialogue;

    private void Start()
    {
        if (testDialogue!= null)
        {
            AssignDialogue(testDialogue);
        }
    }

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
            if (currentDialogue > dialogues.Count || dialogues.Count == 0)
            {
                Debug.Log("Ya pas de dialogues");
                AssignDialogue(fillingDialogues[UnityEngine.Random.Range(0, fillingDialogues.Length)]);
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
        DialogueManager.Instance.AssignDialogue(dialogueToAdd.dialogue.ToList());
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmos() {
        if (!CustomLDData.showGizmos || !CustomLDData.showGizmosDialogue) return;
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, GetComponent<SphereCollider>().radius);
    }
    #endif
}
