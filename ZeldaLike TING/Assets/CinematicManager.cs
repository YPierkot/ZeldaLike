using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class CinematicManager : MonoBehaviour
{
    [SerializeField] private DialogueScriptable dialogue;
    [SerializeField] private GameObject wizard;
    private bool wizardMove;
    [SerializeField] private Transform wizardWaypoint;
    [SerializeField] private List<Transform> waypointList;
    private bool kellMove;
    private int waypointIndex;
    [SerializeField] private VolumeProfile transitionVolume;
    [SerializeField] private AnimationCurve transitionCurve;
    [SerializeField] private CameraShakeScriptable cameraShake;
    [SerializeField] private VolumeProfile hardTransition;
    [SerializeField] private AnimationCurve hardCurve;

    private void Start()
    {
        DialogueManager.Instance.AssignDialogue(dialogue.dialogue.ToList());
        UIManager.Instance.gameObject.SetActive(false);
        Controller.instance.FreezePlayer(true);
        StartCoroutine(ObjectManagement());
    }

    private void FixedUpdate()
    {
        if (wizardMove)
        {
            wizard.transform.Translate((wizardWaypoint.position - wizard.transform.position).normalized * 0.05f);
        }

        if (kellMove)
        {
            Controller.instance.ForceMove(waypointList[waypointIndex].position);
        }

    }

    private IEnumerator ObjectManagement()
    {
        yield return new WaitForSeconds(8);
        wizardMove = true;
        yield return new WaitForSeconds(3f);
        wizardMove = false;
        kellMove = true;
        waypointIndex = 0;
        yield return new WaitForSeconds(0.5f);
        waypointIndex = 1;
        yield return new WaitForSeconds(0.8f);
        waypointIndex = 2;
        yield return new WaitForSeconds(7.5f);
        GameManager.Instance.VolumeTransition(transitionVolume, transitionCurve);
        CameraShake.Instance.AddShakeEvent(cameraShake);
        yield return new WaitForSeconds(1);
        GameManager.Instance.volumeManager.profile = transitionVolume;
        GameManager.Instance.VolumeTransition(hardTransition, hardCurve);
        yield return new WaitForSeconds(0.4f);
        UIManager.Instance.loadingScreen.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("_Scenes/SceneWorkflow/LD_Tuto");
    }
}
