using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dropper : MonoBehaviour
{
    [Serializable]
    public class DropSettings
    {
        public GameObject Item;
        public float dropRate;
    }

    public DropSettings[] Loots;
    public bool lootItem = true;
    [Space] [SerializeField] private Vector3 offset;
    
    public void Loot()
    {
        if (lootItem)
        {
            float rate = Random.Range(0f, 100f);
            foreach (var loot in Loots)
            {
                rate -= loot.dropRate;
                if (rate <= 0)
                {
                    Instantiate(loot.Item, transform.position, quaternion.identity);
                    break;
                }
            }

            lootItem = false;
        }
    }
}
