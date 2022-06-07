using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;


public class GameManager : MonoBehaviour
{
    #region Instance

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public CameraController cameraController;
    public Volume volumeManager;
    public bool isTutorial = true;
    public Transform actualRespawnPoint;
    [SerializeField] private VolumeProfile tutorialWorld;
    [SerializeField] private Volume volumeTransition;
    private bool startTransition = true;
    private float startTime;
    private bool finished;
    private VolumeProfile transitionVolume;
    private AnimationCurve transitionCurve;
    private float curveLenght;
    public AnimationCurve cardTutorialCurve;
    public VolumeProfile tutorialTransition;
    private bool isLoop;
    public bool foundMonolith;
    public bool isDungeonFinished;
    public Animator mistMovement;
    public VolumeProfile dungeonProfile;
    public enum controller
    {
        Keybord,
        Xbox,
        ps
    }


    public controller currentContorller;

    public void DungeonProfile()
    {
        volumeManager.enabled = true;
        volumeManager.profile = dungeonProfile;
    }
    public void TutorialWorld()
    {
        volumeManager.enabled = true;
        volumeManager.profile = tutorialWorld;
    }

    public void VolumeTransition(VolumeProfile volume, AnimationCurve curve, bool loop = false)
    {
        startTime = 0;
        finished = false;
        curveLenght = curve[curve.length - 1].time;
        startTransition = false;
        volumeTransition.weight = 0;
        volumeTransition.profile = volume;
        transitionCurve = curve;
        isLoop = loop;
    }
    

    private void Update()
    {
        if (!startTransition)
        {
            if (!finished)
            {
                startTime += Time.deltaTime;
                volumeTransition.weight = transitionCurve.Evaluate(startTime);
            }
        
            if (startTime >= curveLenght)
            {
                switch (isLoop)
                {
                    case true :
                        finished = true;
                        startTransition = true;
                        //Debug.Log("je loop");
                        VolumeTransition(volumeTransition.profile, transitionCurve, true);
                        break;
                    
                    case false :
                        Debug.Log("Je ne loop pas");
                        finished = true;
                        startTransition = true;
                        volumeTransition.profile = null;
                        break;
                }
            }
        }
    }

    public void Disable(GameObject objectToDisable)
    {
        StartCoroutine(DisableObject(objectToDisable));
    }
    public IEnumerator DisableObject(GameObject _object)
    {
        _object.SetActive(false);
        yield return new WaitForSeconds(0.001f);
        _object.SetActive(true);
    }

    public void SetSpawnPoint(Transform newSpawnPoint)
    {
        actualRespawnPoint = newSpawnPoint;
        int actualMoneyAmount = PlayerStat.instance.money;
        int actualLifeMaxAmount = PlayerStat.instance.lifeMax;
        SaveManager.instance.SetSave(actualLifeMaxAmount, actualMoneyAmount, actualRespawnPoint);
    }
    
    
}