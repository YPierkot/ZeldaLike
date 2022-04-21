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
    [SerializeField] private bool isActivate;


    private void OnTriggerEnter(Collider other)
    {
            Debug.Log("collide Block");
        if (other.GetComponent<pushBlock>() != null)
        {
            switch (plateType)
            {
                case Element.Fire:
                    if (other.GetComponent<InteracteObject>().burning) isActivate = true;
                    else isActivate = false;
                    break;
            
                case Element.Ice :
                    if (other.GetComponent<InteracteObject>().isFreeze) isActivate = true;
                    else isActivate = false;
                    break;
            }
        }
    }
}
