using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dropper : MonoBehaviour
{
    [Serializable]
    class DropSettings
    {
        public GameObject Item;
        public float dropRate;
    }

    [SerializeField] private DropSettings[] Loots;
    public bool lootItem = true;
    
    private void OnDestroy()
    {
        if (lootItem)
        {
            float rate = Random.Range(0f, 100f);
            foreach (var loot in Loots)
            {
                rate -= loot.dropRate;
                if (rate <= 0)
                {
                    Instantiate(loot.Item);
                    break;
                }
            }
        }
    }
}