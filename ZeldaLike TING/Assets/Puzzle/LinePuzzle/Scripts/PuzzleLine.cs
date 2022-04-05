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
            if (fromStart)
            {
                if (line.startColor != Color.red) line.startColor = Color.red;
                else 
                {
                    Debug.Log("End");
                    line.endColor = Color.red; 
                    startConnector.RecieveSignal(startSide);
                    progress = false;
                }
            }
            else
            {
                if (line.startColor != Color.red) line.startColor = Color.red;
                else 
                {
                    Debug.Log("End");
                    line.endColor = Color.red; 
                    endConnector.RecieveSignal(endSide);
                    progress = false;
                }
            }
        }
    }

    public void SetProgress(LinePuzzleConnector sender)
    {
        progress = true;
        if (sender == startConnector)
        {
            fromStart = true;
            Debug.Log("progress from start");
        }
        else
        {
            fromStart = false;
            Debug.Log("progress from end");
        }

    }
}
