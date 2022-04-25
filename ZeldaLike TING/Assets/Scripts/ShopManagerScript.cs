using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class ShopManagerScript : MonoBehaviour
{
    #region ShopVariables
    //Variables
    public int ShopLevel = 1;
    public float coins;
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
        _controller.UpgradeSharpness(ShopLevel);
    }
    
    public void BuyLongswordModule()
    {
        
    }
    
    public void BuyKnockbackModule()
    {
        _controller.UpdateKB(ShopLevel);
    }
    
    public void BuyToughnessModule()
    {
        _controller.UpgradeToughness(ShopLevel);
    }
    
    public void BuyThornModule()
    {
        
    }
     
    public void BuyRockModule()
    {
        _controller.UpgradeRockness(ShopLevel);
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
