using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerSpawnPoint : MonoBehaviour
{
    [Header("Forest Variables")]
    [SerializeField] private AudioClip forestAmbiance;
    [SerializeField] private VolumeProfile forest;
    
    [Header("Cinematic Variables")]
    [SerializeField] private string location;
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private float duration;
    [SerializeField] private DialogueScriptable startDialogue;
    private HelpsManager HelpsManager;
    [SerializeField] private DialogueScriptable importantInfo;

    void Start()
    {
        HelpsManager = GetComponent<HelpsManager>();
        Controller.instance.FreezePlayer(false);
        Controller.instance.transform.position = transform.position;
        GameManager.Instance.volumeManager.profile = forest;
        SoundManager.Instance.SetAmbiance(forestAmbiance);
        if (DialogueManager.Instance.isCinematic)
        {
            DialogueManager.Instance.IsCinematic();
        }
        StartCoroutine(SmallCinematic());
    }

    private IEnumerator SmallCinematic()
    {
        Controller.instance.FreezePlayer(true);
        DialogueManager.Instance.IsCinematic();
        UIManager.Instance.playerLocation.text = location;
        GameManager.Instance.cameraController.ChangePoint(cameraPoint);
        yield return new WaitForSeconds(duration);
        UIManager.Instance.playerLocation.text = null;
        GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
        DialogueManager.Instance.AssignDialogue(startDialogue.dialogue.ToList());
        HelpsManager.DisplayHelp();
        DialogueManager.Instance.EnqueuedDialogue = importantInfo.dialogue.ToList();
        StartCoroutine(DialogueManager.Instance.CinematicWait(12f));

    }
}
