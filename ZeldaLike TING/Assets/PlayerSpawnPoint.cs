using UnityEngine;
using UnityEngine.Rendering;

public class PlayerSpawnPoint : MonoBehaviour
{
    [SerializeField] private AudioClip forestAmbiance;
    [SerializeField] private VolumeProfile forest;
    void Start()
    {
        Controller.instance.FreezePlayer(false);
        Controller.instance.transform.position = transform.position;
        GameManager.Instance.volumeManager.profile = forest;
        SoundManager.Instance.SetAmbiance(forestAmbiance);
        if (DialogueManager.Instance.isCinematic)
        {
            DialogueManager.Instance.IsCinematic();
        }
    }
}
