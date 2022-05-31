using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PathCreation.Examples;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class CinematicManager : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueScriptable dialogue;
    
    [Header("Objects")]
    

    [SerializeField] private Transform cameraPoint;
    
    [Header("Waypoints & Movement")]
    [SerializeField] private PathFollower wizard;

    [SerializeField] private Transform wizardTransform;
    [SerializeField] private PathFollower kellPath;

    [Header("PostProcess & GameFeel")]
    [SerializeField] private VolumeProfile transitionVolume;
    [SerializeField] private AnimationCurve transitionCurve;
    [SerializeField] private CameraShakeScriptable cameraShake;
    [SerializeField] private VolumeProfile hardTransition;
    [SerializeField] private AnimationCurve hardCurve;

    private void Start()
    {
        GameManager.Instance.cameraController.ChangePoint(cameraPoint);
        DialogueManager.Instance.AssignDialogue(dialogue.dialogue.ToList());
        UIManager.Instance.gameObject.SetActive(false);
        Controller.instance.FreezePlayer(true);
        StartCoroutine(ObjectManagement());
    }

    private void FixedUpdate()
    {
        wizardTransform.position = wizard.transform.position;
        Controller.instance.transform.position = kellPath.transform.position;

    }

    private IEnumerator ObjectManagement()
    {
        yield return new WaitForSeconds(8);
        wizard.enabled = true;
        yield return new WaitForSeconds(3f);
        kellPath.enabled = true;
        yield return new WaitForSeconds(7.5f);
        GameManager.Instance.VolumeTransition(transitionVolume, transitionCurve);
        CameraShake.Instance.AddShakeEvent(cameraShake);
        yield return new WaitForSeconds(1);
        GameManager.Instance.volumeManager.profile = transitionVolume;
        GameManager.Instance.VolumeTransition(hardTransition, hardCurve);
        yield return new WaitForSeconds(0.4f);
        UIManager.Instance.loadingScreen.SetActive(true);
        yield return new WaitForSeconds(2f);
        GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
        DialogueManager.Instance.IsCinematic();
        SceneManager.LoadScene("_Scenes/Level Design/LD_DonjonPrinc");
    }
}
