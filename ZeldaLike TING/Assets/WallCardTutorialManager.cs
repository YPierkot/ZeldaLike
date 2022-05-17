using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallCardTutorialManager : MonoBehaviour
{
    
    [Header("Dialogues and helps")]
    
    private Queue<DialogueScriptable> dialogueQueue;
    [SerializeField] private List<DialogueScriptable> dialogues;
    [SerializeField] private HelpsManager HelpsManager;
    
    [Header("State Bools")]
    
    public bool canStart;
    private bool lianaSet;
    public bool isFinished;
    private bool spawnedEnemies;
    private bool canSpawnLianas;
    
    [Header("First Challenge")]
    
    
    [Header("Second Challenge")]
    
    
    [Header("Third Challenge")]
   
    
    [Header("Blocking Player")]
    
    [SerializeField] private GameObject barrier;
    [SerializeField] private GameObject pasStairBarrier = null;
    [SerializeField] private GameObject platformBarrier;


    private void Start()
    {
        dialogueQueue = new Queue<DialogueScriptable>();
        foreach (DialogueScriptable dialogue in dialogues)
        {
            dialogueQueue.Enqueue(dialogue);
        }
        
    }

    private void Update()
    {
        if (canStart && !DialogueManager.Instance.isPlayingDialogue)
        {
            PlayerStat.instance.life = PlayerStat.instance.lifeMax;
            barrier.SetActive(true);
            int remainingDialogue = dialogueQueue.Count;
            switch (remainingDialogue)
            {
                case 3:
                    DialogueManager.Instance.AssignDialogue(dialogueQueue.Dequeue().dialogue.ToList());
                    break;
                case 2 :
                    
                    break;
                case 1 :
                    
                    break;
                
                case 0 :
                    
                    break;

                    
            }
            
        }
    }
}
