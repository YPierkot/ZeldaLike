using System.Collections;
using System.Linq;
using UnityEngine;

public class TriggeringDialogue : MonoBehaviour
{
    [SerializeField] private DialogueScriptable dialogue;
    private bool hasGivenDialogue;
    public bool isTutorial;
    [HideInInspector] public TutorialManager tutorialManager;
    public bool enemiesIsCondition;
    [HideInInspector] public Transform enemiesParent;

    public bool isCinematic;
    [HideInInspector] public float cinematicTime;
    [HideInInspector] public Transform cameraPoint;
    [HideInInspector] public string zoneName;
    [HideInInspector] public bool isMonolith;
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
                        StartCoroutine(Cinematic(cinematicTime, cameraPoint));
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
                }
            }
            else
            {
                hasGivenDialogue = true;
                DialogueManager.Instance.AssignDialogue(dialogue.dialogue.ToList());
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
        UIManager.Instance.playerLocation.text = zoneName;
        yield return new WaitForSeconds(4f);
        UIManager.Instance.playerLocation.text = null;
        StartCoroutine(DialogueManager.Instance.CinematicWait(timing - 4f));
    }
#if UNITY_EDITOR
    private void OnDrawGizmos() {
        if (!CustomLDData.showGizmos || !CustomLDData.showGizmosDialogue) return;
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, GetComponent<SphereCollider>().radius);
    }
#endif
}
