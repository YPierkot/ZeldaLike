using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class BossStart : MonoBehaviour
{
    [SerializeField] private GameObject bossIsland;
    [SerializeField] private GameObject monolith;
    [SerializeField] private Animator ithar;
    [SerializeField] private DialogueScriptable itharMeetKell;
    [SerializeField] private Animator portalFX;
    [SerializeField] private Transform teleportationPoint;
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private VolumeProfile itharAppearProfile;
    [SerializeField] private AnimationCurve profileAnimationCurve;
    [SerializeField] private CameraShakeScriptable itharAppearCameraShake;

    public void StartBoss()
    {
        StartCoroutine(BossCinematic());
    }

    private IEnumerator BossCinematic()
    {
        bossIsland.SetActive(true);
        DialogueManager.Instance.IsCinematic();
        Controller.instance.FreezePlayer(true);
        GameManager.Instance.cameraController.ChangePoint(cameraPoint);
        CameraShake.Instance.AddShakeEvent(itharAppearCameraShake);
        yield return new WaitForSeconds(2f);
        GameManager.Instance.VolumeTransition(itharAppearProfile, profileAnimationCurve);
        yield return new WaitForSeconds(3f);
        ithar.gameObject.SetActive(true);
        ithar.Play("ItharAppear");
        monolith.SetActive(true);
        yield return new WaitForSeconds(4f);
        GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint,true);
        portalFX.gameObject.SetActive(true);
        portalFX.SetTrigger("PortalOn");
        yield return new WaitForSeconds(1.5f);
        Controller.instance.transform.position = teleportationPoint.position;
    }
}
