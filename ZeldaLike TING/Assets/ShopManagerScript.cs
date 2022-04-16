using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class ShopManagerScript : MonoBehaviour
{
    public int[,] shopItemps = new int[5, 8];
    private int nbItems = 8;
    public float coins;
    public TextMeshProUGUI CoinsText;
    
    
    void Start()
    {
        CoinsText.text = $"Coins: {coins.ToString()}";
        
        
        for (int i = 1; i < nbItems +1; i++)
        {
            //shopItemps[1, i] = i;     // Setup item's id
            //shopItemps[2, i] = 300;   // Setup item's price
            //shopItemps[3, i] = 1;  // Setup item's level
            //Debug.Log($"{i}ème item ");
        }
    }

    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if (coins >= shopItemps[2, ButtonRef.GetComponent<ButtonInfo>().ItemID])
        {
            coins -= shopItemps[2, ButtonRef.GetComponent<ButtonInfo>().ItemID];
            CoinsText.text = $"Coins: {coins.ToString()}";

            if ( shopItemps[3, ButtonRef.GetComponent<ButtonInfo>().ItemID] < 3) // Niveau max étant 3
            {
                shopItemps[3, ButtonRef.GetComponent<ButtonInfo>().ItemID]++; // Augementation de level d'objet
                shopItemps[2, ButtonRef.GetComponent<ButtonInfo>().ItemID] += 400; // Augementation du prix
                ButtonRef.GetComponent<ButtonInfo>().UpdateShopInfos();
            }
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }
}
