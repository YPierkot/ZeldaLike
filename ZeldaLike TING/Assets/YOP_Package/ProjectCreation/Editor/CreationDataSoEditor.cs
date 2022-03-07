#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CreationDataSO))]
public class CreationDataSoEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        using (new GUILayout.HorizontalScope()) {
            if (GUILayout.Button("Upload Data")) {
                ProjectCreationEditor.UpdateProjectData(true);
            }

            if (GUILayout.Button("Reset Scriptable")) {
                ((CreationDataSO) target).ReinitializeAsset();
            }
        }
    }
}
#endif
