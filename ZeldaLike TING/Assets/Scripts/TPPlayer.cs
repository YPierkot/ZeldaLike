using UnityEngine;
using UnityEngine.SceneManagement;

public class TPPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && !DialogueManager.Instance.isPlayingDialogue)
        {
            SceneManager.LoadScene("LD_Playtest");
        }

        if (DialogueManager.Instance.isPlayingDialogue && other.transform.CompareTag("Player"))
        {
            UIManager.Instance.gameObject.SetActive(false);
            DialogueManager.Instance.IsCinematic();
            Controller.instance.FreezePlayer(true);
        }
    }
}