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

    public struct ProgressLine
    {
        public LineRenderer line;
        public bool fromStart ;
        public Side side;
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
            GradientColorKey[] gradientColorKey = progress.line.colorGradient.colorKeys;
            if (progress.fromStart)
            {
                gradientColorKey[0].color = Color.red;
                gradientColorKey[0].time += 0.01f;
                if (gradientColorKey[0].time >= 1)
                {
                    SendSignal(progress.side);
                    progressLines.Remove(progress);
                }
            }
            else
            {
                int index = progress.line.colorGradient.colorKeys.Length-1;
                
                gradientColorKey[index].color = Color.red;
                gradientColorKey[index].time += 0.01f;
                if (gradientColorKey[index].time <= 0)
                {
                    EmitSignal(progress.side);
                    progressLines.Remove(progress);
                }
            }
        }
    }

    public override void OnFireEffect()
    {
        base.OnFireEffect();
        
    }

    public void RecieveSignal(Side side)
    {
        ProgressLine newProgress =new ProgressLine();
        newProgress.fromStart = true;
        switch (side)
        {
            case Side.top:
                newProgress.line =_enters.topEnter.GetComponentInChildren<LineRenderer>();
                newProgress.side = Side.top;
                break;
            
            case Side.left: 
                newProgress.line =_enters.leftEnter.GetComponentInChildren<LineRenderer>();
                newProgress.side = Side.left;
                break;
            
            case Side.right:
                newProgress.line =_enters.rightEnter.GetComponentInChildren<LineRenderer>();
                newProgress.side = Side.right;
                break;
            
            case Side.bot: 
                newProgress.line =_enters.botEnter.GetComponentInChildren<LineRenderer>();
                newProgress.side = Side.bot;
                break;
        }
        progressLines.Add(newProgress);
    }

    public void SendSignal(Side sideStart)
    {
        if (sideStart != Side.top)
        {
            ProgressLine topLineProg = new ProgressLine();
            topLineProg.line = _enters.topEnter.GetComponentInChildren<LineRenderer>();
            
        }
    }

    public void EmitSignal(Side side)
    {
        switch (side)
        {
            case Side.top: if(topLine != null) topLine.SetProgress(this);
                break;
            
            case Side.left: if(leftLine != null) leftLine.SetProgress(this);
                break;
            
            case Side.right: if(rightLine != null) rightLine.SetProgress(this);
                break;
            
            case Side.bot: if(botLine != null) botLine.SetProgress(this);
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
