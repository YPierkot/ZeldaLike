using System;
using UnityEngine;

public class GivePlayerCard : MonoBehaviour
{
    private bool isCardGiven;
    private bool canGiveCard = true;
    [SerializeField] private WindCardTutorialManager WindCardTutorialManager;
    [SerializeField] private Transform glow;
    private bool playerIn;
    
    public enum CardToGive
    {
        fire, wind, wall, ice
    }

    [SerializeField] private CardToGive _cardToGive;

    private void Awake()
    {
        isCardGiven = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && Input.GetKeyDown(KeyCode.E));
        {
            playerIn = true;
            Controller.instance.playerInteraction = GiveCard;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIn = false;
            if (Controller.instance.playerInteraction == GiveCard)
            {
                Controller.instance.playerInteraction = null;
            }
        }
    }

    public void GiveCard()
    {
        if (!isCardGiven && canGiveCard && !DialogueManager.Instance.isPlayingDialogue)
        {
            Debug.Log("je donne la carte " + _cardToGive);
            CardsController.instance.canUseCards = true;
            switch (_cardToGive)
            {
                case CardToGive.fire :
                    CardsController.instance.fireCardUnlock = true;
                    break;
                case CardToGive.ice :
                    CardsController.instance.iceCardUnlock = true;
                    break;
                case CardToGive.wall :
                    CardsController.instance.wallCardUnlock = true;
                    break;
                case CardToGive.wind :
                    WindCardTutorialManager.canStart = true;
                    CardsController.instance.windCardUnlock = true;
                    break;
            }
            UIManager.Instance.UpdateCardUI();
            UIManager.Instance.ChangeCard(0);
            isCardGiven = true;
        }
    }

    
    private void Update()
    {
        
        if (isCardGiven && glow.localScale.x <= 20)
        {
            glow.localScale += Vector3.one;
        }

        if (glow.localScale.x >= 20)
        {
            glow.gameObject.SetActive(false);
            transform.localScale -= Vector3.one/10;
        }

        if (transform.localScale.y <= 0.5f)
        {
            gameObject.SetActive(false);
        }
    }

    public void ActivGetCard() => canGiveCard = true;
}