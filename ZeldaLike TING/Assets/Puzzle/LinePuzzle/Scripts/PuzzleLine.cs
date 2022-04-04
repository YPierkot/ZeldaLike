using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleLine : MonoBehaviour
{
    private LineRenderer line;

    public LinePuzzleConnector startConnector;
    public LinePuzzleConnector.Side startSide;
    public LinePuzzleConnector endConnector;
    public LinePuzzleConnector.Side endSide;

    private bool progress;
    private bool fromStart;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (progress)
        {
            GradientColorKey[] gradientColorKey = line.colorGradient.colorKeys;
            if (fromStart)
            {
                gradientColorKey[0].color = Color.red;
                gradientColorKey[0].time += 0.01f;
                if (gradientColorKey[0].time >= 1)
                {
                    endConnector.RecieveSignal(endSide);
                }
            }
            else
            {
                int index = line.colorGradient.colorKeys.Length-1;
                
                gradientColorKey[index].color = Color.red;
                gradientColorKey[index].time -= 0.01f;
                if (gradientColorKey[index].time <= 0)
                {
                    endConnector.RecieveSignal(startSide);
                }
            }
        }
    }

    public void SetProgress(LinePuzzleConnector sender)
    {
        progress = true;
        if (sender == startConnector) fromStart = true;
        else fromStart = false;

    }
}
