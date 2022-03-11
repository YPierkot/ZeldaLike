using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    private UnityEvent activateEvent;
    private MeshRenderer mesh;

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }
    
    
    void OnTriggerEnter()
    {
        mesh.material.color = Color.yellow;
    }

    
    
    void OnTriggerExit()
    {
        mesh.material.color = Color.white;
    }
}
