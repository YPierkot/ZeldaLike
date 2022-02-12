using CustomizeEditor.HierarchySO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HierarchySettingsSO))]
public class HierarchySettingsSOEditor : Editor{

    public override void OnInspectorGUI() {
        if (GUILayout.Button("Open Settings")) {
            SettingsService.OpenProjectSettings("Project/Custom Hierarchy");
        }
    }
}
