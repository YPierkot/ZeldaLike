using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportationPortal : MonoBehaviour
{
    public bool changeScene;

    [SerializeField] private TeleportationPortal destination;
    [SerializeField] private string destinationSceneName;

    [SerializeField] private Animator teleportingShader;
    public Animator animator;
    public Animator particleAnimator;

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
            Controller.instance.FreezePlayer(true);
            teleportingShader.gameObject.SetActive(true);
            StartCoroutine(PlayerTeleporting());
        }
    }

    public IEnumerator PlayerTeleporting()
    {
        if (!changeScene)
        {
            animator.SetTrigger("PortalOn");
            particleAnimator.SetTrigger("PortalOn");
            yield return new WaitForSeconds(1f);
            Controller.instance.gameObject.SetActive(false);
            yield return new WaitForSeconds(2f);
            teleportingShader.gameObject.SetActive(false);
            StartCoroutine(destination.PlayerAppearing());
        }
        else
        {
            animator.SetTrigger("PortalOn");
            particleAnimator.SetTrigger("PortalOn");
            yield return new WaitForSeconds(1f);
            Controller.instance.gameObject.SetActive(false);
            SceneManager.LoadScene(destinationSceneName);
        }
        
    }

    public IEnumerator PlayerAppearing()
    {
        teleportingShader.ResetTrigger("TeleportIn");
        animator.SetTrigger("PortalOn");
        particleAnimator.SetTrigger("PortalOn");
        teleportingShader.gameObject.SetActive(true);
        Controller.instance.transform.position = transform.position;
        yield return new WaitForSeconds(2.1f);
        Controller.instance.transform.position = teleportingShader.transform.position;
        Controller.instance.FreezePlayer(false);
        Controller.instance.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        particleAnimator.ResetTrigger("PortalOn");
        animator.ResetTrigger("PortalOn");
        teleportingShader.gameObject.SetActive(false);
    }
}