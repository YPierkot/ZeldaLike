using UnityEngine;

public class TestScriptRespawnPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(PlayerStat.instance.PlayerDeath());
        }
    }
}
