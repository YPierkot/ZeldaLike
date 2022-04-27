using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private List<DialogueScriptable> tutorialDialogues;
    private Queue<DialogueScriptable> dialogueQueue;
    private int remainingDialogues;
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private Transform prisonPosition;
    [SerializeField] private List<Transform> ennemiesSpawnPoints;
    [SerializeField] private Transform ennemyParent;
    [SerializeField] private GameObject[] ennemies = new GameObject[2];
    private bool enemySpawn = true;

    private void Awake()
    {
        dialogueQueue = new Queue<DialogueScriptable>();
        foreach (DialogueScriptable dialogue in tutorialDialogues)
        {
            dialogueQueue.Enqueue(dialogue);
        }
    }

    private void Start()
    {
        Controller.instance.transform.position = spawnPoint.position;
        Controller.instance.canMove = false;
        EnqueueDialogue();
    }

    private void Update()
    {
        if (DialogueManager.Instance.isPlayingDialogue == false)
        {
            remainingDialogues = dialogueQueue.Count;
            Debug.Log(remainingDialogues);
            switch (remainingDialogues)
            {
                case 6 : 
                    Controller.instance.canMove = true;
                    GameManager.Instance.cameraController.ChangePoint(cameraPoint);
                    EnqueueDialogue();
                    Invoke("ResetCamera", 4);
                    break;
                case 5 :
                    break;
                case 4 :
                    if (enemySpawn)
                    {
                        enemySpawn = false;
                        Instantiate(ennemies[1], ennemiesSpawnPoints[0].position, Quaternion.identity, ennemyParent);
                        Instantiate(ennemies[0], ennemiesSpawnPoints[1].position, Quaternion.identity, ennemyParent);
                        Instantiate(ennemies[0], ennemiesSpawnPoints[2].position, Quaternion.identity, ennemyParent);
                        Controller.instance.canMove = true;
                        ResetCamera();
                    }

                    if (ennemyParent.childCount == 0)
                    {
                        EnqueueDialogue();
                        Controller.instance.canMove = false;
                    }
                    break;
                case 3 :
                    Controller.instance.canMove = true;
                    break;
                case 2 : 
                    break;
                case 1 : 
                    break;
                case 0 : 
                    break;
                
            }
        }

        else
        {
            switch (remainingDialogues)
            {
                case 5:
                    GameManager.Instance.cameraController.ChangePoint(cameraPoint);
                    Controller.instance.ForceMove(prisonPosition.position);
                    break;
            }
        }
        
        
    }

    private void ResetCamera()
    {
        GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
    }

    public void EnqueueDialogue()
    {
        Debug.Log("Je lance le prochain dialogue du tutoriel");
        DialogueManager.Instance.AssignDialogue(dialogueQueue.Dequeue().dialogue.ToList());
    }
}
