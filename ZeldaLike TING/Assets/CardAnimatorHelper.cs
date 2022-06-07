using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimatorHelper : MonoBehaviour
{
    private float topPosY = 83f;
    private Vector3 basePosition;

    private void Awake()
    {
        basePosition = transform.parent.GetComponent<RectTransform>().localPosition;

    }


    public void SetBotPos()
    {
        transform.parent.GetComponent<RectTransform>().localPosition = basePosition;
    }
    
    public void SetTopPos()
    {
        transform.parent.GetComponent<RectTransform>().localPosition = new Vector3(basePosition.x, topPosY ,basePosition.z);
    }
}
