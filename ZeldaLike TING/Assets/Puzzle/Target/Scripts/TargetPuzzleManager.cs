using UnityEngine;

public class TargetPuzzleManager : MonoBehaviour
{
    [SerializeField] private Door[] doors;
    private int openDoor;

    private void OpenDoor()
    {
        doors[openDoor].SwitchState();
        openDoor++;
    }
}
