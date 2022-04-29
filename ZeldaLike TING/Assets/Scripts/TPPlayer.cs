using UnityEngine;
using UnityEngine.SceneManagement;

public class TPPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && !DialogueManager.Instance.isPlayingDialogue)
        {
            SceneManager.LoadScene("Gameplay");
        }

        if (DialogueManager.Instance.isPlayingDialogue && other.transform.CompareTag("Player"))
        {
            UIManager.Instance.gameObject.SetActive(false);
            DialogueManager.Instance.IsCinematic();
            Controller.instance.FreezePlayer(true);
        }
    }
}