using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class RunePlate : MonoBehaviour {
    private enum Element {
        Fire,
        Ice,
        Ground,
        Wind
    }

    [SerializeField] private RunePuzzleManager runeManager = null;
    [Space]
    [SerializeField] private Element plateType;

    [SerializeField] private bool isActivate;
    [SerializeField] private UnityEvent onActivation;
    [SerializeField] private UnityEvent onDeactivation;
    private MeshRenderer mesh;
    [ColorUsage(true, true)]
    [SerializeField] private Color fireColor;
    [ColorUsage(true, true)]
    [SerializeField] private Color iceColor;
    
    public bool IsActivate {
        get => isActivate;
        set => isActivate = value;
    }
    
    private void Start()
    {
        if (GetComponentInParent<RunePuzzleManager>())
        {
            runeManager = GetComponentInParent<RunePuzzleManager>();
        }
        if (runeManager == null) {
            Debug.LogError("There is no rune manager on this object. Please add one before testing the puzzle.", this.transform);
            return;
        }

        mesh = GetComponent<MeshRenderer>();
        runeManager.runesList.Add(this);
    }

    private void OnTriggerStay(Collider other) 
    {
        //Debug.Log("ok");
        if (other.GetComponent<pushBlock>() != null) {
            if (runeManager == null) return;
            

            switch (plateType) {
                case Element.Fire:
                    if (other.GetComponent<InteracteObject>().burning) {
                        if (!isActivate) {
                            isActivate = true;
                            onActivation.Invoke();
                            runeManager.CheckRunes();
                        }
                    }
                    else {
                        isActivate = false;
                        onDeactivation.Invoke();
                    }

                    break;

                case Element.Ice:
                    if (other.GetComponent<InteracteObject>().isFreeze) {
                        if (!isActivate) {
                            onActivation.Invoke();
                            isActivate = true;
                            runeManager.CheckRunes();
                        }
                    }
                    else {
                        if (isActivate)
                        {
                            isActivate = false;
                            onDeactivation.Invoke();
                        }
                    }

                    break;
            }
        }
    }

    public void ActivateRune()
    {
        switch (plateType)
        {
            case Element.Fire: 
                mesh.material.SetColor("_Emission_Teinte", fireColor);
                break;
            case Element.Ice :
                mesh.material.SetColor("_Emission_Teinte", iceColor);
                break;
        }
        
    }

    public void DeactivateRune()
    {
        switch (plateType)
        {
            case Element.Fire: 
                mesh.material.SetColor("_Emission_Teinte", Color.grey);
                break;
            case Element.Ice :
                mesh.material.SetColor("_Emission_Teinte", Color.black);
                break;
        }
    }
}