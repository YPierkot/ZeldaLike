using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PathCreation.Examples;
using UnityEngine;
using UnityEngine.Rendering;

public class TutorialManager : MonoBehaviour
{
    [Header("Start Tutorial")]
    
    [SerializeField] private Transform spawnPoint;
    
    [Header("Dialogues & Helps")]
    
    [SerializeField] private List<DialogueScriptable> tutorialDialogues;
    private Queue<DialogueScriptable> dialogueQueue;
    private HelpsManager helpManager;
    private int remainingDialogues;
    private bool setHelp = true;

    [Header("First Cinematic")] 
    [SerializeField] private Animator portal;

    [SerializeField] private Animator portalDarkCircle;
    [SerializeField] private GameObject teleportShader;
    [SerializeField] private CameraShakeScriptable appearCameraShake;

    private bool playerAppeared;
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private Transform prisonPosition;
    [SerializeField] private Transform prison;
    [SerializeField] private List<Transform> ennemiesSpawnPoints;
    [SerializeField] private Animator ithar;
    private bool touchedPrison;
    private bool itharStarted;
    [SerializeField] private GameObject enemyBreach;
    [SerializeField] private PathFollower prisonParticle;
    [SerializeField] private CameraShakeScriptable breachCameraShake;
    [SerializeField] private CameraShakeScriptable enemiesCameraShake;
    [SerializeField] private Animator mistMovement;
    
    [Header("Ennemies")]
    [SerializeField] private Transform ennemyParent;
    [SerializeField] private GameObject[] ennemies = new GameObject[2];
    [SerializeField] private GameObject eAppearFX;
    private bool enemySpawn = true;

    [Header("Fire Card Tutorial")]
    [SerializeField] private FireCardTutorialManager FireCardTutorialManager;
    [SerializeField] private GivePlayerCard givePlayerFireCard = null;
    private bool fireCardCinematic = true;
    
    [Header("Volumes & Feedbacks")]
    
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
        mistMovement = GameManager.Instance.mistMovement;
        helpManager = GetComponent<HelpsManager>();
        GameManager.Instance.DungeonProfile();
        StartCoroutine(PortalAppearance());
        UIManager.Instance.gameObject.SetActive(false);
        Controller.instance.gameObject.SetActive(false);
        SoundManager.Instance.musicSource.clip = SoundManager.Instance.dungeonMusic;
        SoundManager.Instance.musicSource.Play();
        DialogueManager.Instance.IsCinematic(true);
        Controller.instance.transform.position = spawnPoint.position;
        GameManager.Instance.cameraController.transform.position = Controller.instance.PlayerCameraPoint.position;
        UIManager.Instance.loadingScreen.SetActive(false);
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
        Controller.instance.animatorPlayerHand.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        Controller.instance.animatorPlayer.SetBool("isRun", false);
        yield return new WaitForSeconds(3);
        CameraShake.Instance.AddShakeEvent(prisonShake);
        touchedPrison = true;
        prisonParticle.enabled = true;
        GameManager.Instance.VolumeTransition(transitionVolume, transitionCurve);
        yield return new WaitForSeconds(3);
        GameManager.Instance.VolumeTransition(tensionVolume, constantVolumeCurve, true);
        ithar.gameObject.SetActive(true);
        ithar.Play("ItharAppear");
        prisonParticle.gameObject.SetActive(false);
        yield return new WaitForSeconds(11.5f);
        ithar.Play("ItharPower");
        CameraShake.Instance.AddShakeEvent(breachCameraShake);
        yield return new WaitForSeconds(1f);
        enemyBreach.SetActive(true);
        yield return new WaitForSeconds(15.5f);
        ithar.Play("ItharDisappear");
        GameManager.Instance.VolumeTransition(tensionVolume, constantVolumeCurve);
        yield return new WaitForSeconds(0.9f);
        DialogueManager.Instance.isCursed = true;
        DialogueManager.Instance.mist.SetTrigger("Appear");
        GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
        ithar.gameObject.SetActive(false);
        mistMovement.Play("MistMovement");
        yield return new WaitForSeconds(26f);
        CameraShake.Instance.AddShakeEvent(enemiesCameraShake);
    }

    private void Dialogues()
    {
        if (DialogueManager.Instance.isPlayingDialogue == false && playerAppeared)
        {
            Controller _controller = Controller.instance;
            remainingDialogues = dialogueQueue.Count;
            switch (remainingDialogues)
            {
                case 6 : //Après le tout premier dialogue
                    _controller.FreezePlayer(true);
                    GameManager.Instance.cameraController.ChangePoint(cameraPoint);
                    EnqueueDialogue();
                    Invoke("ResetCamera", 6);
                    DialogueManager.Instance.dialogueMoved = false;
                    Controller.instance.animatorPlayerHand.gameObject.SetActive(true);
                    break;
                case 5 : //Après avoir vu l'objet magique
                    _controller.FreezePlayer(false);
                    _controller.FreezePlayer(true, "Attack");
                    UIManager.Instance.gameObject.SetActive(true);
                    if (setHelp)
                    {
                        DialogueManager.Instance.IsCinematic(false);
                        setHelp = false;
                        StartCoroutine(helpManager.DisplayHelp());
                    }
                    break;
                case 4 : //Après avoir libéré Ithar
                    if (enemySpawn)
                    {
                        Controller.instance.animatorPlayerHand.gameObject.SetActive(true);
                        StartCoroutine(helpManager.DisplayHelp());
                        enemySpawn = false;
                        UIManager.Instance.gameObject.SetActive(true);
                        enemyBreach.SetActive(false);
                        StartCoroutine(SpawnEnnemiesCo());
                        DialogueManager.Instance.IsCinematic(false);
                        ResetCamera();
                        Controller.instance.FreezePlayer(false);
                        CardsController.instance.canUseCards = false;
                    }
                    if (ennemyParent.childCount == 0)
                    {
                        EnqueueDialogue();
                        setHelp = true;
                    }
                    break;
                case 3: //Après avoir tué les ennemis
                    if (ennemyParent.childCount == 0)
                    {
                        givePlayerFireCard.ActivGetCard();
                    }
                    break;
                case 2 : //Après avoir récupéré la carte de feu
                    if (setHelp)
                    {
                        setHelp = false;
                        StartCoroutine(helpManager.DisplayHelp());
                    }
                    _controller.FreezePlayer(false);
                    if (CardsController.instance.fireCardUnlock && !DialogueManager.Instance.isPlayingDialogue)
                    {
                        Debug.Log("ya pu cinématique");
                        DialogueManager.Instance.IsCinematic(false);
                        UIManager.Instance.gameObject.SetActive(true);
                        _controller.FreezePlayer(true, "Cards");
                        GameManager.Instance.TutorialWorld();
                        GameManager.Instance.VolumeTransition(GameManager.Instance.tutorialTransition, GameManager.Instance.cardTutorialCurve);
                        EnqueueDialogue();
                    }
                    
                    break;
                case 1 : //Une fois l'intro du monde tutoriel finie
                    FireCardTutorialManager.canStart = true;
                    if (FireCardTutorialManager.isFinished)
                    {
                        GameManager.Instance.volumeManager.enabled = false;
                        EnqueueDialogue();
                        StartCoroutine(ActivatePortal());
                        Invoke("ResetCamera", 6);
                    }
                    break;
                case 0 : //Une fois le deal passé
                    _controller.FreezePlayer(false);
                    Debug.Log("J'ai capté que le dialogue était fini");
                    if (GameManager.Instance.isTutorial && DialogueManager.Instance.isCinematic)
                    {
                        DialogueManager.Instance.IsCinematic(false);
                    }
                    UIManager.Instance.gameObject.SetActive(true);
                    gameObject.SetActive(false);
                    break;
                
            }
        }

        else
        {
            switch (remainingDialogues)
            {
                case 5:
                    if (!DialogueManager.Instance.isCursed)
                    {
                        GameManager.Instance.cameraController.ChangePoint(cameraPoint);
                    }
                    UIManager.Instance.gameObject.SetActive(false);
                    Controller.instance.ForceMove(prisonPosition.position);
                    Controller.instance.FreezePlayer(true);
                    if (touchedPrison)
                    {
                        prison.localScale += new Vector3(1f, 1f, 1f);
                        if (prison.localScale.x >= 20)
                        {
                            touchedPrison = false;
                            prison.gameObject.SetActive(false);
                        }
                    }
                    if (!itharStarted)
                    {
                        DialogueManager.Instance.IsCinematic(true);
                        itharStarted = true;
                        StartCoroutine(ManageIthar());
                    }
                    
                    break;
                case 3 :
                    Controller.instance.FreezePlayer(true);
                    if (fireCardCinematic)
                    {
                        fireCardCinematic = false;
                        DialogueManager.Instance.IsCinematic(true);
                    }
                    
                    UIManager.Instance.gameObject.SetActive(false);
                    break;
            }
        }
    }

    private IEnumerator PortalAppearance()
    {
        portal.SetTrigger("PortalOn");
        portalDarkCircle.SetTrigger("PortalOn");
        CameraShake.Instance.AddShakeEvent(appearCameraShake);
        yield return new WaitForSeconds(1.5f);
        teleportShader.SetActive(true);
        yield return new WaitForSeconds(2f);
        portal.ResetTrigger("PortalOn");
        portalDarkCircle.ResetTrigger("PortalOn");
        Controller.instance.gameObject.SetActive(true);
        Controller.instance.FreezePlayer(true);
        playerAppeared = true;
        EnqueueDialogue();
        yield return new WaitForSeconds(0.5f);
        teleportShader.SetActive(false);
    }

    private IEnumerator SpawnEnnemiesCo()
    {
        Destroy(Instantiate(eAppearFX, ennemiesSpawnPoints[1].position + new Vector3(0,3.3f, 0), Quaternion.identity, ennemyParent), 5f);
        Destroy(Instantiate(eAppearFX, ennemiesSpawnPoints[2].position + new Vector3(0,3.3f, 0), Quaternion.identity, ennemyParent), 5f);
        
        yield return new WaitForSeconds(3.5f);
        
        Instantiate(ennemies[0], ennemiesSpawnPoints[1].position, Quaternion.identity, ennemyParent);
        Instantiate(ennemies[0], ennemiesSpawnPoints[2].position, Quaternion.identity, ennemyParent);
    }
    private IEnumerator ActivatePortal()
    {
        yield return new WaitForSeconds(2f);
        portal.SetTrigger("PortalOn");
        portalDarkCircle.SetTrigger("PortalOn");
    }
    
}
