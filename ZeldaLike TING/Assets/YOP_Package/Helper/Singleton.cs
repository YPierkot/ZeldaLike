using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static T _instance = null;
    public static T Instance => _instance;

    private void Awake() {
        if (_instance == null) {
            _instance = this as T;
            Init();
        }
        else Destroy(this.gameObject);
    }

    /// <summary>
    /// Function called when this is initialized
    /// </summary>
    protected virtual void Init() {
    }
}
