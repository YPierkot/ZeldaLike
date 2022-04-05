using UnityEditor;

[CustomEditor(typeof(Door), true)]
public class DoorEditor : InteracteObjectEditor
{
    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();  
        base.OnInspectorGUI();
        Door script = (Door) target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("door"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("openPosition"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("closePosition"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"));
        serializedObject.ApplyModifiedProperties();
    }
}
