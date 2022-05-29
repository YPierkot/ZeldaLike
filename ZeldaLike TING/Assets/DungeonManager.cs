using System;
using System.Collections;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{

    [SerializeField] private Transform cameraPoint;
    [SerializeField] private GameObject puzzleCube;

    private IEnumerator OnEntrance()
    {
        Controller.instance.FreezePlayer(true);
        DialogueManager.Instance.IsCinematic();
        GameManager.Instance.cameraController.ChangePoint(cameraPoint);
        yield return new WaitForSeconds(3f);
        puzzleCube.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
        Controller.instance.FreezePlayer(false);
        DialogueManager.Instance.IsCinematic();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GameManager.Instance.isTutorial && other.CompareTag("Player"))
        {
            StartCoroutine(OnEntrance());
        }
    }
}
