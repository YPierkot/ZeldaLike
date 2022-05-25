using System;
using System.Collections.Generic;
using System.Collections;
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
    
    [SerializeField] private GameObject puzzle;

    [SerializeField] private bool puzzleFinished;

    [Header("Second Challenge")] 
    
    [SerializeField] private GameObject mannequin;
    [SerializeField] private GameObject brasier;
    [SerializeField] private GameObject fakeBrasier;
    [SerializeField] private bool isDead;
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
                case 5 :
                    barrier.SetActive(true);
                    DialogueManager.Instance.AssignDialogue(dialogueQueue.Dequeue().dialogue.ToList());
                    DialogueManager.Instance.IsCinematic();
                    StartCoroutine(DialogueManager.Instance.CinematicWait(14));
                    Controller.instance.FreezePlayer(true);
                    break;
                
                case 4:
                    if (!DialogueManager.Instance.isPlayingDialogue)
                    {
                        Controller.instance.FreezePlayer(false);
                        DialogueManager.Instance.AssignDialogue(dialogueQueue.Dequeue().dialogue.ToList());
                        GameManager.Instance.TutorialWorld();
                        GameManager.Instance.VolumeTransition(GameManager.Instance.tutorialTransition, GameManager.Instance.cardTutorialCurve);
                        puzzle.SetActive(true);
                    }
                    break;
                
                case 3 :
                    if (puzzleFinished)
                    {
                        puzzleFinished = false;
                        StartCoroutine(GameManager.Instance.DisableObject(puzzle));
                        DialogueManager.Instance.AssignDialogue(dialogueQueue.Dequeue().dialogue.ToList());
                        StartCoroutine(MannequinDelay());
                    }
                    
                    break;
                case 2 :
                    if (isDead)
                    {
                        DialogueManager.Instance.AssignDialogue(dialogueQueue.Dequeue().dialogue.ToList());
                        isDead = false;
                    }

                    break;
                case 1 :
                    if (!spawnedEnemies && !DialogueManager.Instance.isPlayingDialogue)
                    {
                        spawnedEnemies = true;
                        spawner.SpawnEnemies();
                    }
                    if (spawner.enemiesParent.childCount == 3)
                    {
                        GameManager.Instance.volumeManager.profile = forestProfile;
                        DialogueManager.Instance.AssignDialogue(dialogueQueue.Dequeue().dialogue.ToList());
                        StartCoroutine(GameManager.Instance.DisableObject(barrier));
                    }
                    break;
                case 0 : 
                    if (!isFinished)
                    {
                        isFinished = true;
                        canStart = false;
                    }
                    break;
            }
            
        }
    }

    private IEnumerator MannequinDelay()
    {
        yield return new WaitForSeconds(6f);
        mannequin.SetActive(true);
        brasier.SetActive(true);
    }

    public void FinishPuzzle()
    {
        puzzleFinished = true;
    }

    public void MannequinKilled()
    {
        StartCoroutine(MannequinDeathWait());
        isDead = true;
    }

    private IEnumerator MannequinDeathWait()
    {
        yield return new WaitForSeconds(1.5f);
        fakeBrasier.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        mannequin.SetActive(false);
        brasier.SetActive(false);
        yield return new WaitForSeconds(3f);
        fakeBrasier.SetActive(false);
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
