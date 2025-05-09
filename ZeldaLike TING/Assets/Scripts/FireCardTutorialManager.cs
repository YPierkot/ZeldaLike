using System.Collections;
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
    [SerializeField] private Material puzzleEM;
    [SerializeField] private MeshRenderer puzzleMesh;
    
    [Header("Third Challenge")]
    [SerializeField] private EnemySpawnTrigger enemySpawner;

    [SerializeField] private TeleportationPortal portal;
    [SerializeField] private Transform portalCameraPoint;
    
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
            //Debug.Log("Je commence le tutoriel du feu");
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
                            lianas.SetActive(true);
                            break;
                    }
                    if (lianas.transform.childCount == 0)
                    {
                        DialogueManager.Instance.AssignDialogue(dialogueQueue.Dequeue().dialogue.ToList());
                    }
                    break;
                case 1 :
                    if (!puzzle.activeSelf)
                    {
                        puzzle.SetActive(true);
                        StartCoroutine(HelpsManager.DisplayHelp());
                        StartCoroutine(DelayMat());
                    }
                    if (puzzle.transform.childCount == 5)
                    {
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
                        portal.GetComponent<SphereCollider>().enabled = true;
                        GameManager.Instance.cameraController.ChangePoint(portalCameraPoint);
                        gameObject.SetActive(false);
                    }
                    break;
            }
        }
    }

    private IEnumerator DelayMat()
    {
        yield return new WaitForSeconds(2f);
        puzzleMesh.material = puzzleEM;
    }
}
