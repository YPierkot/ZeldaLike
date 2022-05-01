
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
         initCardUI();
      }
   #endregion

   [System.Serializable]
   class HandleRef
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
      cardYPos = cardHandles[0].transform.position.y;

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
        cardHandle.image = cardHandle.Handle.GetComponent<Image>();
        cardColor[index] = cardHandle.image.color;
      }
   }

   public void UpdateCardUI()
   {
      foreach (var cardHandle in cardHandlesReference)
      {
         switch (cardHandle.card)
         {
            case CardsController.CardsState.Fire :
               //Debug.Log($"FIRE Unlock: {CardsController.instance.fireCardUnlock}, canUse {CardsController.instance.canUseFireCard}, isRecto: {CardsController.instance.fireRectoUse}, isGround: {CardsController.isFireGround}");
               if (CardsController.instance.fireCardUnlock)
               {
                  cardHandle.Handle.gameObject.SetActive(true);
                  if(CardsController.instance.fireRectoUse)cardHandle.sideText.text = "recto";
                  else cardHandle.sideText.text = "verso";
                  
                  if (CardsController.instance.canUseFireCard)
                  {
                     cardHandle.image.color = cardColor[0];
                  }
                  else if (CardsController.instance.fireRectoUse || !CardsController.isFireGround) 
                  {
                     cardHandle.image.color = Color.grey;
                  }
                  
               }
               else cardHandle.Handle.gameObject.SetActive(false);
               break;
            
            case CardsController.CardsState.Ice :
               if (CardsController.instance.iceCardUnlock)
               {
                  if(CardsController.instance.iceRectoUse)cardHandle.sideText.text = "recto";
                  else cardHandle.sideText.text = "verso";
                  
                  cardHandle.Handle.gameObject.SetActive(true);
                  if (CardsController.instance.canUseIceCard)
                  {
                     cardHandle.image.color = cardColor[1];
                  }
                  else if (CardsController.instance.iceRectoUse || !CardsController.isIceGround)
                  {
                     cardHandle.image.color = Color.grey;
                  }
               }
               else cardHandle.Handle.gameObject.SetActive(false);
               break;
            
            case CardsController.CardsState.Wall :
               if (CardsController.instance.wallCardUnlock)
               {
                  cardHandle.Handle.gameObject.SetActive(true);
                  
                  if(CardsController.instance.wallRectoUse)cardHandle.sideText.text = "recto";
                  else cardHandle.sideText.text = "verso";
                  
                  if (CardsController.instance.canUseWallCard) 
                  {
                     cardHandle.image.color = cardColor[2];
                  }
                  else
                  {
                     if (CardsController.instance.wallRectoUse || !CardsController.isWallGround)
                     {
                        cardHandle.image.color = Color.grey;
                     }
                  }
               }
               else cardHandle.Handle.gameObject.SetActive(false);
               break;
            case CardsController.CardsState.Wind :
               if (CardsController.instance.windCardUnlock)
               {
                  cardHandle.Handle.gameObject.SetActive(true);
                  
                  if(CardsController.instance.windRectoUse)cardHandle.sideText.text = "recto";
                  else cardHandle.sideText.text = "verso";
                  
                  if (CardsController.instance.canUseWindCard)
                  {
                     cardHandle.image.color = cardColor[3];
                  }
                  else
                  {
                     if (CardsController.instance.windRectoUse || !CardsController.isWindGround)
                     {
                        cardHandle.image.color = Color.grey;
                     }
                  }
               }
               else cardHandle.Handle.gameObject.SetActive(false);
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
      //Debug.Log($"change card {changeInt}");
      if (changeInt != 0)
      {
         cardHandles[currentCard].position = new Vector3(cardHandles[currentCard].position.x, cardYPos,cardHandles[currentCard].position.z);

         Transform newCard = null;
         int antiWhile = 0;
         while (newCard == null)
         {
            currentCard += changeInt;
            if (currentCard == 4) currentCard = 0;
            else if (currentCard == -1) currentCard = 3;

            switch (cardsDictionary[cardHandles[currentCard]].card)
            {
               case CardsController.CardsState.Fire :
                  if (CardsController.instance.fireCardUnlock)
                  {
                     newCard = cardHandles[currentCard];
                  }
                  break;
               
               case CardsController.CardsState.Ice : 
                  if (CardsController.instance.iceCardUnlock)
                  {
                     newCard = cardHandles[currentCard];
                  }
                  break;
               
               case CardsController.CardsState.Wall : 
                  if (CardsController.instance.wallCardUnlock)
                  {
                     newCard = cardHandles[currentCard];
                  }
                  break;
               
               case CardsController.CardsState.Wind : 
                  if (CardsController.instance.windCardUnlock)
                  {
                     newCard = cardHandles[currentCard];
                  }
                  break;
            }

            antiWhile++;
            if (antiWhile == 5)
            {
               //Debug.Log("Anti-While break");
               break;
            }
         }
      }
      else
      {
         currentCard = 0;
      }
      
         cardHandles[currentCard].position = new Vector3(cardHandles[currentCard].position.x, cardYPos -55,cardHandles[currentCard].position.z);

         CardsController.instance.State = cardsDictionary[cardHandles[currentCard]].card;
   }
   
}
