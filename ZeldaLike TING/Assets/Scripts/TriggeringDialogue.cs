using System.Collections;
using System.Linq;
using UnityEngine;

public class TriggeringDialogue : MonoBehaviour
{
    [SerializeField] private DialogueScriptable dialogue;
    public DialogueScriptable importantInfo;
    private bool hasGivenDialogue;
    public bool isTutorial;
    [HideInInspector] public TutorialManager tutorialManager;
    public bool enemiesIsCondition;
    [HideInInspector] public Transform enemiesParent;
    [SerializeField] private GameObject barrier;

    public bool isCinematic;
    [HideInInspector] public float cinematicTime;
    [HideInInspector] public Transform cameraPoint;
    [HideInInspector] public string zoneName;
    [HideInInspector] public bool isMonolith;
    [HideInInspector] public bool isEarthMonolith;
    [HideInInspector] public Animator mistMovement;
    [HideInInspector] public Animator monolithFX;
    [HideInInspector] public DialogueScriptable shortMonolithDialogue;

    private Transform defaultCamera;

    private void Start()
    {
        defaultCamera = Controller.instance.PlayerCameraPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasGivenDialogue)
        {
            if (barrier != null)
            {
                barrier.SetActive(true);
            }
            if (isTutorial)
            {
                if (enemiesIsCondition)
                {
                    if (enemiesParent.childCount == 0)
                    {
                        hasGivenDialogue = true;
                        tutorialManager.EnqueueDialogue();
                        if (isCinematic)
                        {
                            StartCoroutine(Cinematic(cinematicTime, cameraPoint));
                        }
                    }
                }
                else
                {
                    hasGivenDialogue = true;
                    tutorialManager.EnqueueDialogue();
                    if (isCinematic)
                    {
                        StartCoroutine(Cinematic(cinematicTime, cameraPoint));
                    }
                }
            }
            else if (isMonolith)
            {
                hasGivenDialogue = true;
                if (GameManager.Instance.foundMonolith)
                {
                    DialogueManager.Instance.AssignDialogue(shortMonolithDialogue.dialogue.ToList());
                    if (isCinematic)
                    {
                        cinematicTime = 8;
                        StartCoroutine(Cinematic(cinematicTime, cameraPoint));
                    }

                    if (isEarthMonolith)
                    {
                        StartCoroutine(EarthMonolithCinematic(1));
                    }
                    else
                    {
                        StartCoroutine(WindMonolithCinematic(1));
                    }
                }
                else
                {
                    GameManager.Instance.foundMonolith = true;
                    DialogueManager.Instance.AssignDialogue(dialogue.dialogue.ToList());
                    if (isCinematic)
                    {
                        StartCoroutine(Cinematic(cinematicTime, cameraPoint));
                    }
                    if (isEarthMonolith)
                    {
                        StartCoroutine(EarthMonolithCinematic(22));
                    }
                    else
                    {
                        StartCoroutine(WindMonolithCinematic(22));
                    }
                }

                if (importantInfo != null)
                {
                    DialogueManager.Instance.EnqueuedDialogue = importantInfo.dialogue.ToList();
                }
            }
            else
            {
                hasGivenDialogue = true;
                DialogueManager.Instance.AssignDialogue(dialogue.dialogue.ToList());
                if (importantInfo != null)
                {
                    DialogueManager.Instance.EnqueuedDialogue = importantInfo.dialogue.ToList();
                }
                if (isCinematic)
                {
                    StartCoroutine(Cinematic(cinematicTime, cameraPoint));
                }
            }
        }
    }

    IEnumerator Cinematic(float timing, Transform camera = null)
    {
        DialogueManager.Instance.IsCinematic();
        Controller.instance.FreezePlayer(true);
        if (camera != null)
        {
            GameManager.Instance.cameraController.ChangePoint(camera);
        }
        UIManager.Instance.playerLocationTween.Play("PlayerLocation");
        UIManager.Instance.playerLocation.text = zoneName;
        yield return new WaitForSeconds(3.5f);
        UIManager.Instance.playerLocationTween.Play("PlayerLocationOut");
        yield return new WaitForSeconds(0.5f);
        UIManager.Instance.playerLocation.text = null;
        StartCoroutine(DialogueManager.Instance.CinematicWait(timing - 4f));
    }

    private IEnumerator EarthMonolithCinematic(float timer)
    {
        yield return new WaitForSeconds(timer);
        monolithFX.Play("EarthMonolithFX");
        mistMovement.Play("EarthMonolithMist");
    }
    private IEnumerator WindMonolithCinematic(float timer)
    {
        yield return new WaitForSeconds(timer);
        monolithFX.Play("WindMonolythFX");
        mistMovement.Play("WindMonolithMist");
    }

    public IEnumerator IceMonolithCinematic(float timer)
    {
        yield return null;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos() {
        if (!CustomLDData.showGizmos || !CustomLDData.showGizmosDialogue) return;
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, GetComponent<SphereCollider>().radius);
    }
#endif
}
