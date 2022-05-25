using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    public bool IsActivate {
        get => isActivate;
        set => isActivate = value;
    }
    
    private void Start()
    {
        if (GetComponentInParent<RunePuzzleManager>()) runeManager = GetComponentInParent<RunePuzzleManager>();
        if (runeManager == null) {
            Debug.LogError("There is no rune manager on this object. Please add one before testing the puzzle.", this.transform);
            return;
        }
        
        runeManager.runesList.Add(this);
    }

    private void OnTriggerStay(Collider other) {
        if (other.GetComponent<pushBlock>() != null) {
            if (runeManager == null) return;
            Debug.Log("ok");

            switch (plateType) {
                case Element.Fire:
                    if (other.GetComponent<InteracteObject>().burning) {
                        if (!isActivate) {
                            GetComponent<MeshRenderer>().material.color = GetComponent<MeshRenderer>().material.color + new Color(.2f, .2f, .2f);
                            isActivate = true;
                            runeManager.CheckRunes();
                        }
                    }
                    else {
                        if (isActivate) GetComponent<MeshRenderer>().material.color = GetComponent<MeshRenderer>().material.color - new Color(.2f, .2f, .2f);
                        isActivate = false;
                    }

                    break;

                case Element.Ice:
                    if (other.GetComponent<InteracteObject>().isFreeze) {
                        if (!isActivate) {
                            GetComponent<MeshRenderer>().material.color = GetComponent<MeshRenderer>().material.color + new Color(.2f, .2f, .2f);
                            isActivate = true;
                            runeManager.CheckRunes();
                        }
                    }
                    else {
                        if (isActivate) GetComponent<MeshRenderer>().material.color = GetComponent<MeshRenderer>().material.color - new Color(.2f, .2f, .2f);
                        isActivate = false;
                    }

                    break;
            }
        }
    }
}