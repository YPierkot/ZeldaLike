using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(InteracteObject) , true)]
public class InteracteObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        InteracteObject script = (InteracteObject)target;
        serializedObject.FindProperty("isRune").boolValue = EditorGUILayout.Toggle("Is Rune", serializedObject.FindProperty("isRune").boolValue);
        GUILayout.Label("--- FIRE");
        script.fireAffect = EditorGUILayout.Toggle("Fire Affect", script.fireAffect);
        if (script.fireAffect)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fireColor"));
            serializedObject.FindProperty("canBurn").boolValue = EditorGUILayout.Toggle("Can Burn", serializedObject.FindProperty("canBurn").boolValue);
            
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
            EditorGUILayout.PropertyField(serializedObject.FindProperty("iceColor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onFreeze"));
            script.canFreeze = EditorGUILayout.Toggle("Can Freeze", script.canFreeze);
            script.barrier = EditorGUILayout.Toggle("Barrier", script.barrier);
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


        if (!((InteracteObject) target).gameObject.CompareTag("Interactable")) ((InteracteObject) target).gameObject.tag = "Interactable";
        if (((InteracteObject) target).gameObject.layer != 11) ((InteracteObject) target).gameObject.layer = 11;
    }
}

