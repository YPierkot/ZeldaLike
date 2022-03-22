using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    #region INSTANCE

    public static PoolManager Instance;

    private void Awake()
    {
       if(Instance == null) Instance = this;
    }

    #endregion
    
    public enum Object
    {
        None, fireCard, iceCard, wallCard, windCard
    }
    
    [Serializable]
    struct Pool
    {
       public GameObject ObjectPrefabs;
       public Object typeOfObject;
    }
    

    [SerializeField] private Pool[] poolArray;
    private Dictionary<Object, Queue<GameObject>> queueDictionary = new Dictionary<Object, Queue<GameObject>>(); // Store Queue of Object
    private Dictionary<Object, GameObject> prefabDictionary = new Dictionary<Object, GameObject>(); // Store prefabs independently of Queue


    void Start()
    {
        InitPool();
    }

    void InitPool()
    {
        foreach (Pool pol in poolArray)
        {
            queueDictionary.Add(pol.typeOfObject, new Queue<GameObject>());
            prefabDictionary.Add(pol.typeOfObject, pol.ObjectPrefabs);
        }
    }

    public GameObject PoolInstantiate(Object typeOfObject)
    {
        GameObject GM;
        Queue<GameObject> queue = queueDictionary[typeOfObject];
        if (queue.Count == 0)
        {
            GM = Instantiate(prefabDictionary[typeOfObject]);
        }
        else
        {
            GM = queue.Dequeue();
            GM.SetActive(true);
        }

        return GM;
    }

}
