using UnityEngine;
using UnityEngine.Events;

public class InteracteWithObect : MonoBehaviour {
    [SerializeField] private bool destroyObjectAfterHitting = true;
    [SerializeField] private UnityEvent destroyAction;
    
    /// <summary>
    /// Destroy the object when needed
    /// </summary>
    public void InteractWithObject() {
        destroyAction.Invoke();
        if(destroyObjectAfterHitting) Destroy(gameObject);
    }
}
