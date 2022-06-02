using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Loot))]
public class LootEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Loot script = (Loot)target;
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("lootType"));
        switch (script.lootType)
        {
            case Loot.LootType.Coin : EditorGUILayout.PropertyField(serializedObject.FindProperty("amount"));
                break;
            
            case Loot.LootType.ModulePiece : EditorGUILayout.PropertyField(serializedObject.FindProperty("_moduleType"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
