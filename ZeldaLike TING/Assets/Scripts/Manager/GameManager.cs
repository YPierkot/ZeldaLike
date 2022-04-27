using UnityEngine;

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
    public enum controller
    {
        Keybord, Xbox, ps
    }
    
    
    public controller currentContorller;
}
