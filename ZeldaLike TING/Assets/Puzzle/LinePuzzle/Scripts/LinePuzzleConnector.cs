using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePuzzleConnector : MonoBehaviour
{
    public enum Side
    {
        top,left,right,bot
    }

    [Serializable]
    struct Enters
    {
        public Transform topEnter;
        public Transform leftEnter;
        public Transform rightEnter;
        public Transform botEnter;
    }

    [Header("=== LinePuzzleConnector")]
    [SerializeField] private Enters _enters;

    private PuzzleLine topLine;
    private PuzzleLine leftLine;
    private PuzzleLine rightLine;
    private PuzzleLine botLine;

    [Header("Connect Side")]
    [SerializeField] private bool top;
    [SerializeField] private bool left;
    [SerializeField] private bool right;
    [SerializeField] private bool bot;

    #region EDITOR

    public void UpdateEnter()
    {
        if (top) _enters.topEnter.gameObject.SetActive(false);
        if (top) _enters.topEnter.gameObject.SetActive(false);
        if (top) _enters.topEnter.gameObject.SetActive(false);
        if (top) _enters.topEnter.gameObject.SetActive(false);
    }
    
    public void SetEnter(PuzzleLine line, Transform enter)
    {
        if (enter == _enters.topEnter) topLine = line;
        
        else if (enter == _enters.leftEnter) leftLine = line;
        
        else if (enter == _enters.rightEnter) rightLine = line;
        
        else if (enter == _enters.botEnter) botLine = line;
    }
    
    public void RemoveEnter(PuzzleLine line)
    {
        if (line == topLine) topLine = null;
        else if (line == leftLine) leftLine = null;
        else if (line == rightLine) rightLine = null;
        else if (line == botLine) botLine = null;
    }
    
    #endregion
}
