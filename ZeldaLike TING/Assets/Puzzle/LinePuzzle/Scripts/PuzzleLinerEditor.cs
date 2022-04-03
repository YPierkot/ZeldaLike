using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class PuzzleLinerEditor : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    [SerializeField] private PuzzleLine _puzzleLine;


   

    void Update()
    {
        if (Selection.Contains(gameObject))
        {
            Collider[] endEnters = Physics.OverlapSphere(line.GetPosition(line.positionCount-1)+transform.position, 0.1f);
            foreach (Collider enter in endEnters)
            {
                if (enter.CompareTag("LinePuzzleInput"))
                {
                    var connector = enter.GetComponentInParent<LinePuzzleConnector>();
                    _puzzleLine.endConnector = connector;
                    connector.SetEnter(_puzzleLine, enter.transform);
                    line.SetPosition(line.positionCount-1, enter.transform.position-transform.position);
                }
            }
            if (endEnters.Length == 0 && _puzzleLine.endConnector != null)
            {
                _puzzleLine.endConnector.RemoveEnter(_puzzleLine);
                _puzzleLine.endConnector = null;
            }
            
            Collider[] startEnters = Physics.OverlapSphere(line.GetPosition(0)+transform.position, 0.1f);
            foreach (Collider enter in startEnters)
            {
                Debug.Log(enter.name);
                if (enter.CompareTag("LinePuzzleInput"))
                {
                    var connector = enter.GetComponentInParent<LinePuzzleConnector>();
                    _puzzleLine.startConnector = connector;
                    connector.SetEnter(_puzzleLine, enter.transform);
                    line.SetPosition(0, enter.transform.position-transform.position);
                }
            }
            if (startEnters.Length == 0 && _puzzleLine.startConnector != null)
            {
                _puzzleLine.startConnector.RemoveEnter(_puzzleLine);
                _puzzleLine.startConnector = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(line.GetPosition(line.positionCount-1)+transform.position, 0.1f);
    }
}
