using UnityEngine;
using UnityEngine.SceneManagement;

public class TPPlayer : MonoBehaviour
{
    [SerializeField] private string sceneName = "PlaytestPuzzle";
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && !DialogueManager.Instance.isPlayingDialogue)
        {
            SceneManager.LoadScene(sceneName);
        }

        if (DialogueManager.Instance.isPlayingDialogue && other.transform.CompareTag("Player"))
        {
            UIManager.Instance.gameObject.SetActive(false);
            DialogueManager.Instance.IsCinematic();
            Controller.instance.FreezePlayer(true);
        }
    }
}