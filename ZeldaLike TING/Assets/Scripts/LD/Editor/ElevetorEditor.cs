using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Elevetor))]
public class ElevetorEditor : InteracteObjectEditor
{
    public override void OnInspectorGUI() {
        serializedObject.Update();
        base.OnInspectorGUI();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("platform"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("passPoint"));
        
        GUILayout.Space(5);

        using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
            using (new GUILayout.HorizontalScope()) {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("waitingTime"));
                serializedObject.FindProperty("waitBetweenStartAndEnd").boolValue = GUILayout.Toggle(serializedObject.FindProperty("waitBetweenStartAndEnd").boolValue, "Wait between Start and End", new GUIStyle(GUI.skin.button));
            }

            if (!serializedObject.FindProperty("waitBetweenStartAndEnd").boolValue) {
                
                GUILayout.Space(4);
                
                using (new GUILayout.HorizontalScope()) {
                    GUILayout.Space(15);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("waitPassPoint"));
                    GUILayout.Space(7);
                }
            }
        }

        GUILayout.Space(5);
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("auto"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("canMove"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("boxHeight"));
        serializedObject.ApplyModifiedProperties();
    }
}
