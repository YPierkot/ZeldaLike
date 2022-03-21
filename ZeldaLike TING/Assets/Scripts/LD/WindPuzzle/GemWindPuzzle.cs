using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemWindPuzzle : MonoBehaviour
{
    private bool playerCollide;
    public MeshRenderer mesh;

    [HideInInspector] public GemWindManager gemManager;

    private void Start()
    {
        mesh.material.color = Color.red;
    }

    public void WindInteract()
    {
        if (playerCollide)
        {
            gemManager.UpdatePuzzle(this);
            mesh.material.color = Color.green;
            this.enabled = false;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.CompareTag("Player")) playerCollide = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player")) playerCollide = false;
    }



}
