using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    [SerializeField] private AudioClip forestAmbiance;
    void Start()
    {
        Controller.instance.transform.position = transform.position;
        GameManager.Instance.volumeManager.profile = null;
        SoundManager.Instance.SetAmbiance(forestAmbiance);
    }
}
