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

    public void SetSave(int liveAmount, int moneyAmount, Transform spawnSaveTransform)
    {
        lifeSave = liveAmount;
        moneySave = moneyAmount;
        spawnPointSave = spawnSaveTransform;
    }
}
