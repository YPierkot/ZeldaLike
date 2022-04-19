using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    void Start()
    {
        Controller.instance.transform.position = transform.position;
    }
}
