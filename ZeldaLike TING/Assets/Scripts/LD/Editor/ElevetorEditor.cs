using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Elevetor))]
public class ElevetorEditor : InteracteObjectEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("platform"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("passPoint"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("waitingTime"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("auto"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("canMove"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("boxHeight"));
        serializedObject.ApplyModifiedProperties();

    }
}
