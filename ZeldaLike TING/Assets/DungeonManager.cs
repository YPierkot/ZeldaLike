using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{

    [Header("First Part")]
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private DialogueScriptable entranceDialogue;
    [SerializeField] private GameObject puzzleCube;
    
    [Header("Aeryn")]
    [SerializeField] private Transform aerynCameraPoint;
    [SerializeField] private Transform enemyParent;
    [SerializeField] private AerynBehaviour aeryn;
    [SerializeField] private GameObject barrier;
    private bool freedAeryn;
    private bool enemiesSpawned;
    [SerializeField] private List<DialogueScriptable> aerynDialogues;
    private Queue<DialogueScriptable> dialogues;
    
    [Header("Ice Card")]
    public bool startIce;
    [SerializeField] private GameObject iceCard;
    
    [Header("Puzzle")]
    [SerializeField] private Transform leverCamera;
    [SerializeField] private Transform puzzleCamera;
    [SerializeField] private Animator puzzleBounds;
    [SerializeField] private bool puzzleFinished;
    [SerializeField] private RunePuzzleManager runePuzzleManager;
    private bool puzzleDisappear;
    [SerializeField] private DialogueScriptable underAttack;
    [SerializeField] private EnemySpawnTrigger spawner;
    [SerializeField] private EnemySpawnTrigger secondWave;
    private bool secondWaveSpawned = true;
    
    [Header("ManaPool")]
    private bool enteredManaPool;
    [SerializeField] private Transform doorCameraPoint;
    [SerializeField] private Transform lastleverCamera;
    [SerializeField] private Transform manaPoolCamera;
    [SerializeField] private Transform wideManaPoolCamera;
    [SerializeField] private TeleportationPortal portal;
    private bool goingToPortal;
    [SerializeField] private CameraShakeScriptable aeryngettingPowers;
    [SerializeField] private Animator manaPool;
 

    private void Start()
    {
        dialogues = new Queue<DialogueScriptable>();
        foreach (var dialogue in aerynDialogues)
        {
            dialogues.Enqueue(dialogue);
        }
    }

    private IEnumerator OnEntrance()
    {
        Controller.instance.FreezePlayer(true);
        DialogueManager.Instance.IsCinematic(true);
        GameManager.Instance.cameraController.ChangePoint(cameraPoint);
        DialogueManager.Instance.AssignDialogue(entranceDialogue.dialogue.ToList());
        yield return new WaitForSeconds(3f);
        puzzleCube.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
        Controller.instance.FreezePlayer(false);
        DialogueManager.Instance.IsCinematic(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GameManager.Instance.isTutorial && other.CompareTag("Player"))
        {
            StartCoroutine(OnEntrance());
        }
    }

    public void FreeingAeryn()
    {
        GameManager.Instance.cameraController.ChangePoint(aerynCameraPoint);
        DialogueManager.Instance.AssignDialogue(dialogues.Dequeue().dialogue.ToList());
        DialogueManager.Instance.IsCinematic(true);
        Controller.instance.FreezePlayer(true);
        freedAeryn = true;
    }

    private void Update()
    {
        switch (dialogues.Count)
        {
            case 7:
                if (freedAeryn && !DialogueManager.Instance.isPlayingDialogue)
                {
                    GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
                    if (DialogueManager.Instance.isCinematic)
                    {
                        DialogueManager.Instance.IsCinematic(false);
                    }
                    aeryn.isFreed = true;
                    Controller.instance.FreezePlayer(false);
                    DialogueManager.Instance.AssignDialogue(dialogues.Dequeue().dialogue.ToList());
                }

                break;
            case 6:
                if (!enemiesSpawned && enemyParent.childCount > 5)
                {
                    enemiesSpawned = true;
                }
                if (enemyParent.childCount <= 5 && enemiesSpawned)
                {
                    barrier.SetActive(false);
                }
                if (startIce)
                {
                    StartCoroutine(GiveIceCard());
                    DialogueManager.Instance.AssignDialogue(dialogues.Dequeue().dialogue.ToList());
                    iceCard.SetActive(true);
                }
                break;
            case 5 :
                if (CardsController.instance.iceCardUnlock && !DialogueManager.Instance.isPlayingDialogue)
                {
                    StartCoroutine(LeverCamera());
                    DialogueManager.Instance.AssignDialogue(dialogues.Dequeue().dialogue.ToList());
                }
                break;
            case 4 :
                if (!puzzleBounds.gameObject.activeSelf)
                {
                    aeryn.firstPath = false;
                    DialogueManager.Instance.AssignDialogue(dialogues.Dequeue().dialogue.ToList());
                }
                break;
            case 3 :
                if (puzzleFinished && !puzzleDisappear && runePuzzleManager.gameObject.activeSelf) 
                {
                    StartCoroutine(PuzzleIsFinished());
                }
                if (spawner.enemiesParent.childCount == 4 && !secondWaveSpawned && puzzleFinished)
                {
                    secondWaveSpawned = true;
                    secondWave.SpawnEnemies();
                }
                if (puzzleFinished && spawner.enemiesParent.childCount == 4 && secondWave.enemiesParent.childCount == 5 && !runePuzzleManager.gameObject.activeSelf)
                {
                    secondWave.barrier.SetActive(false);
                    aeryn.isThirdPath = true;
                    DialogueManager.Instance.AssignDialogue(dialogues.Dequeue().dialogue.ToList());
                }
                break;
            case 2 :
                if (enteredManaPool)
                {
                    StartCoroutine(ManaPoolCinematic());
                }
                break;
            case 1 :
                if (goingToPortal)
                {
                    GameManager.Instance.isDungeonFinished = true;
                }
                    
                break;
                
        }
        if (puzzleDisappear)
        {
            runePuzzleManager.RunesDisappear();
        }
    }

    public void IceDialogue()
    {
        if(aeryn.isFreed) startIce = true;
    }

    public void ActivateLever()
    {
        StartCoroutine(ShowPuzzleBounds());
        puzzleBounds.Play("PuzzleBounds");
    }

    public void PuzzleFinished()
    {
        puzzleFinished = true;
    }

    public void EnteredManaPool()
    {
        enteredManaPool = true;
    }

    private IEnumerator GiveIceCard()
    {
        Controller.instance.FreezePlayer(true);
        DialogueManager.Instance.IsCinematic(true);
        GameManager.Instance.cameraController.ChangePoint(doorCameraPoint);
        yield return new WaitForSeconds(5f);
        GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
        Controller.instance.FreezePlayer(false);
        DialogueManager.Instance.IsCinematic(false);
    }

    private IEnumerator LeverCamera()
    {
        yield return new WaitForSeconds(3f);
        iceCard.SetActive(false);
        yield return new WaitForSeconds(12f);
        GameManager.Instance.cameraController.ChangePoint(leverCamera);
        yield return new WaitForSeconds(4f);
        GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
    }

    private IEnumerator ShowPuzzleBounds()
    {
        Controller.instance.FreezePlayer(true);
        GameManager.Instance.cameraController.ChangePoint(puzzleCamera);
        yield return new WaitForSeconds(5f);
        Controller.instance.FreezePlayer(false);
        puzzleBounds.gameObject.SetActive(false);
        GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
    }

    private IEnumerator PuzzleIsFinished()
    {
        runePuzzleManager.PuzzleDisappear();
        Controller.instance.FreezePlayer(true);
        GameManager.Instance.cameraController.ChangePoint(puzzleCamera);
        puzzleDisappear = true;
        yield return new WaitForSeconds(2f);
        DialogueManager.Instance.AssignDialogue(underAttack.dialogue.ToList());
        spawner.SpawnEnemies();
        Controller.instance.FreezePlayer(false);
        GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
        yield return new WaitForSeconds(2f);
        secondWaveSpawned = false;
        puzzleDisappear = false;
        runePuzzleManager.gameObject.SetActive(false);
    }

    private IEnumerator ManaPoolCinematic()
    {
        Controller.instance.FreezePlayer(true);
        GameManager.Instance.cameraController.ChangePoint(wideManaPoolCamera);
        DialogueManager.Instance.IsCinematic(true);
        DialogueManager.Instance.AssignDialogue(dialogues.Dequeue().dialogue.ToList());
        yield return new WaitForSeconds(5f);
        aeryn.isFourthPath = true;
        GameManager.Instance.cameraController.ChangePoint(manaPoolCamera);
        yield return new WaitForSeconds(2.5f);
        CameraShake.Instance.AddShakeEvent(aeryngettingPowers);
        manaPool.Play("ManaPool");
        yield return new WaitForSeconds(3f);
        aeryn.shield.SetActive(true);
        yield return new WaitForSeconds(2f);
        GameManager.Instance.cameraController.ChangePoint(lastleverCamera);
        portal.animator.SetTrigger("PortalOn");
        portal.particleAnimator.SetTrigger("PortalOn");
        yield return new WaitForSeconds(3f);
        GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
        Controller.instance.FreezePlayer(false);
        DialogueManager.Instance.IsCinematic(false);
    }
}
