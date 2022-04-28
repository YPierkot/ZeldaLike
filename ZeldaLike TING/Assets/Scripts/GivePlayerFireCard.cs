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
                CardsController.instance.canUseCards = true;
                CardsController.instance.fireCardUnlock = true;
                UIManager.Instance.UpdateCardUI();
                    isCardGiven = true;
            }
        }
    }
}