using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PathCreation;
using PathCreation.Examples;
using UnityEngine;
using UnityEngine.Rendering;

public class LastCinematicManager : MonoBehaviour
{
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
    private bool itharFollow;

    public IEnumerator LastCinematic()
    {
        ithar.transform.position = itharFleePath.transform.position;
        Controller.instance.transform.position = playerPosition.position;
        DialogueManager.Instance.AssignDialogue(beatIthar.dialogue.ToList());
        CameraShake.Instance.AddShakeEvent(itharBeaten);
        GameManager.Instance.VolumeTransition(itharVolume, itharVolumeCurve);
        GameManager.Instance.cameraController.ChangePoint(cameraPoint);
        DialogueManager.Instance.IsCinematic(true);
        Controller.instance.FreezePlayer(true);
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
        yield return new WaitForSeconds(3.5f);
        itharFleePath.enabled = false;
        yield return new WaitForSeconds(0.5f);
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
        yield return new WaitForSeconds(1f);
        CameraShake.Instance.AddShakeEvent(itharScream);
        
    }

    private void FixedUpdate()
    {
        if (itharFollow)
        {
            ithar.transform.position = itharFleePath.transform.position;
        }
    }
}