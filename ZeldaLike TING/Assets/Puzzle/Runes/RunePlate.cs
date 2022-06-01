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

    private void OnTriggerStay(Collider other) 
    {
        Debug.Log("ok");
        if (other.GetComponent<pushBlock>() != null) {
            if (runeManager == null) return;
            

            switch (plateType) {
                case Element.Fire:
                    if (other.GetComponent<InteracteObject>().burning) {
                        if (!isActivate) {
                            GetComponent<MeshRenderer>().material.color = GetComponent<MeshRenderer>().material.color + new Color(.2f, .2f, .2f);
                            isActivate = true;
                            SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.runeActivation);
                            onActivation.Invoke();
                            runeManager.CheckRunes();
                        }
                    }
                    else {
                        if (isActivate) GetComponent<MeshRenderer>().material.color = GetComponent<MeshRenderer>().material.color - new Color(.2f, .2f, .2f);
                        isActivate = false;
                        onDeactivation.Invoke();
                    }

                    break;

                case Element.Ice:
                    if (other.GetComponent<InteracteObject>().isFreeze) {
                        if (!isActivate) {
                            GetComponent<MeshRenderer>().material.color = GetComponent<MeshRenderer>().material.color + new Color(.2f, .2f, .2f);
                            onActivation.Invoke();
                            isActivate = true;
                            SoundEffectManager.Instance.PlaySound(SoundEffectManager.Instance.sounds.runeActivation);
                            runeManager.CheckRunes();
                        }
                    }
                    else {
                        if (isActivate) GetComponent<MeshRenderer>().material.color = GetComponent<MeshRenderer>().material.color - new Color(.2f, .2f, .2f);
                        isActivate = false;
                        onDeactivation.Invoke();
                    }

                    break;
            }
        }
    }
}