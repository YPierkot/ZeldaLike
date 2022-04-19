
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
   
   
   [Header("--- LIFE & STAT")] 
   [SerializeField] private Image[] lifeArray;


   public GameObject MerchandInterfaceInUIManager;
      
   private void Start()
   {
      cardHandles = new Transform[cardHandlesReference.Length];
      foreach (HandleRef handle in cardHandlesReference)
      {
         cardsDictionary.Add(handle.Handle, handle.card);
         
      }

      UpdateCardUI();
      
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
         cardHandles[currentCard].localScale = new Vector3(0.8f,0.8f,0.8f);
      
         currentCard += changeInt;
         if (currentCard == cardUnlock) currentCard = 0;
         else if (currentCard == -1) currentCard = cardUnlock - 1;
      
         cardHandles[currentCard].localScale = new Vector3(1.2f,1.2f,1.2f);
      }

      CardsController.instance.State = cardsDictionary[cardHandles[currentCard]];
   }

   public void UpdateCardUI()
   {
      for (int i = 0; i < cardHandlesReference.Length; i++)
      {
         cardHandles[i] = cardHandlesReference[i].Handle;
         if (i < cardUnlock)
         {
            cardHandles[i].gameObject.SetActive(true);
         }
         else
         {
            cardHandles[i].gameObject.SetActive(false);
         }
      }
   }
}
