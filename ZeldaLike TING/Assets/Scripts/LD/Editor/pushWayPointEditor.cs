using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CustomEditor(typeof(pushWayPoint))]
public class pushWayPointEditor : Editor
{
    private float merge = 0.5f;
    private Vector3 WaypointPos;

    
    private bool magnet = true;
    

    public override void OnInspectorGUI()
    {
        pushWayPoint script = (pushWayPoint) target;
        WaypointPos = script.transform.position;

            if (script.topPoint != null)
            {
                if (magnet && script.transform.position.x < script.topPoint.transform.position.x + merge && script.transform.position.x > script.topPoint.transform.position.x - merge) WaypointPos.x = script.topPoint.transform.position.x;
                if(script.topPoint.botPoint == null && script.topPoint.canSetWaypoint) script.topPoint.botPoint = script;
            }

            if (script.botPoint != null)
            {
                if (magnet && script.transform.position.x < script.botPoint.transform.position.x + merge && script.transform.position.x > script.botPoint.transform.position.x - merge) WaypointPos.x = script.botPoint.transform.position.x;
                if (script.botPoint.topPoint == null && script.botPoint.canSetWaypoint)
                {
                    script.botPoint.topPoint = script;
                    Debug.Log($"Set {script.botPoint.name}.topPoin : {script.name}");
                }
            }

            if (script.leftPoint != null)
            {
                if (magnet && script.transform.position.z < script.leftPoint.transform.position.z + merge && script.transform.position.z > script.leftPoint.transform.position.z - merge) WaypointPos.z = script.leftPoint.transform.position.z;
                if(script.leftPoint.rightPoint == null && script.leftPoint.canSetWaypoint) script.leftPoint.rightPoint = script;
            }

            if (script.rightPoint != null)
            {
                if (magnet && script.transform.position.z < script.rightPoint.transform.position.z + merge && script.transform.position.z > script.rightPoint.transform .position.z - merge) WaypointPos.z = script.rightPoint.transform.position.z;
                if(script.rightPoint.leftPoint == null && script.rightPoint.canSetWaypoint) script.rightPoint.leftPoint = script;
            }

            script.transform.position = WaypointPos;

            DrawDefaultInspector();
            //serializedObject.FindProperty("canSetWaypoint").boolValue  = EditorGUILayout.Toggle("can Set Waypoint", serializedObject.FindProperty("canSetWaypoint").boolValue);
            magnet =  EditorGUILayout.Toggle("Magnet", magnet);
    }
}
/*
*/
