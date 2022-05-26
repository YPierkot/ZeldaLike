
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
   [SerializeField] private CardUIScriptable cardSprites;
   [SerializeField] HandleRef[] cardHandlesReference;
   private Dictionary<Transform, HandleRef> cardsDictionary = new Dictionary<Transform, HandleRef>();
   private Transform[] cardHandles;
   public int cardUnlock = 1;
   private int currentCard = 0;
   float cardYPos;
   
   
   [Header("--- LIFE & STAT")] 
   [SerializeField] private Animator[] lifeArray;
   [SerializeField] private Animator KellHead;
   [Space]
   [SerializeField] private Animator[] dashHandles;
   private Color dashColor= new Color(.5f, .2f, .5f, 1);
   
   public GameObject loadingScreen;
   public TextMeshProUGUI playerLocation;

   
   
   private void Start()
   {
      cardYPos = cardHandles[0].transform.position.y;
      
      ChangeCard(0);
      UIManager.Instance.UpdateCardUI();

   }

   void initCardUI()
   {
      cardHandles = new Transform[cardHandlesReference.Length];
      
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
      }
   }

   public void UpdateCardUI()
   {
      //Debug.Log("Update Card");
      foreach (var cardHandle in cardHandlesReference)
      {
         switch (cardHandle.card)
         {
            case CardsController.CardsState.Fire :
               //Debug.Log($"FIRE Unlock: {CardsController.instance.fireCardUnlock}, canUse {CardsController.instance.canUseFireCard}, isRecto: {CardsController.instance.fireRectoUse}, isGround: {CardsController.isFireGround}");
               if (CardsController.instance.fireCardUnlock)
               {
                  cardHandle.Handle.gameObject.SetActive(true);
                  if(CardsController.instance.fireRectoUse)cardHandle.image.sprite = cardSprites.fireRecto;
                  else cardHandle.image.sprite = cardSprites.fireVerso;
                  
                  if (CardsController.instance.canUseFireCard)
                  {
                     cardHandle.image.color = Color.white;
                     cardHandle.image.sprite = cardSprites.fireVerso;
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
                  if (CardsController.instance.iceRectoUse) cardHandle.image.sprite = cardSprites.iceRecto;
                  else cardHandle.image.sprite = cardSprites.iceVerso;
                  
                  cardHandle.Handle.gameObject.SetActive(true);
                  if (CardsController.instance.canUseIceCard)
                  {
                     cardHandle.image.color = Color.white;
                     cardHandle.image.sprite = cardSprites.iceVerso;
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

                  if (CardsController.instance.wallRectoUse) cardHandle.image.sprite = cardSprites.groundRecto;
                  else cardHandle.image.sprite = cardSprites.groundVerso;
                  
                  if (CardsController.instance.canUseWallCard) 
                  {
                     cardHandle.image.color = Color.white;
                     cardHandle.image.sprite = cardSprites.groundVerso;
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

                  if (CardsController.instance.windRectoUse) cardHandle.image.sprite = cardSprites.windRecto;
                  else cardHandle.image.sprite = cardSprites.windVerso;
                  
                  if (CardsController.instance.canUseWindCard)
                  {
                     cardHandle.image.sprite = cardSprites.windVerso;
                     cardHandle.image.color = Color.white;
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

   public void InitLife(int life)
   {
      for (int i = 0; i < lifeArray.Length; i++)
      {
         if (i >= life)lifeArray[i].gameObject.SetActive(false);
         else lifeArray[i].gameObject.SetActive(true);
      }
   }

   public void TakeDamageUI(int life)
   {
      for (int i = 0; i < life; i++)
      {
         lifeArray[i].SetTrigger("TakeDamage");
      }
      Debug.Log($"destroy {lifeArray[life]}");
      lifeArray[life].SetTrigger("Destroy");
      KellHead.SetTrigger("TakeDamage");
   }

   public void UpdateDash(int dash = 3, bool add=false)
   {
      /*for (int i = 0; i < dashHandles.Length; i++)
      {
         if (i < dash) dashHandles[i].color = dashColor; 
         else dashHandles[i].color = new Color(.3f, .3f, .3f, .5f);
      }*/

      if (!add)
      {
         dashHandles[dash].SetTrigger("Dash");
      }
      else
      {
         dashHandles[dash - 1].SetTrigger("Recover");
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
      
         cardHandles[currentCard].position = new Vector3(cardHandles[currentCard].position.x, cardYPos +55,cardHandles[currentCard].position.z);

         CardsController.instance.State = cardsDictionary[cardHandles[currentCard]].card;
   }
   
}
