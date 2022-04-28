using UnityEngine;
using UnityEngine.Rendering;

public class PlayerSpawnPoint : MonoBehaviour
{
    [SerializeField] private AudioClip forestAmbiance;
    [SerializeField] private VolumeProfile forest;
    void Awake()
    {
        Controller.instance.transform.position = transform.position;
        GameManager.Instance.volumeManager.profile = forest;
        SoundManager.Instance.SetAmbiance(forestAmbiance);
    }
}
