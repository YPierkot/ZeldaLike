using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Instance

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public enum controller
    {
        Keybord, Xbox, ps
    }

    public controller currentContorller;
}
