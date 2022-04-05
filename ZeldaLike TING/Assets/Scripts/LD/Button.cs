using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private UnityEngine.Events.UnityEvent activateEvent;
    private MeshRenderer mesh;

    private bool inRange;

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            activateEvent.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mesh.material.color = Color.yellow;
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mesh.material.color = Color.white;
            inRange = false;
        }
    }
}
