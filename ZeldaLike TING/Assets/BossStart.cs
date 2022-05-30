using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class BossStart : MonoBehaviour
{
    [SerializeField] private GameObject bossIsland;
    [SerializeField] private GameObject monolith;
    [SerializeField] private Animator ithar;
    [SerializeField] private DialogueScriptable itharMeetKell;
    [SerializeField] private TeleportationPortal portalFX;
    [SerializeField] private Transform teleportationPoint;
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private VolumeProfile itharAppearProfile;
    [SerializeField] private AnimationCurve profileAnimationCurve;
    [SerializeField] private CameraShakeScriptable itharAppearCameraShake;
    [SerializeField] private Transform secondCameraPoint;

    public void StartBoss()
    {
        StartCoroutine(BossCinematic());
    }

    private IEnumerator BossCinematic()
    {
        DialogueManager.Instance.IsCinematic();
        Controller.instance.FreezePlayer(true);
        GameManager.Instance.cameraController.ChangePoint(cameraPoint);
        yield return new WaitForSeconds(2f);
        bossIsland.SetActive(true);
        CameraShake.Instance.AddShakeEvent(itharAppearCameraShake);
        GameManager.Instance.VolumeTransition(itharAppearProfile, profileAnimationCurve);
        yield return new WaitForSeconds(5f);
        ithar.gameObject.SetActive(true);
        ithar.Play("ItharAppear");
        monolith.SetActive(true);
        yield return new WaitForSeconds(3f);
        GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint,true);
        Controller.instance.transform.position = portalFX.transform.position;
        yield return new WaitForSeconds(1f);
        portalFX.gameObject.SetActive(true);
        portalFX.Teleport();
        yield return new WaitForSeconds(1.5f);
        Controller.instance.transform.position = teleportationPoint.position;
        GameManager.Instance.cameraController.ChangePoint(secondCameraPoint);
        yield return new WaitForSeconds(4f);
        Controller.instance.FreezePlayer(true);
        DialogueManager.Instance.AssignDialogue(itharMeetKell.dialogue.ToList());
    }
}
