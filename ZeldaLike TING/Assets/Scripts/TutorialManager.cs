using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    [SerializeField] private Transform fireCardCamera;
    [SerializeField] private FireCardTutorialManager FireCardTutorialManager;
    [SerializeField] private Animator ithar;
    private bool itharStarted;
    [SerializeField] private TextMeshProUGUI helpText;
    [TextArea(4, 10)]
    [SerializeField] private List<string> helps;


    [SerializeField] private GivePlayerFireCard givePlayerFireCard = null;
    
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
        UIManager.Instance.gameObject.SetActive(false);
        Controller.instance.canMove = false;
        EnqueueDialogue();
    }

    private void Update()
    {
        if (DialogueManager.Instance.isPlayingDialogue == false)
        {
            remainingDialogues = dialogueQueue.Count;
            switch (remainingDialogues)
            {
                case 6 :
                    Controller.instance.canMove = true;
                    GameManager.Instance.cameraController.ChangePoint(cameraPoint);
                    EnqueueDialogue();
                    Invoke("ResetCamera", 6);
                    break;
                case 5 :
                    break;
                case 4 :
                    if (enemySpawn)
                    { 
                        helpText.text = helps[1] + " " + helps[2];

                        enemySpawn = false;
                        UIManager.Instance.gameObject.SetActive(true);
                        Instantiate(ennemies[1], ennemiesSpawnPoints[0].position, Quaternion.identity, ennemyParent);
                        Instantiate(ennemies[0], ennemiesSpawnPoints[1].position, Quaternion.identity, ennemyParent);
                        Instantiate(ennemies[0], ennemiesSpawnPoints[2].position, Quaternion.identity, ennemyParent);
                        Controller.instance.canMove = true;
                        ResetCamera();
                    }

                    if (ennemyParent.childCount == 0)
                    {
                        EnqueueDialogue();
                    }
                    break;
                case 3 :
                    if(ennemyParent.childCount == 0) givePlayerFireCard.ActivGetCard();
                    break;
                case 2 :
                    Controller.instance.canMove = true;
                    UIManager.Instance.gameObject.SetActive(true);
                    GameManager.Instance.TutorialWorld();
                    EnqueueDialogue();
                    break;
                case 1 :
                    FireCardTutorialManager.canStart = true;
                    if (FireCardTutorialManager.isFinished)
                    {
                        GameManager.Instance.volumeManager.enabled = false;
                        EnqueueDialogue();
                    }
                    break;
                case 0 :
                    Controller.instance.canMove = true;
                    UIManager.Instance.gameObject.SetActive(true);
                    break;
                
            }
        }

        else
        {
            switch (remainingDialogues)
            {
                case 5:
                    GameManager.Instance.cameraController.ChangePoint(cameraPoint);
                    UIManager.Instance.gameObject.SetActive(false);
                    Controller.instance.ForceMove(prisonPosition.position);
                    if (!itharStarted)
                    {
                        itharStarted = true;
                        StartCoroutine(ManageIthar());
                    }
                    
                    break;
                case 3 :
                    Controller.instance.canMove = false;
                    UIManager.Instance.gameObject.SetActive(false);
                    break;
            }
        }
    }

    private void ResetCamera()
    {
        GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
        UIManager.Instance.gameObject.SetActive(true);
        helpText.text = helps[0];
    }

    public void EnqueueDialogue()
    {
        Debug.Log("Je lance le prochain dialogue du tutoriel");
        DialogueManager.Instance.AssignDialogue(dialogueQueue.Dequeue().dialogue.ToList());
    }

    private IEnumerator ManageIthar()
    {
        yield return new WaitForSeconds(8);
        ithar.gameObject.SetActive(true);
        ithar.Play("ItharAppear");
        yield return new WaitForSeconds(28f);
        ithar.Play("ItharDisappear");
        yield return new WaitForSeconds(0.9f);
        DialogueManager.Instance.isCursed = true;
        DialogueManager.Instance.mist.SetTrigger("Appear");
        ithar.gameObject.SetActive(false);
    }
    
}
