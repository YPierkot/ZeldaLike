using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SphereCollider))]
public class PlayerInteraction : MonoBehaviour {
    [SerializeField] private UnityEvent Interaction;
    
    private PlayerInputMap playerInput = null;
    private bool canPressInput = false;
    
    private void Start() {
        playerInput = new PlayerInputMap();
        playerInput.Action.Interaction.started += InteractWithObject;
    }

    /// <summary>
    /// When the player press the interaction key
    /// </summary>
    /// <param name="obj"></param>
    private void InteractWithObject(InputAction.CallbackContext obj) {
        if (!canPressInput) return;
        Interaction.Invoke();
    }

    /// <summary>
    /// When player enter in the trigger area
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        playerInput.Enable();
        canPressInput = true;
    }

    /// <summary>
    /// When the player exit the trigger area
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other) {
        playerInput.Disable();
        canPressInput = false;
    }
}
