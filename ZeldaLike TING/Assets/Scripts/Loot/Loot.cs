using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public enum LootType
    {
        Coin, HalfHeart, Hearth, ModulePiece
    }

    [SerializeField] public LootType lootType;
    [SerializeField] private int amount;
    [SerializeField] moduleScriptableUI.moduleType _moduleType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Le player me rentre dedans");
            switch (lootType)
            {
                case LootType.Coin : PlayerStat.instance.ChangeMoney(amount);
                    break;
                
                case LootType.HalfHeart : PlayerStat.instance.TakeDamage(-1);
                    break;
                
                case  LootType.Hearth : PlayerStat.instance.TakeDamage(-2);
                    break;

                case LootType.ModulePiece :
                    switch (_moduleType)
                    {
                        case moduleScriptableUI.moduleType.Sharpness : PlayerStat.instance.sharpnessModuleComposant++;
                            break;
                        
                        case moduleScriptableUI.moduleType.Rock : PlayerStat.instance.rockModuleComposant++;
                            break;

                        case moduleScriptableUI.moduleType.Swiftness : PlayerStat.instance.swiftnessModuleComposant++;
                            break;

                        case moduleScriptableUI.moduleType.Thorn : PlayerStat.instance.thornModuleComposant++;
                            break;

                        case moduleScriptableUI.moduleType.Toughness : PlayerStat.instance.toughnessModuleComposant++;
                            break;

                        case moduleScriptableUI.moduleType.LongSword : PlayerStat.instance.longSwordModuleComposant++;
                            break;
                    }
                    break;
            }
            Destroy(gameObject);
        }
    }
}
