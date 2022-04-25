using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantScript : MonoBehaviour
{
    [SerializeField] bool isPlayerInRange;
    [SerializeField] private bool isShopOpen;
    [SerializeField] private GameObject ShopUI;


    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F) && !isShopOpen)
        {
            isShopOpen = true;
            ShopUI.SetActive(true);
            ShopManagerScript.instance.UpdateShopUI();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isPlayerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isPlayerInRange = false;

        if (isShopOpen)
        {
            isShopOpen = false;
            ShopUI.SetActive(false);
        }
    }
}
