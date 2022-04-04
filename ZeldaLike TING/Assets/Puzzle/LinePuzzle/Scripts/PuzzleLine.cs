using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleLine : MonoBehaviour
{
    public LinePuzzleConnector startConnector;
    public LinePuzzleConnector.Side startSide;
    public LinePuzzleConnector endConnector;
    public LinePuzzleConnector.Side endSide;
    
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetProgress(LinePuzzleConnector sender)
    {
        if (sender == startConnector)
        {
            
        }
    }
}
