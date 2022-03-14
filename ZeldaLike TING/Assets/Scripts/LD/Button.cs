using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Users;

public class Button : MonoBehaviour
{
    [SerializeField] private UnityEvent activateEvent;
    private MeshRenderer mesh;

    private bool inRange;

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            activateEvent.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mesh.material.color = Color.yellow;
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mesh.material.color = Color.white;
            inRange = false;
        }
    }
}
