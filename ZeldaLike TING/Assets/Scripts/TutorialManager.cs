using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

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
    [SerializeField] private GameObject eAppearFX;
    private bool enemySpawn = true;
    [SerializeField] private Transform fireCardCamera;
    [SerializeField] private FireCardTutorialManager FireCardTutorialManager;
    [SerializeField] private Animator ithar;
    private bool itharStarted;
    private HelpsManager helpManager;
    [SerializeField] private GivePlayerFireCard givePlayerFireCard = null;
    private bool setHelp = true;
    private bool fireCardCinematic = true;
    [SerializeField] private CameraShakeScriptable prisonShake;
    [SerializeField] private VolumeProfile transitionVolume;
    [SerializeField] private AnimationCurve transitionCurve;
    [SerializeField] private AnimationCurve constantVolumeCurve;
    [SerializeField] private VolumeProfile tensionVolume;
     
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
        helpManager = GetComponent<HelpsManager>();
        Controller.instance.transform.position = spawnPoint.position;
        UIManager.Instance.gameObject.SetActive(false);
        Controller.instance.FreezePlayer(true);
        DialogueManager.Instance.IsCinematic();
        UIManager.Instance.loadingScreen.SetActive(false);
        EnqueueDialogue();
    }

    private void Update()
    {
        Dialogues();
    }

    private void ResetCamera()
    {
        GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
        
    }

    public void EnqueueDialogue()
    {
        DialogueManager.Instance.AssignDialogue(dialogueQueue.Dequeue().dialogue.ToList());
    }

    private IEnumerator ManageIthar()
    {
        yield return new WaitForSeconds(4);
        CameraShake.Instance.AddShakeEvent(prisonShake);
        GameManager.Instance.VolumeTransition(transitionVolume, transitionCurve);
        yield return new WaitForSeconds(3);
        GameManager.Instance.VolumeTransition(tensionVolume, constantVolumeCurve, true);
        ithar.gameObject.SetActive(true);
        ithar.Play("ItharAppear");
        yield return new WaitForSeconds(28f);
        ithar.Play("ItharDisappear");
        yield return new WaitForSeconds(0.9f);
        GameManager.Instance.VolumeTransition(tensionVolume, constantVolumeCurve);
        DialogueManager.Instance.isCursed = true;
        DialogueManager.Instance.mist.SetTrigger("Appear");
        ithar.gameObject.SetActive(false);
    }

    private void Dialogues()
    {
        if (DialogueManager.Instance.isPlayingDialogue == false)
        {
            remainingDialogues = dialogueQueue.Count;
            switch (remainingDialogues)
            {
                case 6 : //Après le tout premier dialogue
                    Controller.instance.FreezePlayer(true);
                    GameManager.Instance.cameraController.ChangePoint(cameraPoint);
                    EnqueueDialogue();
                    Invoke("ResetCamera", 6);
                    break;
                case 5 : //Après avoir vu l'objet magique
                    Controller.instance.FreezePlayer(false);
                    Controller.instance.FreezePlayer(true, "Cards");
                    Controller.instance.FreezePlayer(true, "Attack");
                    UIManager.Instance.gameObject.SetActive(true);
                    if (setHelp)
                    {
                        DialogueManager.Instance.IsCinematic();
                        setHelp = false;
                        helpManager.DisplayHelp();
                    }
                    break;
                case 4 : //Après avoir libéré Ithar
                    if (enemySpawn)
                    {
                        helpManager.DisplayHelp();
                        enemySpawn = false;
                        UIManager.Instance.gameObject.SetActive(true);
                        StartCoroutine(SpawnEnnemiesCo());
                        
                        Controller.instance.FreezePlayer(false);
                        Controller.instance.FreezePlayer(true, "Cards");
                        DialogueManager.Instance.IsCinematic();
                        ResetCamera();
                    }

                    if (ennemyParent.childCount == 0)
                    {
                        EnqueueDialogue();
                    }
                    break;
                case 3 : //Après avoir tué les ennemis
                    if(ennemyParent.childCount == 0) givePlayerFireCard.ActivGetCard();
                    break;
                case 2 : //Après avoir récupéré la carte de feu
                    Controller.instance.FreezePlayer(false);
                    DialogueManager.Instance.IsCinematic();
                    UIManager.Instance.gameObject.SetActive(true);
                    Controller.instance.FreezePlayer(true, "Cards");
                    GameManager.Instance.TutorialWorld();
                    GameManager.Instance.VolumeTransition(GameManager.Instance.tutorialTransition, GameManager.Instance.cardTutorialCurve);
                    EnqueueDialogue();
                    break;
                case 1 : //Une fois l'intro du monde tutoriel finie
                    FireCardTutorialManager.canStart = true;
                    if (FireCardTutorialManager.isFinished)
                    {
                        GameManager.Instance.volumeManager.enabled = false;
                        EnqueueDialogue();
                    }
                    break;
                case 0 : //Une fois le deal passé
                    Controller.instance.FreezePlayer(false);
                    Debug.Log("J'ai capté que le dialogue était fini");
                    if (GameManager.Instance.isTutorial && DialogueManager.Instance.isCinematic)
                    {
                        GameManager.Instance.isTutorial = false;
                        DialogueManager.Instance.IsCinematic();
                    }
                    
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
                    Controller.instance.FreezePlayer(true);
                    if (!itharStarted)
                    {
                        DialogueManager.Instance.IsCinematic();
                        itharStarted = true;
                        StartCoroutine(ManageIthar());
                    }
                    
                    break;
                case 3 :
                    Controller.instance.FreezePlayer(true);
                    if (fireCardCinematic)
                    {
                        fireCardCinematic = false;
                        DialogueManager.Instance.IsCinematic();
                    }
                    
                    UIManager.Instance.gameObject.SetActive(false);
                    break;
            }
        }
    }

    private IEnumerator SpawnEnnemiesCo()
    {
        Destroy(Instantiate(eAppearFX, ennemiesSpawnPoints[1].position + new Vector3(0,3.3f, 0), Quaternion.identity, ennemyParent), 5f);
        Destroy(Instantiate(eAppearFX, ennemiesSpawnPoints[2].position + new Vector3(0,3.3f, 0), Quaternion.identity, ennemyParent), 5f);
        
        yield return new WaitForSeconds(3.5f);
        
        Instantiate(ennemies[0], ennemiesSpawnPoints[1].position, Quaternion.identity, ennemyParent);
        Instantiate(ennemies[0], ennemiesSpawnPoints[2].position, Quaternion.identity, ennemyParent);
    }
    
}
