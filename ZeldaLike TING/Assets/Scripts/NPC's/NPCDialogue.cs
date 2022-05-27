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
            Controller.instance.playerInteraction = Interact;
            Debug.Log("You can interact");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Controller.instance.playerInteraction == Interact) Controller.instance.playerInteraction = null;
            Debug.Log("You can't interact anymore");
        }
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.E) && playerIn)
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
        }*/
    }

    void Interact()
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

    private void AssignDialogue(DialogueScriptable dialogueToAdd)
    {
        if (!DialogueManager.Instance.isPlayingDialogue)
        {
            DialogueManager.Instance.AssignDialogue(dialogueToAdd.dialogue.ToList());
        }
    }
    
    /// <summary>
    /// Assign a random dialogue
    /// </summary>
    public void AssignRandomDialogue() => AssignDialogue(fillingDialogues[UnityEngine.Random.Range(0, fillingDialogues.Length)]);
    
    #if UNITY_EDITOR
    private void OnDrawGizmos() {
        if (!CustomLDData.showGizmos || !CustomLDData.showGizmosDialogue) return;
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, GetComponent<SphereCollider>().radius);
    }
    #endif
}
