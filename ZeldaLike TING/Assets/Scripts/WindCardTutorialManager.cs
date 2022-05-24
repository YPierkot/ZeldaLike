using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class WindCardTutorialManager : MonoBehaviour
{
    
    [Header("Dialogues and helps")]
    
    private Queue<DialogueScriptable> dialogueQueue;
    [SerializeField] private List<DialogueScriptable> dialogues;
    [SerializeField] private VolumeProfile forestProfile;
    
    
    [Header("State Bools")]
    
    public bool canStart;
    public bool isFinished;
    private bool spawnedEnemies;

    [Header("First Challenge")] 
    
    [SerializeField] private GameObject[] debris;

    [Header("Second Challenge")] 
    
    [SerializeField] private GameObject[] crystalShards;
    
    [Header("Third Challenge")]
    
    [SerializeField] private EnemySpawnTrigger spawner;

    [Header("Blocking Player")] 
    
    [SerializeField] private GameObject barrier;
    
    

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
            int remainingDialogue = dialogueQueue.Count;
            switch (remainingDialogue)
            {
                case 4:
                    DialogueManager.Instance.AssignDialogue(dialogueQueue.Dequeue().dialogue.ToList());
                    GameManager.Instance.TutorialWorld();
                    GameManager.Instance.VolumeTransition(GameManager.Instance.tutorialTransition, GameManager.Instance.cardTutorialCurve);
                    foreach (var debri in debris)
                    {
                        debri.SetActive(true);
                    }
                    break;
                
                case 3 :
                    /*if (Les débris sont bougés)
                    {
                        DialogueManager.Instance.AssignDialogue(dialogueQueue.Dequeue().dialogue.ToList());
                        foreach (var shard in crystalShards)
                        {
                            shard.SetActive(true);
                        }
                    }*/
                    
                    
                    break;
                case 2 :
                    
                    spawner.SpawnEnemies();
                    if (spawner.enemiesParent.childCount == 0)
                    {
                        DialogueManager.Instance.AssignDialogue(dialogueQueue.Dequeue().dialogue.ToList());
                    }
                    
                    break;
                case 1 :
                    if (!isFinished)
                    {
                        isFinished = true;
                        GameManager.Instance.volumeManager.profile = forestProfile;
                    }
                    break;
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFinished)
        {
            barrier.SetActive(true);
            canStart = true;
            GameManager.Instance.actualRespawnPoint = transform;
        }
    }
    
}
