using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationPortal : MonoBehaviour
{
    public bool changeScene;

    [SerializeField] private TeleportationPortal destination;

    [SerializeField] private string destinationSceneName;

    [SerializeField] private Animator teleportingShader;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Controller.instance.playerInteraction = Teleport;
        }
    }


    public void Teleport()
    {
        if (changeScene)
        {
            teleportingShader.SetTrigger("TeleportIn");
            teleportingShader.gameObject.SetActive(true);
            StartCoroutine(PlayerTeleporting());
        }
        else
        {
            teleportingShader.SetTrigger("TeleportIn");
            teleportingShader.gameObject.SetActive(true);
            StartCoroutine(PlayerTeleporting());
        }
    }

    public IEnumerator PlayerTeleporting()
    {
        yield return new WaitForSeconds(1f);
        Controller.instance.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        teleportingShader.gameObject.SetActive(false);
        StartCoroutine(destination.PlayerAppearing());
    }

    public IEnumerator PlayerAppearing()
    {
        teleportingShader.ResetTrigger("TeleportIn");
        teleportingShader.gameObject.SetActive(true);
        Controller.instance.transform.position = transform.position;
        yield return new WaitForSeconds(2.1f);
        Controller.instance.transform.position = teleportingShader.transform.position;
        Controller.instance.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        teleportingShader.gameObject.SetActive(false);
    }
}
