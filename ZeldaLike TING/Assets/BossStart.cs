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
    [SerializeField] private Transform thirdCameraPoint;
    [SerializeField] private VolumeProfile itharTension;
    [SerializeField] private AnimationCurve itharTensionCurve;
    [SerializeField] private CameraShakeScriptable itharAngry;
    [SerializeField] private Transform monolithCameraPoint;
    [SerializeField] private Transform islandsCameraPoint;
    [SerializeField] private CameraShakeScriptable itharScream;
    [SerializeField] private Animator itharHands;
    [SerializeField] private CameraShakeScriptable itharHandsShake;
    [SerializeField] private GameObject tornado;
    [SerializeField] private GameObject iceSpikes;
    [SerializeField] private GameObject bossManager;
    [SerializeField] private Material islandMaterial;
    [SerializeField] private MeshRenderer islandMesh;
    public void StartBoss()
    {
        StartCoroutine(BossCinematic());
    }

    private IEnumerator BossCinematic()
    {
        DialogueManager.Instance.IsCinematic(true);
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
        islandMesh.material = islandMaterial;
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
        GameManager.Instance.VolumeTransition(itharTension, itharTensionCurve);
        Controller.instance.FreezePlayer(true);
        Controller.instance.lastDir = new Vector3(1, 0, 0);
        GameManager.Instance.cameraController.ChangePoint(thirdCameraPoint);
        GameManager.Instance.mistMovement.Play("MistMovement");
        DialogueManager.Instance.AssignDialogue(itharMeetKell.dialogue.ToList());
        yield return new WaitForSeconds(19f);
        CameraShake.Instance.AddShakeEvent(itharAngry);
        yield return new WaitForSeconds(11f);
        GameManager.Instance.cameraController.ChangePoint(monolithCameraPoint);
        yield return new WaitForSeconds(4f);
        GameManager.Instance.cameraController.ChangePoint(islandsCameraPoint);
        yield return new WaitForSeconds(6.5f);
        GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
        yield return new WaitForSeconds(3.5f);
        GameManager.Instance.cameraController.ChangePoint(thirdCameraPoint);
        CameraShake.Instance.AddShakeEvent(itharScream);
        yield return new WaitForSeconds(7f);
        CameraShake.Instance.AddShakeEvent(itharHandsShake);
        ithar.Play("ItharPower");
        yield return new WaitForSeconds(1f);
        itharHands.gameObject.SetActive(true);
        GameManager.Instance.VolumeTransition(itharTension, itharTensionCurve);
        yield return new WaitForSeconds(9f);
        itharHands.GetComponent<SpriteRenderer>().enabled = true;
        ithar.GetComponent<SpriteRenderer>().enabled = false;
        itharHands.Play("Boss_BoolCast");
        GameManager.Instance.cameraController.ChangePoint(secondCameraPoint);
        SoundManager.Instance.musicSource.clip = SoundManager.Instance.bossMusic;
        SoundManager.Instance.musicSource.Play();
        yield return new WaitForSeconds(0.5f);
        CameraShake.Instance.AddShakeEvent(itharScream);
        tornado.SetActive(true);
        iceSpikes.SetActive(true);
        itharHands.Play("Boss_Idle");
        yield return new WaitForSeconds(7f);
        tornado.SetActive(false);
        iceSpikes.SetActive(false);
        itharHands.Play("Boss_TP");
        yield return new WaitForSeconds(0.3f);
        ithar.gameObject.SetActive(false);
        itharHands.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        DialogueManager.Instance.IsCinematic(false);
        GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
        bossManager.SetActive(true);
        this.enabled = false;
        
    }
}
