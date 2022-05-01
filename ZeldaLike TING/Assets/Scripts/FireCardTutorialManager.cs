using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireCardTutorialManager : MonoBehaviour
{
    private Queue<DialogueScriptable> dialogueQueue;
    [SerializeField] private List<DialogueScriptable> dialogues;
    public bool canStart;
    [SerializeField] private GameObject lianas;
    private bool lianaSet;
    [SerializeField] private GameObject puzzle;
    [SerializeField] private EnemySpawnTrigger enemySpawner;
    [SerializeField] private GameObject barrier;
    [SerializeField] private GameObject pasStairBarrier = null;
    [SerializeField] private GameObject liana;
    public bool isFinished;
    private bool spawnedEnemies;
    [SerializeField] private HelpsManager HelpsManager;
    [SerializeField] private Transform[] lianaSpawnPoints;
    private float lastLianaSpawn;
    [SerializeField] private float lianaSpawnDelay;
    private bool canSpawnLianas;
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
                    switch (lianaSet)
                    {
                        case false:
                            lianaSet = true;
                            CardsController.instance.canUseCards = true;
                            lianas.transform.position = Controller.instance.transform.position;
                            HelpsManager.DisplayHelp();
                            break;
                    }
                    lianas.SetActive(true);
                    if (lianas.transform.childCount == 0)
                    {
                        lastLianaSpawn = Time.time;
                        canSpawnLianas = true;
                        DialogueManager.Instance.AssignDialogue(dialogueQueue.Dequeue().dialogue.ToList());
                    }
                    break;
                case 1 :
                    puzzle.SetActive(true);
                    if (puzzle.transform.childCount == 4)
                    {
                        HelpsManager.DisplayHelp();
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
                    }
                    break;

                    
            }
            if (Time.time > lastLianaSpawn + lianaSpawnDelay && canSpawnLianas && !isFinished)
            {
                Instantiate(liana, lianaSpawnPoints[UnityEngine.Random.Range(0, lianaSpawnPoints.Length)]);
                lastLianaSpawn = Time.time;
            }
        }
    }
}
