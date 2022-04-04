using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class LinePuzzleConnector : InteracteObject
{

    public enum Side
    {
        none, top, left, right, bot
    }
    
    [Serializable]
    struct Enters
    {
        public Transform topEnter;
        public Transform leftEnter;
        public Transform rightEnter;
        public Transform botEnter;
    }

    struct ProgressLine
    {
        public LineRenderer line;
        public bool fromStart;
    }

    [Header("=== Line Puzzle Connector")]
    [SerializeField] private Enters _enters;


    [Header("Connect Side")]
    [SerializeField] private bool top;
    [SerializeField] private bool left;
    [SerializeField] private bool right;
    [SerializeField] private bool bot;

    private PuzzleLine topLine;
    private PuzzleLine leftLine;
    private PuzzleLine rightLine;
    private PuzzleLine botLine;
    
    List<ProgressLine> progressLines = new List<ProgressLine>();

    void Update()
    {
        foreach (var progress in progressLines)
        {
            if (progress.fromStart)
            {
                //progress.line.colorGradient
            }
        }
    }

    public override void OnFireEffect()
    {
        base.OnFireEffect();
        
    }

    public void RecieveSignal(Side side)
    {
        switch (side)
        {
            case Side.top:
                progressLines.Add(_enters.topEnter.GetComponentInChildren<LineRenderer>());
                break;
            
            case Side.left: 
                progressLines.Add(_enters.leftEnter.GetComponentInChildren<LineRenderer>());
                break;
            
            case Side.right:
                progressLines.Add(_enters.rightEnter.GetComponentInChildren<LineRenderer>());
                break;
            
            case Side.bot: 
                progressLines.Add(_enters.botEnter.GetComponentInChildren<LineRenderer>());
                break;
        }
    }
    

    #region EDITOR

    public void UpdateEnter()
    {
        if (top) _enters.topEnter.gameObject.SetActive(true);
        else _enters.topEnter.gameObject.SetActive(false);
        if (left) _enters.leftEnter.gameObject.SetActive(true);
        else _enters.leftEnter.gameObject.SetActive(false);
        if (right) _enters.rightEnter.gameObject.SetActive(true);
        else _enters.rightEnter.gameObject.SetActive(false);
        if (bot) _enters.botEnter.gameObject.SetActive(true);
        else _enters.botEnter.gameObject.SetActive(false);
    }
    
    public Side SetEnter(PuzzleLine line, Transform enter)
    {
        if (enter == _enters.topEnter)
        {
            topLine = line;
            return Side.top;
        }
        
        else if (enter == _enters.leftEnter)
        {
            leftLine = line;
            return Side.left;
        }
        
        else if (enter == _enters.rightEnter)
        {
            rightLine = line;
            return Side.right;
        }
        
        else if (enter == _enters.botEnter)
        {
            botLine = line;
            return Side.bot;
        }
        else
        {
            return Side.none;
        }
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
