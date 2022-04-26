using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InteracteObject) , true)]
public class InteracteObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        InteracteObject script = (InteracteObject)target;
        GUILayout.Label("--- FIRE");
        script.fireAffect = EditorGUILayout.Toggle("Fire Affect", script.fireAffect);
        if (script.fireAffect)
        {
            serializedObject.FindProperty("canBurn").boolValue = EditorGUILayout.Toggle("Can Burn", serializedObject.FindProperty("canBurn").boolValue);
            serializedObject.FindProperty("burnDestroy").boolValue = EditorGUILayout.Toggle("Burn Destroy", serializedObject.FindProperty("burnDestroy").boolValue);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onBurn"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onBurnDestroy"));
        }
        
        GUILayout.Label("--- WIND");
        script.windAffect = EditorGUILayout.Toggle("Wind Affect", script.windAffect);
        if (script.windAffect)
        {
            script.windThrough = EditorGUILayout.Toggle("Wind Through", script.windThrough);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onWind"));
        }
        
        GUILayout.Label("--- ICE");
        script.iceAffect = EditorGUILayout.Toggle("Ice Affected", script.iceAffect);
        if (serializedObject.FindProperty("iceAffect").boolValue)
        {
            script.canFreeze = EditorGUILayout.Toggle("Can Freeze", script.canFreeze);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("freezeCollider"));
            script.freezeTime = EditorGUILayout.FloatField("Freeze Time" , script.freezeTime);
        }

        script.moveRestricted = EditorGUILayout.Toggle("Move Restricted", serializedObject.FindProperty("moveRestricted").boolValue);
        if (script.moveRestricted)  
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("moveTop"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("moveLeft"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("moveRight"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("moveBot"));
            
        }
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("destroyInteract")); 
        serializedObject.ApplyModifiedProperties();
        
    }
}

