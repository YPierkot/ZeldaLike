using UnityEngine;
using UnityEngine.Events;

public class PlayerTrigger : MonoBehaviour
{
    public UnityEvent OnEntry;
    public bool respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (respawnPoint)
            {
                GameManager.Instance.SetSpawnPoint(transform);
            }
            OnEntry.Invoke();
        }
    }
}
