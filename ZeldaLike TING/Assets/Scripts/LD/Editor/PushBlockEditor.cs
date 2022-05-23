using UnityEditor;

[CustomEditor(typeof(pushBlock), true)]
public class PushBlockEditor : InteracteObjectEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("currentWaypoint"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("newWaypoint"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("distanceThreshold"));
        
        serializedObject.ApplyModifiedProperties();
    }
}
