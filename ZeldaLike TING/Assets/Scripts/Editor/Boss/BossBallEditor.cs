using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(BossBall))]
public class BossBallEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BossBall script = (BossBall)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("speedModifier"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("heightMultiplicator"));
        EditorGUILayout.Space();
        script.ballType = (BossBall.EffectType)EditorGUILayout.EnumPopup("Ball Type", script.ballType);
        switch (script.ballType)
        {
            case BossBall.EffectType.Wind :
                EditorGUILayout.PropertyField(serializedObject.FindProperty("repulseForce"));
                break;
            case BossBall.EffectType.Rock :
                EditorGUILayout.PropertyField(serializedObject.FindProperty("rockTransform"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
