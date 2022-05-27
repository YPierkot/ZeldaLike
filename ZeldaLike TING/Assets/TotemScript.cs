using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemScript : MonoBehaviour
{
    public Card cardToTake;
    
    [System.Serializable]
    
    public enum Card
    {
        Fire, Wind
    }

    private void OnTriggerEnter(Collider other)
    {
        Controller.instance.playerInteraction = TakeCard;
    }

    private void OnTriggerExit(Collider other)
    {
        if (Controller.instance.playerInteraction == TakeCard)
        {
            Controller.instance.playerInteraction = null;
        }
    }

    private void TakeCard()
    {
        switch (cardToTake)
        {
            case Card.Fire:
                CardsController.instance.fireCardUnlock = false;
                CardsController.instance.canUseFireCard = false;
                UIManager.Instance.UpdateCardUI();
                break;
            case Card.Wind : 
                CardsController.instance.windCardUnlock = false;
                CardsController.instance.canUseWindCard = false;
                UIManager.Instance.UpdateCardUI();
                break;
        }
    }
}
