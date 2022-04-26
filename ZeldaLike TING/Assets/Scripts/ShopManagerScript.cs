using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ShopManagerScript : MonoBehaviour
{
    #region ShopVariables
    //Variables
    public int ShopLevel = 1;
    public int coins;
    public int itemPriceLvl1 = 300;
    public int itemPriceLvl2 = 600;
    public int itemPriceLvl3 = 900;
    public bool isPlayerInRange;
    public bool isShopOpen;
    public GameObject[] iconsLvl1;
    public GameObject[] iconsLvl2;
    public GameObject[] iconsLvl3;

    //References
    public TextMeshProUGUI CoinsText;
    public GameObject ShopUI;
    private PlayerStat _controller;
    public static ShopManagerScript instance;
    private int moduleCost;

    #endregion

    private void Awake()
    {
        if (instance != null && instance != this) 
        {
            Destroy(this.gameObject);
        }
 
        instance = this;
    }

    private void Start()
    {
        _controller = Controller.instance.GetComponent<PlayerStat>();
        ShopUI.SetActive(false);
    }
    
    public void BuySharpnessModule()
    {
        int k = PlayerStat.instance.sharpnessModuleComposant;
        switch (ShopLevel)
        {
            case 1: 
                if (k == 1) moduleCost = itemPriceLvl1 / (1/3);
                else if (k == 2) moduleCost = itemPriceLvl1 / (2/3);
                else if (k == 3) moduleCost = 0;
                else moduleCost = itemPriceLvl1;
                break;
            case 2:
                if (k == 1) moduleCost = itemPriceLvl2 / (1/3);
                else if (k == 2) moduleCost = itemPriceLvl2 / (2/3);
                else if (k == 3) moduleCost = 0;
                else moduleCost = itemPriceLvl2;
                break;
            case 3:
                if (k == 1) moduleCost = itemPriceLvl3/ (1/3);
                else if (k == 2) moduleCost = itemPriceLvl3 / (2/3);
                else if (k == 3) moduleCost = 0;
                else moduleCost = itemPriceLvl3;
                break;
            default:
                Debug.Log("CECI EST UN BOG -- VERIF CODE");
                break;
        }

        if (coins > moduleCost) { _controller.UpgradeSharpness(ShopLevel); PlayerStat.instance.sharpnessModuleComposant -= k;}
        else Debug.Log("Fond insufissants");

        
        moduleCost = 0;
    }
    
    public void BuyLongswordModule()
    {
        
    }
    
    public void BuyKnockbackModule()
    {
        int k = PlayerStat.instance.sharpnessModuleComposant;
        switch (ShopLevel)
        {
            case 1: 
                if (k == 1) moduleCost = itemPriceLvl1 / (1/3);
                else if (k == 2) moduleCost = itemPriceLvl1 / (2/3);
                else if (k == 3) moduleCost = 0;
                else moduleCost = itemPriceLvl1;
                break;
            case 2:
                if (k == 1) moduleCost = itemPriceLvl2 / (1/3);
                else if (k == 2) moduleCost = itemPriceLvl2 / (2/3);
                else if (k == 3) moduleCost = 0;
                else moduleCost = itemPriceLvl2;
                break;
            case 3:
                if (k == 1) moduleCost = itemPriceLvl3/ (1/3);
                else if (k == 2) moduleCost = itemPriceLvl3 / (2/3);
                else if (k == 3) moduleCost = 0;
                else moduleCost = itemPriceLvl3;
                break;
            default:
                Debug.Log("CECI EST UN BOG -- VERIF CODE");
                break;
        }
        
        if (coins > moduleCost) { _controller.UpdateKB(ShopLevel); PlayerStat.instance.sharpnessModuleComposant -= k;}
        else Debug.Log("Fond insufissants");
    }
    
    public void BuyToughnessModule()
    {
        int k = PlayerStat.instance.sharpnessModuleComposant;
        switch (ShopLevel)
        {
            case 1: 
                if (k == 1) moduleCost = itemPriceLvl1 / (1/3);
                else if (k == 2) moduleCost = itemPriceLvl1 / (2/3);
                else if (k == 3) moduleCost = 0;
                else moduleCost = itemPriceLvl1;
                break;
            case 2:
                if (k == 1) moduleCost = itemPriceLvl2 / (1/3);
                else if (k == 2) moduleCost = itemPriceLvl2 / (2/3);
                else if (k == 3) moduleCost = 0;
                else moduleCost = itemPriceLvl2;
                break;
            case 3:
                if (k == 1) moduleCost = itemPriceLvl3/ (1/3);
                else if (k == 2) moduleCost = itemPriceLvl3 / (2/3);
                else if (k == 3) moduleCost = 0;
                else moduleCost = itemPriceLvl3;
                break;
            default:
                Debug.Log("CECI EST UN BOG -- VERIF CODE");
                break;
        }
        
        if (coins > moduleCost) {_controller.UpgradeToughness(ShopLevel); PlayerStat.instance.sharpnessModuleComposant -= k;}
        else Debug.Log("Fond insufissants");
    }
    
    public void BuyThornModule()
    {
        
    }
     
    public void BuyRockModule()
    {
        int k = PlayerStat.instance.sharpnessModuleComposant;
        switch (ShopLevel)
        {
            case 1: 
                if (k == 1) moduleCost = itemPriceLvl1 / (1/3);
                else if (k == 2) moduleCost = itemPriceLvl1 / (2/3);
                else if (k == 3) moduleCost = 0;
                else moduleCost = itemPriceLvl1;
                break;
            case 2:
                if (k == 1) moduleCost = itemPriceLvl2 / (1/3);
                else if (k == 2) moduleCost = itemPriceLvl2 / (2/3);
                else if (k == 3) moduleCost = 0;
                else moduleCost = itemPriceLvl2;
                break;
            case 3:
                if (k == 1) moduleCost = itemPriceLvl3/ (1/3);
                else if (k == 2) moduleCost = itemPriceLvl3 / (2/3);
                else if (k == 3) moduleCost = 0;
                else moduleCost = itemPriceLvl3;
                break;
            default:
                Debug.Log("CECI EST UN BOG -- VERIF CODE");
                break;
        }
        
        if (coins > moduleCost) {_controller.UpgradeRockness(ShopLevel); PlayerStat.instance.sharpnessModuleComposant -= k;}
        else Debug.Log("Fond insufissants");
    }
    
    public void BuySwiftnessModule()
    {
        _controller.UpgradeSwiftness(ShopLevel);
    }
    
    public void BuyStaminaModule()
    {
        
    }

    public void UpdateShopUI()
    {
        switch (ShopLevel)
        {
            case 1:
                for (int i = 0; i < iconsLvl1.Length; i++)
                {
                    iconsLvl1[i].SetActive(true);
                    iconsLvl2[i].SetActive(false);
                    iconsLvl3[i].SetActive(false);
                }
                break;
            case 2:
                for (int i = 0; i < iconsLvl1.Length; i++)
                {
                    iconsLvl1[i].SetActive(false);
                    iconsLvl2[i].SetActive(true);
                    iconsLvl3[i].SetActive(false);
                }
                break;
            case 3:
                for (int i = 0; i < iconsLvl1.Length; i++)
                {
                    iconsLvl1[i].SetActive(false);
                    iconsLvl2[i].SetActive(false);
                    iconsLvl3[i].SetActive(true);
                }
                break;
        }
    }
}
