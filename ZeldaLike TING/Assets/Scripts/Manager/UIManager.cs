
using System;
using System.Collections.Generic;
using TMPro;
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
      public TextMeshProUGUI sideText;
      public CardsController.CardsState card;
      [HideInInspector]public Image image;
   }
   
   [Header("--- CARDS")] 
   [SerializeField] HandleRef[] cardHandlesReference;
   private Dictionary<Transform, HandleRef> cardsDictionary = new Dictionary<Transform, HandleRef>();
   private Transform[] cardHandles;
   private Color[] cardColor;
   public int cardUnlock = 1;
   private int currentCard = 0;
   float cardYPos;
   
   
   [Header("--- LIFE & STAT")] 
   [SerializeField] private Image[] lifeArray;

   
   private void Start()
   {
      initCardUI();
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

   void initCardUI()
   {
      cardHandles = new Transform[cardHandlesReference.Length];
      cardColor = new Color[cardHandlesReference.Length];
      
      foreach (HandleRef handle in cardHandlesReference)
      {
         cardsDictionary.Add(handle.Handle, handle);
      }
      
      foreach (var cardHandle in cardHandlesReference)
      {
         Transform CurrentHandle = cardHandle.Handle;
         int index = 0;
         switch (cardHandle.card)
         {
            case CardsController.CardsState.Fire: index = 0;
               break;
            
            case CardsController.CardsState.Ice:  index = 1;
               break;
            
            case CardsController.CardsState.Wall: index = 2;
               break;
            
            case CardsController.CardsState.Wind: index = 3;
               break;
               
         }
         cardHandles[index] = cardHandle.Handle;
         var currentHandleRef = cardsDictionary[cardHandles[index]];
         currentHandleRef.image = cardHandle.Handle.GetComponent<Image>();
      }
   }

   public void UpdateCardUI()
   {
      Debug.Log("Update Life");
      foreach (var cardHandle in cardHandlesReference)
      {
         switch (cardHandle.card)
         {
            case CardsController.CardsState.Fire :
               if (CardsController.instance.fireCardUnlock)
               {
                  cardHandle.Handle.gameObject.SetActive(true);
                  if (CardsController.instance.canUseFireCard)
                  {
                     cardHandle.image.color = cardsDictionary[cardHandle.Handle.transform].image.color;
                     cardHandle.sideText.text = "verso";
                  }
                  else
                  {
                     if (CardsController.instance.fireRectoUse && CardsController.instance.isFireGround)
                     {
                        cardHandle.sideText.text = "recto";
                        cardHandle.image.color = Color.grey;
                     }
                  }
               }
               break;
            
            case CardsController.CardsState.Ice :
               if (CardsController.instance.iceCardUnlock)
               {
                  cardHandle.Handle.gameObject.SetActive(true);
                  if (CardsController.instance.canUseIceCard)
                  {
                     cardHandle.image.color = cardsDictionary[cardHandle.Handle.transform].image.color;
                     cardHandle.sideText.text = "verso";
                  }
                  else
                  {
                     if (CardsController.instance.iceRectoUse)
                     {
                        cardHandle.sideText.text = "recto";
                        cardHandle.image.color = Color.grey;
                     }
                  }
               }
               break;
            
            case CardsController.CardsState.Wall :
               if (CardsController.instance.wallCardUnlock)
               {
                  cardHandle.Handle.gameObject.SetActive(true);
                  if (CardsController.instance.canUseWallCard) 
                  {
                     cardHandle.image.color = cardsDictionary[cardHandle.Handle.transform].image.color;
                     cardHandle.sideText.text = "verso";
                  }
                  else
                  {
                     if (CardsController.instance.wallRectoUse)
                     {
                        cardHandle.sideText.text = "recto";
                        cardHandle.image.color = Color.grey;
                     }
                  }
               }
               break;
            case CardsController.CardsState.Wind :
               if (CardsController.instance.windCardUnlock)
               {
                  cardHandle.Handle.gameObject.SetActive(true);
                  if (CardsController.instance.canUseWindCard)
                  {
                     cardHandle.image.color = cardsDictionary[cardHandle.Handle.transform].image.color;
                     cardHandle.sideText.text = "verso";
                  }
                  else
                  {
                     if (CardsController.instance.windRectoUse)
                     {
                        cardHandle.sideText.text = "recto";
                        cardHandle.image.color = Color.grey;
                     }
                  }
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

      CardsController.instance.State = cardsDictionary[cardHandles[currentCard]].card;
   }
   
}
