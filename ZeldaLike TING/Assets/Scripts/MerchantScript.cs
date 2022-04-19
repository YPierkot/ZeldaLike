using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantScript : MonoBehaviour
{
    public NPCDialogue NpcDialogue;
    public static GameObject MerchandInterface;
    private bool isInterfaceOpen;
    
    private enum UpgradeType
    {
        Sharpness = 0, LongSword, Knockback, Toughness, Thorn, Rock, Swiftness, Stamina
    }

    private UpgradeType State = UpgradeType.Sharpness;
    
    private void Awake()
    {
        NpcDialogue = GetComponent<NPCDialogue>();
        MerchandInterface = UIManager.Instance.MerchandInterfaceInUIManager;
        //MerchandInterface.SetActive(false);
    }
    
    private void Update()
    {
        if (NpcDialogue.playerIn && Input.GetKeyDown(KeyCode.R))
        {
            OpenMerchantInterface();
        }
    }


    private void OpenMerchantInterface()
    {
        if (!isInterfaceOpen)
        {
            MerchandInterface.SetActive(true);
        }
    }

    
    private void BuyModuleUpgrade(UpgradeType choosedStateToUpgrade)
    {
        switch(choosedStateToUpgrade)
        {
            case UpgradeType.Sharpness: break;
            case UpgradeType.LongSword:  break;
            case UpgradeType.Knockback:  ; break;
            case UpgradeType.Toughness: ; break;
            case UpgradeType.Thorn: ; break;
            case UpgradeType.Rock: ; break;
            case UpgradeType.Swiftness: ; break;
            case UpgradeType.Stamina: ; break;
        }
    }

    private void UpgradeSharpness()
    {
        
    }
    
}
