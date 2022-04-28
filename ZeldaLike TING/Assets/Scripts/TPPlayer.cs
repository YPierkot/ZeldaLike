using UnityEngine;
using UnityEngine.SceneManagement;

public class TPPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            SceneManager.LoadScene("Gameplay");
        }
    }
}
