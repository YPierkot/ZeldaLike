using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerSpawnPoint : MonoBehaviour
{
    [Header("Forest Variables")]
    [SerializeField] private AudioClip forestAmbiance;
    [SerializeField] private VolumeProfile forest;

    [Header("Cinematic Variables")]
    [SerializeField] private string location;
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private float duration;
    [SerializeField] private DialogueScriptable startDialogue;
    private HelpsManager HelpsManager;
    [SerializeField] private DialogueScriptable importantInfo;
    [SerializeField] private Transform dungeonSpawnPoint;
    [SerializeField] private DialogueScriptable thePlan;
    [SerializeField] private GameObject aeryn;
    [SerializeField] private Transform monolithCamera;
    [SerializeField] private TeleportationPortal portal;
    private bool spawned;
    [SerializeField] private Transform islandCamera;

    void Start()
    {
        GameManager.Instance.isTutorial = false;
        HelpsManager = GetComponent<HelpsManager>();
        Controller.instance.FreezePlayer(false);
        Controller.instance.transform.position = transform.position;
        GameManager.Instance.volumeManager.profile = forest;
        SoundManager.Instance.SetAmbiance(forestAmbiance);
        if (DialogueManager.Instance.isCinematic)
        {
            DialogueManager.Instance.IsCinematic();
        }
        //StartCoroutine(SmallCinematic());
    }

    private IEnumerator SmallCinematic()
    {
        if (!GameManager.Instance.isDungeonFinished)
        {
            Controller.instance.FreezePlayer(true);
            Controller.instance.gameObject.SetActive(true);
            DialogueManager.Instance.IsCinematic();
            //UIManager.Instance.playerLocationTween.Play("PlayerLocation");
            UIManager.Instance.playerLocation.text = location;
            GameManager.Instance.cameraController.ChangePoint(cameraPoint);
            yield return new WaitForSeconds(duration-0.5f);
            //UIManager.Instance.playerLocationTween.Play("PlayerLocationOut");
            yield return new WaitForSeconds(0.5f);
            UIManager.Instance.playerLocation.text = null;
            GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
            DialogueManager.Instance.AssignDialogue(startDialogue.dialogue.ToList());
            StartCoroutine(HelpsManager.DisplayHelp());
            DialogueManager.Instance.EnqueuedDialogue = importantInfo.dialogue.ToList();
            StartCoroutine(DialogueManager.Instance.CinematicWait(23f));
            HelpsManager.ResetHelpText();
        }
    }

    /*private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.isDungeonFinished && !spawned)
        {
            spawned = true;
            Controller.instance.transform.position = dungeonSpawnPoint.position;
            Controller.instance.gameObject.SetActive(false);
            aeryn.SetActive(true);
            DialogueManager.Instance.IsCinematic();
            StartCoroutine(ThePlan());
        }
    }*/

    private IEnumerator ThePlan()
    {
        StartCoroutine(portal.PlayerAppearing());
        yield return new WaitForSeconds(2f);
        Controller.instance.FreezePlayer(true);
        DialogueManager.Instance.AssignDialogue(thePlan.dialogue.ToList());
        yield return new WaitForSeconds(6f);
        GameManager.Instance.cameraController.ChangePoint(monolithCamera);
        yield return new WaitForSeconds(4f);
        GameManager.Instance.cameraController.ChangePoint(islandCamera);
        yield return new WaitForSeconds(8f);
        GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
        Controller.instance.FreezePlayer(false);
        DialogueManager.Instance.IsCinematic();
    }
}
