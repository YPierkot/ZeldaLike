using UnityEngine;

public class TestScriptRespawnPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = GameManager.Instance.actualRespawnPoint.position;
        }
    }
}
