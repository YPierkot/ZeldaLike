using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePlayerFireCard : MonoBehaviour
{
    private bool isCardGiven;
    private bool canGiveCard = false;

    private void Awake()
    {
        isCardGiven = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (!isCardGiven && canGiveCard && !DialogueManager.Instance.isPlayingDialogue)
            {
                CardsController.instance.canUseCards = true;
                CardsController.instance.fireCardUnlock = true;
                UIManager.Instance.UpdateCardUI();
                UIManager.Instance.ChangeCard(0);
                    isCardGiven = true;
            }
        }
    }

    public void ActivGetCard() => canGiveCard = true;
}