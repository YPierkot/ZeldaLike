using System;
using UnityEngine;

public class LinePuzzleConnector : InteracteObject
{

    public enum Side
    {
        none, top, left, right, bot
    }
    
    [System.Serializable]
    struct Enters
    {
        public Transform topEnter;
        public Transform leftEnter;
        public Transform rightEnter;
        public Transform botEnter;
    }

    [Serializable]
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

    [SerializeField] private PuzzleLine topLine;
    [SerializeField] private PuzzleLine leftLine;
    [SerializeField] private PuzzleLine rightLine;
    [SerializeField] private PuzzleLine botLine;
    
    System.Collections.Generic.List<ProgressLine> progressLines = new System.Collections.Generic.List<ProgressLine>();

    void Update()
    {
        foreach (var progress in progressLines)
        {
            /*if (LRDebug != null)
            { 
                LRDebug.colorGradient.colorKeys[1].color = Color.red; 
                LRDebug.colorGradient.colorKeys[1].time = 0.5f;
            }*/
            
            GradientColorKey[] gradientColorKey = progress.line.colorGradient.colorKeys;
            if (progress.fromStart)
            {
                if (progress.line.startColor != Color.red) progress.line.startColor = Color.red;
                else 
                {
                    Debug.Log("Finish Line");
                    progress.line.endColor = Color.red; 
                    SendSignal(progress.side);
                    progressLines.Remove(progress);
                    break;
                }
            }
            else
            {
                if (progress.line.endColor != Color.red) progress.line.endColor = Color.red;
                else 
                {
                    Debug.Log("Finish "+ progress.line.name +" Line");
                    progress.line.startColor = Color.red; 
                    EmitSignal(progress.side);
                    progressLines.Remove(progress);
                    break;
                }

            }
        }
    }

    public override void OnFireEffect()
    {
        base.OnFireEffect();
        if(FireAffect)SendSignal(Side.none);
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

   void SendSignal(Side sideStart)
    {
        if (sideStart != Side.top && top)
        {
            ProgressLine topLineProg = new ProgressLine();
            topLineProg.line = _enters.topEnter.GetComponentInChildren<LineRenderer>();
            topLineProg.side = Side.top;
            progressLines.Add(topLineProg);
        }
        if (sideStart != Side.left && left)
        {
            ProgressLine leftLineProg = new ProgressLine();
            leftLineProg.line = _enters.leftEnter.GetComponentInChildren<LineRenderer>();
            leftLineProg.side = Side.left;
            progressLines.Add(leftLineProg);
            
        }
        if (sideStart != Side.right && right)
        {
            ProgressLine rightLineProg = new ProgressLine();
            rightLineProg.line = _enters.rightEnter.GetComponentInChildren<LineRenderer>();
            rightLineProg.side = Side.right;
            progressLines.Add(rightLineProg);
        }
        if (sideStart != Side.bot&& bot)
        {
            ProgressLine botLineProg = new ProgressLine();
            botLineProg.line = _enters.botEnter.GetComponentInChildren<LineRenderer>();
            botLineProg.side = Side.bot;
            progressLines.Add(botLineProg);
        }
    }

    void EmitSignal(Side side)
    {
        Debug.Log("Emit from " + side);
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
