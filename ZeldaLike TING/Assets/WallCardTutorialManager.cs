using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class WallCardTutorialManager : MonoBehaviour
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
    
    [SerializeField] private Transform objective;
    [SerializeField] private GameObject chest;

    [Header("Second Challenge")] 
    
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
            chest.SetActive(false);
            PlayerStat.instance.life = PlayerStat.instance.lifeMax;
            int remainingDialogue = dialogueQueue.Count;
            switch (remainingDialogue)
            {
                case 3 :
                    DialogueManager.Instance.AssignDialogue(dialogueQueue.Dequeue().dialogue.ToList());
                    break;
                case 2:
                    if (CardsController.instance.wallCardUnlock)
                    {
                        DialogueManager.Instance.AssignDialogue(dialogueQueue.Dequeue().dialogue.ToList());
                        GameManager.Instance.TutorialWorld();
                        objective.gameObject.SetActive(true);
                        barrier.SetActive(true);
                        GameManager.Instance.VolumeTransition(GameManager.Instance.tutorialTransition, GameManager.Instance.cardTutorialCurve);
                    }
                    break;
                case 1 :
                    if (Vector3.Distance(objective.position, Controller.instance.transform.position) <= 3)
                    {
                        StartCoroutine(GameManager.Instance.DisableObject(objective.gameObject));
                        DialogueManager.Instance.AssignDialogue(dialogueQueue.Dequeue().dialogue.ToList());
                    }
                    
                    break;
                case 0 :
                    if (!spawnedEnemies)
                    {
                        spawner.SpawnEnemies();
                        spawnedEnemies = true;
                        
                    }
                    if (spawner.enemiesParent.childCount == 3)
                    {
                        GameManager.Instance.volumeManager.profile = forestProfile;
                        isFinished = true;
                        canStart = false;
                        StartCoroutine(GameManager.Instance.DisableObject(barrier));
                        chest.SetActive(true);
                        gameObject.SetActive(false);
                    }
                    break;
            }
            
        }
    }

}
