using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TriggeringDialogue))]
public class TriggeringDialogueEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TriggeringDialogue script = (TriggeringDialogue) target;
        if (script.isCinematic)
        {
            script.cameraPoint =
                (Transform)EditorGUILayout.ObjectField("Camera Point", script.cameraPoint, typeof(Transform), true);
            script.cinematicTime = EditorGUILayout.FloatField("Cinematic Time", script.cinematicTime);
            script.zoneName = EditorGUILayout.TextField("Zone name", script.zoneName);
            script.isMonolith = EditorGUILayout.Toggle("Monolith", script.isMonolith);
        }

        if (script.isMonolith)
        {
            script.shortMonolithDialogue = (DialogueScriptable) EditorGUILayout.ObjectField("Short dialogue",
                script.shortMonolithDialogue, typeof(DialogueScriptable), true);
            script.mistMovement =
                (Animator) EditorGUILayout.ObjectField("Mist Movement", script.mistMovement, typeof(Animator), true);
            script.monolithFX = (Animator) EditorGUILayout.ObjectField("Monolith FX", script.monolithFX, typeof(Animator), true);
            script.isEarthMonolith = EditorGUILayout.Toggle("Earth Monolith", script.isEarthMonolith);
        }

        if (script.isTutorial)
        {
            script.tutorialManager = (TutorialManager) EditorGUILayout.ObjectField("Tutorial Manager",
                script.tutorialManager, typeof(TutorialManager), true);
        }

        if (script.enemiesIsCondition)
        {
            script.enemiesParent =
                (Transform) EditorGUILayout.ObjectField("Object Parent", script.enemiesParent, typeof(Transform), true);
        }
    }
}
