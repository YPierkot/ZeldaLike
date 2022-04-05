using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InteracteObject) , true)]
public class InteracteObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        InteracteObject script = (InteracteObject)target;
        GUILayout.Label("--- FIRE");
        script.fireAffect = EditorGUILayout.Toggle("Fire Affect", script.fireAffect);
        if (script.fireAffect)
        {
            serializedObject.FindProperty("canBurn").boolValue = EditorGUILayout.Toggle("Can Burn", serializedObject.FindProperty("canBurn").boolValue);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onBurnDestroy"));
        }
        
        GUILayout.Label("--- WIND");
        script.windAffect = EditorGUILayout.Toggle("Wind Affect", script.windAffect);
        if (script.windAffect)
        {
            script.windThrough = EditorGUILayout.Toggle("Wind Through", script.windThrough);
        }
        
        GUILayout.Label("--- ICE");
        script.iceAffect = EditorGUILayout.Toggle("Ice Affected", script.iceAffect);
        if (serializedObject.FindProperty("iceAffect").boolValue)
        {
            script.canFreeze = EditorGUILayout.Toggle("Can Freeze", script.canFreeze);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("freezeCollider"));
            script.freezeTime = EditorGUILayout.FloatField("Freeze Time" , script.freezeTime);
        }
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("destroyInteract")); 
        serializedObject.ApplyModifiedProperties();
        
    }
}

