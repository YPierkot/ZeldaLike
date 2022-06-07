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
    private bool kellMoving;

    [Header("PostProcess & GameFeel")]
    [SerializeField] private VolumeProfile transitionVolume;
    [SerializeField] private AnimationCurve transitionCurve;
    [SerializeField] private CameraShakeScriptable cameraShake;
    [SerializeField] private VolumeProfile hardTransition;
    [SerializeField] private AnimationCurve hardCurve;

    private void Start()
    {
        DialogueManager.Instance.IsCinematic(true);
        Controller.instance.animatorPlayerHand.gameObject.SetActive(false);
        GameManager.Instance.cameraController.ChangePoint(cameraPoint);
        DialogueManager.Instance.AssignDialogue(dialogue.dialogue.ToList());
        UIManager.Instance.gameObject.SetActive(false);
        SoundManager.Instance.musicSource.clip = SoundManager.Instance.exploMusic;
        SoundManager.Instance.musicSource.Play();
        Controller.instance.FreezePlayer(true);
        Controller.instance.Rotate(new Vector2(0, 1));
        StartCoroutine(ObjectManagement());
    }

    private void FixedUpdate()
    {
        wizardTransform.position = wizard.transform.position;
        Controller.instance.transform.position = kellPath.transform.position;
        if (kellMoving)
        {
            DialogueManager.Instance.playerCanMove = true;
        }
        else
        {
            DialogueManager.Instance.playerCanMove = false;
            Controller.instance.animatorPlayer.SetBool("isRun", false);
        }
    }

    private IEnumerator ObjectManagement()
    {
        yield return new WaitForSeconds(8);
        wizard.enabled = true;
        yield return new WaitForSeconds(3f);
        kellPath.enabled = true;
        kellMoving = true;
        Controller.instance.animatorPlayer.SetTrigger("isRun");
        Controller.instance.lastDir = new Vector3(-1, 0, 1);
        Debug.Log("gauche");
        yield return new WaitForSeconds(0.5f);
        Controller.instance.lastDir = new Vector3(0, 0, 1);
        Debug.Log("haut");
        yield return new WaitForSeconds(0.4f);
        Controller.instance.lastDir = new Vector3(1, 0, 1);
        Debug.Log("droite");
        yield return new WaitForSeconds(1.3f);
        Controller.instance.lastDir = new Vector3(0, 0, 1);
        Debug.Log("haut");
        yield return new WaitForSeconds(0.35f);
        Controller.instance.lastDir = new Vector3(-1, 0, 0);
        Debug.Log("gauche");
        yield return new WaitForSeconds(0.25f);
        kellMoving = false;
        Debug.Log("J'arrête le joueur");
        yield return new WaitForSeconds(5f);
        GameManager.Instance.VolumeTransition(transitionVolume, transitionCurve);
        CameraShake.Instance.AddShakeEvent(cameraShake);
        SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.portalAppear);
        yield return new WaitForSeconds(1f);
        GameManager.Instance.volumeManager.profile = transitionVolume;
        GameManager.Instance.VolumeTransition(hardTransition, hardCurve);
        yield return new WaitForSeconds(0.4f);
        UIManager.Instance.loadingScreen.SetActive(true);
        yield return new WaitForSeconds(2f);
        GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
        DialogueManager.Instance.IsCinematic(false);
        SceneManager.LoadScene("_Scenes/SceneWorkflow/LD_DonjonPrinc");
    }
}
