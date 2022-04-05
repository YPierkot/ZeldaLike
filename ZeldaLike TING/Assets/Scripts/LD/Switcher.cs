using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Switcher : InteracteObject
{
    private MeshRenderer mesh;

    [SerializeField] private bool SwitchState;
    [Space] [SerializeField] private UnityEvent onEnable;
    [SerializeField] private UnityEvent onDisable;

    private bool playerInRange;

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        if (SwitchState) mesh.material.color = Color.green;
        else mesh.material.color = Color.red;
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Switch();
        }
    }

    public void Switch()
    {
        SwitchState = !SwitchState;
        if (SwitchState)
        {
            onEnable.Invoke();
            mesh.material.color = Color.green;
        }
        else
        {
            onDisable.Invoke();
            mesh.material.color = Color.red;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            mesh.material.color += new Color(60f/255,60f/255,0);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            mesh.material.color -= new Color(60f/255,60f/255,0);
        }
    }
}