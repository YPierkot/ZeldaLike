using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Vine))]
public class vineEditor : InteracteObjectEditor
{
   public override void OnInspectorGUI()
   {
      base.OnInspectorGUI();
      EditorGUILayout.PropertyField(serializedObject.FindProperty("lianaBurnMaterial"));
      EditorGUILayout.PropertyField(serializedObject.FindProperty("burnAmount"));
      serializedObject.ApplyModifiedProperties();

   }
}
