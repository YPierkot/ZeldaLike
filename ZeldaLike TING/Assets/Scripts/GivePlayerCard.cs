using UnityEngine;

public class GivePlayerCard : MonoBehaviour
{
    private bool isCardGiven;
    private bool canGiveCard = true;
    [SerializeField] private WindCardTutorialManager WindCardTutorialManager;
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
    }

    public void ActivGetCard() => canGiveCard = true;
}