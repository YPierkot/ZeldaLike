using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePlayerSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Controller.instance.transform.position = transform.position;
    }
}
