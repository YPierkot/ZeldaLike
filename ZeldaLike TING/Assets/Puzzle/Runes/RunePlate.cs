using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RunePlate : MonoBehaviour
{
    public enum Element
    {
        Fire, Ice, Ground, Wind
    }

    [SerializeField] private Element plateType;
    public bool isActivate;

    private void Start()
    {
        RunePuzzleManager.Instance.runesList.Add(this);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<pushBlock>() != null)
        {
            switch (plateType)
            {
                case Element.Fire:
                    if (other.GetComponent<InteracteObject>().burning)
                    {
                        if (!isActivate)
                        {
                            GetComponent<MeshRenderer>().material.color = GetComponent<MeshRenderer>().material.color + new Color(.2f, .2f, .2f);
                            isActivate = true;
                            RunePuzzleManager.Instance.CheckRunes();
                        }
                    }
                    else
                    {
                        if (isActivate) GetComponent<MeshRenderer>().material.color = GetComponent<MeshRenderer>().material.color - new Color(.2f, .2f, .2f);
                        isActivate = false;
                    }
                    break;
            
                case Element.Ice :
                    if (other.GetComponent<InteracteObject>().isFreeze)
                    {
                        if (!isActivate)
                        {
                            GetComponent<MeshRenderer>().material.color = GetComponent<MeshRenderer>().material.color + new Color(.2f, .2f, .2f);
                            isActivate = true;
                            RunePuzzleManager.Instance.CheckRunes();
                        }
                    }
                    else
                    {
                        if (isActivate) GetComponent<MeshRenderer>().material.color = GetComponent<MeshRenderer>().material.color - new Color(.2f, .2f, .2f);
                        isActivate = false;
                    }
                    break;
            }
        }
    }
}
