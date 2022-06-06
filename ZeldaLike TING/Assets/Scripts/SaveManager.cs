using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    private void Awake()
    {
        if (instance != null)
            instance = this;
        else 
            instance = this;
    }

    
    public int lifeSave;
    public int moneySave;
    public Transform spawnPointSave;

    private void Start()
    {
        lifeSave = PlayerStat.instance.lifeMax;
        moneySave = PlayerStat.instance.money;
        SetSave(lifeSave, moneySave, transform);
    }

    public void SetSave(int liveAmount, int moneyAmount, Transform spawnSaveTransform)
    {
        lifeSave = liveAmount;
        moneySave = moneyAmount;
        spawnPointSave = spawnSaveTransform;
    }
}
