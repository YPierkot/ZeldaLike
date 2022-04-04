using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LinePuzzleConnector))]
public class LinePuzzleConnectorGUI : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LinePuzzleConnector script = (LinePuzzleConnector) target;
        if (GUILayout.Button("Update"))
        {
            script.UpdateEnter();
        }
    }
}
