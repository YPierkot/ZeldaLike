using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShopManager : MonoBehaviour
{
    [Serializable]
    class BuyableModule
    {
        public Image button;
        public TextMeshProUGUI name;
        public TextMeshProUGUI price;
        public bool isSelected;
    }
    
    [SerializeField] private Transform buyablePanel;
    [SerializeField] private Transform equippedPanel;
    private BuyableModule[] buyableModules = new BuyableModule[3];
    private Image[] equippedModules = new Image[3];
    [SerializeField] private moduleScriptableUI[] buyModules = new moduleScriptableUI[3];
    [SerializeField] private moduleScriptableUI[] modulesList;
    private moduleScriptableUI[] _moduleList;

    private moduleScriptableUI switchPendingModule;

    private void Start()
    {
        InitModuleRef();
        for (int i = 0; i < 3; i++)
        {
            Transform currentModule = buyablePanel.GetChild(i);
            buyableModules[i] = new BuyableModule();
            buyableModules[i].button = currentModule.GetComponent<Image>();
            buyableModules[i].name = currentModule.GetChild(0).GetComponent<TextMeshProUGUI>();
            buyableModules[i].price = currentModule.GetChild(1).GetComponent<TextMeshProUGUI>();

            equippedModules[i] = equippedPanel.GetChild(i).GetComponent<Image>();
        }

        ActualiseShop(1);
        
    }

    public void InitModuleRef()
    {
        _moduleList = new moduleScriptableUI[((Enum.GetNames(typeof(moduleScriptableUI.moduleType)).Length) * 3)];
        foreach (var module in modulesList)
        {
            int i = module.moduleLevel + ((int)module._moduleType*3);
            _moduleList[i - 1] = module;
        }
    }

    void ActualiseShop(int shopLevel)
    {
        List<moduleScriptableUI> moduleAvailable = new List<moduleScriptableUI>();
        for (int i = 0; i < (Enum.GetNames(typeof(moduleScriptableUI.moduleType)).Length); i++)
        {
            int index = (shopLevel - 1) + i*3;
            moduleAvailable.Add(_moduleList[index]);
        }
        for (int i = 0; i < PlayerStat.instance.equippedModules.Length; i++)
        {
            if (moduleAvailable.Contains(PlayerStat.instance.equippedModules[i])) moduleAvailable.Remove(PlayerStat.instance.equippedModules[i]);
        }

        for (int i = 0; i < buyModules.Length; i++)
        {
            int ind = Random.Range(0, moduleAvailable.Count); 
            buyModules[i] = moduleAvailable[ind]; 
            moduleAvailable.RemoveAt(ind);
        }
        UpdateModules();
    }

    moduleScriptableUI GetRandomModule(int level)
    {
        int rdm = Random.Range(0, Enum.GetNames(typeof(moduleScriptableUI.moduleType)).Length);
        int i = level-1 + (rdm*3);
        return _moduleList[i];
    }

    public void UpdateModules()
    {
        for (int i = 0; i < 3; i++)
        {
            if (buyModules[i] != null)
            {
                buyableModules[i].button.gameObject.SetActive(true);
                buyableModules[i].button.sprite = buyModules[i].moduleSprite;
                buyableModules[i].name.text = $"{buyModules[i]._moduleType.ToString()} {buyModules[i].moduleLevel}";
                buyableModules[i].price.text = GetPrice(buyModules[i]).ToString();
            }
            else buyableModules[i].button.gameObject.SetActive(false);

        }

        moduleScriptableUI[] equipModules = PlayerStat.instance.equippedModules;

        
        for (int i = 0; i < equippedModules.Length; i++)
        {
            if(i >= equipModules.Length)equippedModules[i].gameObject.SetActive(false);
            else
            {
                Debug.Log("equippedModules no null");
                if (equipModules[i] != null)
                {
                    equippedModules[i].sprite = equipModules[i].moduleSprite;
                    equippedModules[i].gameObject.SetActive(true);
                }
                else equippedModules[i].gameObject.SetActive(false);
            }
        }
    }

    int GetPrice(moduleScriptableUI module)
    {
        switch (module.moduleLevel)
        {
            case 1 :
                return 50;
                
            case 2 :
                return 100 ;

            case 3 :
                return 200 ;
            
            default:
                return 0;

        }
    }

    public void BuyModule(int slot)
    {
        if (PlayerStat.instance.money >= GetPrice(buyModules[slot-1]))
        {
            for (int i = 0; i < 3; i++)
            {
                if (PlayerStat.instance.equippedModules[i] == null)
                {
                    PlayerStat.instance.equippedModules[i] = buyModules[slot - 1];
                    LaunchUpgrade(buyModules[slot-1]._moduleType, buyModules[slot-1].moduleLevel);
                    PlayerStat.instance.ChangeMoney(- GetPrice(buyModules[slot - 1]) );
                    buyModules[slot - 1] = null;
                    UpdateModules();
                    return;
                }
            }
            buyableModules[slot-1].button.color = Color.grey;
            SwitchModuleEquip(buyModules[slot-1]);
            return;
        }
        else Debug.Log("Can't buy");
    }

    void SwitchModuleEquip(moduleScriptableUI pendingModule)
    {
        switchPendingModule = pendingModule;
        for (int i = 0; i < equippedModules.Length; i++)
        {
            Debug.Log(equippedModules[i].name + " button: " + equippedModules[i].transform.GetComponent<Button>().name);
            equippedModules[i].GetComponent<Button>().enabled = true;
        }
    }

    public void SwitchModule(int slot)
    {
        LaunchUpgrade(PlayerStat.instance.equippedModules[slot-1]._moduleType, 0);
        LaunchUpgrade(switchPendingModule._moduleType, switchPendingModule.moduleLevel);
        PlayerStat.instance.equippedModules[slot - 1] = switchPendingModule;
        switchPendingModule = null;
        for (int i = 0; i < equippedModules.Length; i++)
        {
            equippedModules[i].GetComponent<Button>().enabled = false;
        }
        PlayerStat.instance.ChangeMoney(- GetPrice(buyModules[slot - 1]) );
        foreach (var module in buyableModules)
        {
            module.button.color = Color.white;
        }
    }

    void LaunchUpgrade(moduleScriptableUI.moduleType type, int level)
    {
        switch (type)
        {
            case moduleScriptableUI.moduleType.Rock:
                PlayerStat.instance.UpgradeRockness(level); 
                break;
            
            case moduleScriptableUI.moduleType.Sharpness:
                PlayerStat.instance.UpgradeSharpness(level); 
                break;

            case moduleScriptableUI.moduleType.Swiftness:
                PlayerStat.instance.UpgradeSwiftness(level); 
                break;

            case moduleScriptableUI.moduleType.Thorn:
                PlayerStat.instance.UpdgradeThorn(level); 
                break;

            case moduleScriptableUI.moduleType.Toughness:
                PlayerStat.instance.UpgradeToughness(level); 
                break;

            case moduleScriptableUI.moduleType.LongSword:
                PlayerStat.instance.UpgradeLongSword(level); 
                break;
        } 
    }
}
