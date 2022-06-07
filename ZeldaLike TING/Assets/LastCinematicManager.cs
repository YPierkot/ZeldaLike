using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PathCreation;
using PathCreation.Examples;
using TreeEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class LastCinematicManager : MonoBehaviour
{
    private bool started;
    [SerializeField] private PathCreator itharFirstPath;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Animator ithar;
    [SerializeField] private DialogueScriptable beatIthar;
    [SerializeField] private CameraShakeScriptable itharBeaten;
    [SerializeField] private DialogueScriptable kellGoodbye;
    [SerializeField] private VolumeProfile itharVolume;
    [SerializeField] private AnimationCurve itharVolumeCurve;
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private Animator prison;
    [SerializeField] private PathFollower itharFleePath;
    [SerializeField] private Transform monolithCameraPoint;
    [SerializeField] private CameraShakeScriptable teleportShake;
    [SerializeField] private AnimationCurve teleportationCurve;
    [SerializeField] private CameraShakeScriptable itharScream;
    [SerializeField] private Transform wideCameraPoint;
    [SerializeField] private PathCreator itharSecondPath;
    [SerializeField] private GameObject infiniteBreach;
    [SerializeField] private Transform lastCamera;
    [SerializeField] private GameObject secondBreach;
    private bool itharFollow;
    private bool itharCaptured;
    private bool mistLeaves;

    public IEnumerator LastCinematic()
    {
        started = true;
        ithar.transform.position = itharFleePath.transform.position;
        Controller.instance.transform.position = playerPosition.position;
        DialogueManager.Instance.AssignDialogue(beatIthar.dialogue.ToList());
        CameraShake.Instance.AddShakeEvent(itharBeaten);
        GameManager.Instance.VolumeTransition(itharVolume, itharVolumeCurve);
        GameManager.Instance.cameraController.ChangePoint(cameraPoint);
        DialogueManager.Instance.IsCinematic(true);
        ithar.Play("Boss_Idle");
        Controller.instance.animatorPlayer.SetBool("isRun", false);
        Controller.instance.animatorPlayerHand.gameObject.SetActive(false);
        DialogueManager.Instance.playerCanMove = false;
        Controller.instance.lastDir = new Vector3(1, 0, 0);
        yield return new WaitForSeconds(10.5f);
        prison.gameObject.SetActive(true);
        prison.Play("MagicalObjectAppear");
        yield return new WaitForSeconds(7.25f);
        itharFollow = true;
        itharFleePath.pathCreator = itharFirstPath;
        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.cameraController.ChangePoint(monolithCameraPoint);
        yield return new WaitForSeconds(3.25f);
        itharFleePath.enabled = false;
        yield return new WaitForSeconds(0.75f);
        CameraShake.Instance.AddShakeEvent(teleportShake);
        GameManager.Instance.VolumeTransition(itharVolume, teleportationCurve);
        yield return new WaitForSeconds(4.5f);
        itharFleePath.enabled = true;
        CameraShake.Instance.AddShakeEvent(itharScream);
        GameManager.Instance.cameraController.ChangePoint(cameraPoint);
        yield return new WaitForSeconds(2.5f);
        itharFollow = false;
        yield return new WaitForSeconds(6f);
        CameraShake.Instance.AddShakeEvent(itharScream);
        yield return new WaitForSeconds(5f);
        GameManager.Instance.cameraController.ChangePoint(wideCameraPoint);
        CameraShake.Instance.AddShakeEvent(itharScream);
        yield return new WaitForSeconds(6f);
        prison.Play("MagicalObjectGetIthar");
        CameraShake.Instance.AddShakeEvent(itharBeaten);
        yield return new WaitForSeconds(1f);
        CameraShake.Instance.AddShakeEvent(itharScream);
        itharCaptured = true;
        yield return new WaitForSeconds(19f);
        ithar.Play("Boss_TP");
        yield return new WaitForSeconds(0.3f);
        ithar.gameObject.SetActive(false);
        yield return new WaitUntil(() => !DialogueManager.Instance.isPlayingDialogue);
        SoundManager.Instance.musicSource.clip = SoundManager.Instance.exploMusic;
        SoundManager.Instance.musicSource.Play();
        GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
        DialogueManager.Instance.IsCinematic(true);
        prison.gameObject.SetActive(false);
        DialogueManager.Instance.AssignDialogue(kellGoodbye.dialogue.ToList());
        yield return new WaitForSeconds(6.5f);
        GameManager.Instance.mistMovement.Play("MistMovement");
        infiniteBreach.SetActive(true);
        CameraShake.Instance.AddShakeEvent(itharScream);
        yield return new WaitForSeconds(20.5f);
        mistLeaves = true;
        yield return new WaitForSeconds(18f);
        infiniteBreach.SetActive(false);
        secondBreach.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        DialogueManager.Instance.mist.gameObject.SetActive(false);
        yield return new WaitForSeconds(12.5f);
        UIManager.Instance.playerLocationTween.Play("PlayerLocation");
        UIManager.Instance.playerLocation.text = "FIN";
        GameManager.Instance.cameraController.ChangePoint(lastCamera);
        yield return new WaitForSeconds(4f);
        UIManager.Instance.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (started)
        {
            Controller.instance.FreezePlayer(true);
        }
        if (itharFollow)
        {
            ithar.transform.position = itharFleePath.transform.position;
        }

        if (itharCaptured)
        {
            ithar.transform.position = Vector3.MoveTowards(ithar.transform.position, prison.transform.position + new Vector3(0, 0, -2), 0.05f);
        }

        if (mistLeaves)
        {
            DialogueManager.Instance.mist.transform.position = Vector3.MoveTowards(
                DialogueManager.Instance.mist.transform.position,
                infiniteBreach.transform.position + new Vector3(0, 0, -1), 0.05f);
        }
    }
}