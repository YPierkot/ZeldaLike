using UnityEngine;
using UnityEngine.Events;

public class DestructableObject : MonoBehaviour {
    [SerializeField] private UnityEvent destroyAction;
    
    /// <summary>
    /// Destroy the object when needed
    /// </summary>
    public void DestroyObject() {
        destroyAction.Invoke();
        Destroy(gameObject);
    }
}
