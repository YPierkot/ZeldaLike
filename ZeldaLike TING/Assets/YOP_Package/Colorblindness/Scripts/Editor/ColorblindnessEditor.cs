using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Colorblindness))]
public class ColorblindnessEditor : Editor {
    private bool isButtonOpen = false;

    public override void OnInspectorGUI() {
        Colorblindness script = ((Colorblindness) target);
        base.OnInspectorGUI();

        GUILayout.Space(8);
        isButtonOpen = EditorGUILayout.Foldout(isButtonOpen, "Show Colorblindness Buttons", true);
        
        if (!isButtonOpen) return;
        for (int i = 0; i < Colorblindness.maxType; i++) {
            if (GUILayout.Button(((ColorblindTypes) i).ToString())) {
                script.ChangeFilter(i);
            }
        }
    }
}
