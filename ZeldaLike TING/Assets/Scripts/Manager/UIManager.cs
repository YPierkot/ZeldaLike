
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
   #region INSTANCE
      public static UIManager Instance;

      private void Awake()
      {
         Instance = this;
      }
   #endregion

   [System.Serializable]
   struct HandleRef
   {
      public Transform Handle;
      public CardsController.CardsState card;
   }
   
   [Header("--- CARDS")] 
   [SerializeField] HandleRef[] cardHandlesReference;
   private Dictionary<Transform, CardsController.CardsState> cardsDictionary = new Dictionary<Transform, CardsController.CardsState>();
   private Transform[] cardHandles;
   public int cardUnlock = 1;
   private int currentCard = 0;
   float cardYPos;
   
   
   [Header("--- LIFE & STAT")] 
   [SerializeField] private Image[] lifeArray;

   
   private void Start()
   {
      cardHandles = new Transform[cardHandlesReference.Length];
      foreach (HandleRef handle in cardHandlesReference)
      {
         cardsDictionary.Add(handle.Handle, handle.card);
      }


      UpdateCardUI();
      cardYPos = cardHandles[0].transform.position.y;
      Debug.Log(cardYPos);
      
      /*cardHandles = new Transform[cardHandlesContainer.childCount];
      for (int i = 0; i < cardHandlesContainer.childCount; i++)
      {
         cardHandles[i] = cardHandlesContainer.GetChild(i);
         if (i < cardUnlock)
         {
            cardHandles[i].gameObject.SetActive(true);
            Debug.Log("Active " + i);
         }
         else
         {
            cardHandles[i].gameObject.SetActive(false);
            Debug.Log("Desactive " + i);
         }
      }*/
      ChangeCard(0);

   }

   public void UpdateCardUI()
   {
      foreach (var cardHandle in cardHandlesReference)
      {
         switch (cardHandle.card)
         {
            case CardsController.CardsState.Fire :
               if (CardsController.instance.fireCardUnlock)
               {
                  cardHandles[0].gameObject.SetActive(true);
                  if (CardsController.instance.canUseFireCard);
               }
               break;
         }
         
         
         /*cardHandles[i] = cardHandlesReference[i].Handle;
         if (i < cardUnlock)
         {
            cardHandles[i].gameObject.SetActive(true);
         }
         else
         {
            cardHandles[i].gameObject.SetActive(false);
         }*/
      }
   }

   public void UpdateLife(int life)
   {
      for (int i = 0; i < lifeArray.Length; i++)
      {
         if (i >= life)lifeArray[i].enabled = false;
         else lifeArray[i].enabled = true;
      }
   }

   public void ChangeCard(int changeInt)
   {
      if (cardUnlock != 1)
      {
         cardHandles[currentCard].position = new Vector3(cardHandles[currentCard].position.x, cardYPos,cardHandles[currentCard].position.z);
      
         currentCard += changeInt;
         if (currentCard == cardUnlock) currentCard = 0;
         else if (currentCard == -1) currentCard = cardUnlock - 1;
      
         cardHandles[currentCard].position = new Vector3(cardHandles[currentCard].position.x, cardYPos -55,cardHandles[currentCard].position.z);
      }

      CardsController.instance.State = cardsDictionary[cardHandles[currentCard]];
   }
   
}
