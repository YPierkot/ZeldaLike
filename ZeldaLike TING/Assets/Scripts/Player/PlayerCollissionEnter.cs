using UnityEngine;
using UnityEngine.Events;

public class PlayerCollissionEnter : MonoBehaviour {
    [SerializeField] private UnityEvent enterCollisionEvent;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            enterCollisionEvent.Invoke();
        }
    }
}