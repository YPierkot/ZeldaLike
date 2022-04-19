using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePlayerFireCard : MonoBehaviour
{
    private bool isCardGiven;

    private void Awake()
    {
        isCardGiven = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (!isCardGiven)
            {
                other.transform.GetComponent<CardsController>().canUseCards = true;
                Debug.Log(other.transform.GetComponent<UIManager>().cardUnlock);
                other.transform.GetComponent<UIManager>().cardUnlock = 1;
                Debug.Log(other.transform.GetComponent<UIManager>().cardUnlock);
                other.transform.GetComponent<UIManager>().UpdateCardUI();
                isCardGiven = true;
            }
        }
    }
}