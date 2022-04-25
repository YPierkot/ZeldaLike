using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    private List<ScenePartLoader> partsToLoad;

    

    private void OnTriggerEnter(Collider other)
    {
        NeverDestroy.Instance.SetSpawnPoint(this);
    }
}
