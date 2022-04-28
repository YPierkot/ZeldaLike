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
    public enum controller
    {
        Keybord, Xbox, ps
    }
    
    
    public controller currentContorller;

    public void TutorialWorld()
    {
        volumeManager.enabled = true;
    }
}
