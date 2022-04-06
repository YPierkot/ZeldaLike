using UnityEngine;

public class TargetPuzzleManager : MonoBehaviour
{
    [SerializeField] private InteracteObject[] doors;
    private int openDoor;

    public void OpenDoor()
    {
        Debug.Log("Open Door");
        doors[openDoor].OnFireEffect();
        openDoor++;
    }
}
