using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private UnityEngine.Events.UnityEvent activateEvent;
    private MeshRenderer mesh;

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    void Interact()
    {
            activateEvent.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mesh.material.color = Color.yellow;
            Controller.instance.playerInteraction = Interact;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mesh.material.color = Color.white;
            if (Controller.instance.playerInteraction == Interact) Controller.instance.playerInteraction = null;
        }
    }
}
