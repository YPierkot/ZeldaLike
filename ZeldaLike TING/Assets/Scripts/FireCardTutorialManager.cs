using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireCardTutorialManager : MonoBehaviour
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
    
    [SerializeField] private GameObject lianas;
    private float lastLianaSpawn;

    [Header("Second Challenge")]
    
    [SerializeField] private GameObject puzzle;
    
    [Header("Third Challenge")]
    [SerializeField] private EnemySpawnTrigger enemySpawner;

    [SerializeField] private Animator portal;
    
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
            Debug.Log("Je commence le tutoriel du feu");
            PlayerStat.instance.life = PlayerStat.instance.lifeMax;
            barrier.SetActive(true);
            int remainingDialogue = dialogueQueue.Count;
            switch (remainingDialogue)
            {
                case 3:
                    DialogueManager.Instance.AssignDialogue(dialogueQueue.Dequeue().dialogue.ToList());
                    break;
                case 2 :
                    switch (lianaSet)
                    {
                        case false:
                            lianaSet = true;
                            CardsController.instance.canUseCards = true;
                            StartCoroutine(HelpsManager.DisplayHelp());
                            break;
                    }
                    lianas.SetActive(true);
                    break;
                case 1 :
                    puzzle.SetActive(true);
                    if (puzzle.transform.childCount == 4)
                    {
                        StartCoroutine(HelpsManager.DisplayHelp());
                        puzzle.SetActive(false);
                        DialogueManager.Instance.AssignDialogue(dialogueQueue.Dequeue().dialogue.ToList());
                    }
                    break;
                
                case 0 :
                    if (!spawnedEnemies)
                    {
                        enemySpawner.SpawnEnemies();
                        spawnedEnemies = true;
                    }
                    
                    if (enemySpawner.enemiesParent.childCount == 0)
                    {
                        canStart = false;
                        isFinished = true;
                        pasStairBarrier.SetActive(false);
                        platformBarrier.SetActive(false);
                        portal.SetTrigger("PortalOn");
                    }
                    break;

                    
            }
        }
    }
}
