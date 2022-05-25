using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Chest : MonoBehaviour {
    [SerializeField] private Animator chestAnim = null;
    
    /// <summary>
    /// When the player enter the collision
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Controller.instance.playerInteraction = Interact;
        }
    }
    
    /// <summary>
    /// When the player exit the collision
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player") && Controller.instance.playerInteraction == Interact) {
            Controller.instance.playerInteraction = null;
        }
    }

    /// <summary>
    /// The method for the interaction
    /// </summary>
    private void Interact() {
        chestAnim.SetTrigger("OpenChest");
        Controller.instance.playerInteraction = null;
        enabled = false;
    }
}
