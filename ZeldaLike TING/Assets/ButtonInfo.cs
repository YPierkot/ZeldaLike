using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonInfo : MonoBehaviour
{
    public int ItemID;
    public TextMeshProUGUI PriceText;
    public TextMeshProUGUI LevelText;
    public GameObject ShopManager;

    private void Start()
    {
        UpdateShopInfos();
    }

    public void UpdateShopInfos()
    {
        PriceText.text = "Price: " + ShopManager.GetComponent<ShopManagerScript>().shopItemps[2, ItemID].ToString();
        LevelText.text = "Module Level: " + ShopManager.GetComponent<ShopManagerScript>().shopItemps[3, ItemID].ToString();
    } 
}
