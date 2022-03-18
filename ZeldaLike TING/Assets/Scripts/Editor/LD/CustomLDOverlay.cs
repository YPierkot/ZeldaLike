#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Directory = System.IO.Directory;

[Overlay(typeof(SceneView),overlayID, overlayName )]
public class CustomLDOverlay : IMGUIOverlay {
    private const string overlayID = "my-custom-LD-overlay";
    private const string overlayName = "Custom LD Overlay";
    private static GameObject LDPlan = null;
    private static bool isPlanActiv = false;
    
    #region MenuItem
    /// <summary>
    /// Show or Hide the Gizmos
    /// </summary>
    [MenuItem("Tools/LD/Enable or Disable Gizmos &a")]
    private static void ShowOrHideGizmos() {
        CustomLDData.showGizmos = !CustomLDData.showGizmos;
    }
    
    /// <summary>
    /// Enable Or Disable the LD Plan with a menu item. It allows to use an input to quiclky change the visibility of the plan
    /// </summary>
    [MenuItem("Tools/LD/Enable or Disable Plan &z")]
    private static void EnableOrDisablePlan() {
        if (LDPlan == null) return;
        isPlanActiv = !isPlanActiv;
        SceneVisibilityManager.instance.ToggleVisibility(LDPlan, isPlanActiv);
    }
    #endregion
    
    public override void OnGUI() {
        if (LDPlan == null) {
            LDPlan = GameObject.FindGameObjectWithTag("Plan");
            if (LDPlan != null) isPlanActiv = !SceneVisibilityManager.instance.IsHidden(LDPlan);
        }
        
        using (new GUILayout.HorizontalScope()) {
            GUILayout.BeginVertical();
            GUILayout.Label("Custom LD Overlay".ToUpper(), new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold, fontSize = 12});
            GUILayout.EndVertical();
            GUILayout.Space(10);
        }
        
        GUILayout.Space(4);
        using (new GUILayout.HorizontalScope()) {
            GUIStyle buttonStyle = new GUIStyle(EditorStyles.label) {
                padding = new RectOffset(0, 0, 0, 0),
                alignment = TextAnchor.MiddleCenter
            };
            
            CustomLDData.showGizmos = GUILayout.Toggle(CustomLDData.showGizmos, "Show LD Gizmos (alt+a)");
            if (GUILayout.Button(new GUIContent(EditorGUIUtility.IconContent("TerrainInspector.TerrainToolSettings On")), buttonStyle)) {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Narrativ Gizmos"), CustomLDData.showGizmosDialogue, c => CustomLDData.showGizmosDialogue =! CustomLDData.showGizmosDialogue, null);
                menu.AddItem(new GUIContent("Object Relation Gizmos"), CustomLDData.showGizmosRelation, c => CustomLDData.showGizmosRelation =! CustomLDData.showGizmosRelation, null);
                menu.ShowAsContext();
            }
        }
        
        if (LDPlan != null) {
            EditorGUI.BeginChangeCheck();
            isPlanActiv = GUILayout.Toggle(isPlanActiv, "Show Plan (alt+z)");
            if (EditorGUI.EndChangeCheck()) {
                SceneVisibilityManager.instance.ToggleVisibility(LDPlan, isPlanActiv);
            }
        }

        DrawLoadSceneButton();
        DrawWindowSizeButton();

    }

    /// <summary>
    /// Maximize or Minimize the SceneView
    /// </summary>
    private void DrawWindowSizeButton() {
        if (EditorWindow.focusedWindow == null) return;
        if (GUILayout.Button(EditorWindow.focusedWindow.maximized?"Minimize Window" : "Maximize Window")) {
            EditorWindow.focusedWindow.maximized = !EditorWindow.focusedWindow.maximized;
        }
    }
    
    #region LoadScene
    /// <summary>
    /// Allowed to Enable or Disable some Scene
    /// </summary>
    private void DrawLoadSceneButton() {
        if (GUILayout.Button("Enable/Disable Scene")) {
            GenericMenu menu = new GenericMenu();

            string[] scenesPath = Directory.GetFiles("Assets/_Scenes/Level Design/");
            foreach (string scenePath in scenesPath) {
                if (scenePath.Contains(".unity") && !scenePath.EndsWith(".meta")) {
                    Object actualScene = AssetDatabase.LoadMainAssetAtPath(scenePath);
                    menu.AddItem(new GUIContent(actualScene.name.Substring(3, actualScene.name.Length - 3)), IsSceneActiv(scenePath), EnableOrDisableScene, scenePath);
                }
            }
            
            if(!IsAllSceneActiv(scenesPath)) menu.AddItem(new GUIContent("All"), IsAllSceneActiv(scenesPath),EnableAllScene, scenesPath);
            menu.ShowAsContext();
        }
    }
    
    /// <summary>
    /// Enable or Disable a scene
    /// </summary>
    /// <param name="obj"></param>
    private void EnableOrDisableScene(object obj) {
        if (IsSceneActiv((string) obj)) EditorSceneManager.CloseScene(EditorSceneManager.GetSceneByPath((string) obj), true);
        else EditorSceneManager.OpenScene((string) obj , OpenSceneMode.Additive);
    }

    private void EnableAllScene(object paths) {
        string[] allPath = (string[]) paths;
        foreach (string scenePath in allPath) {
            if (scenePath.Contains(".unity") && !scenePath.EndsWith(".meta") && !SceneManager.GetSceneByPath(scenePath).isLoaded) {
                EditorSceneManager.OpenScene((string) scenePath , OpenSceneMode.Additive);
            }
        }
    }

    /// <summary>
    /// Return true if the scene is loaded
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private bool IsSceneActiv(string path) => EditorSceneManager.GetSceneByPath(path).isLoaded;

    /// <summary>
    /// Is all scenes activ
    /// </summary>
    /// <param name="paths"></param>
    /// <returns></returns>
    private bool IsAllSceneActiv(string[] paths) {
        foreach (string scenePath in paths) {
            if (scenePath.Contains(".unity") && !scenePath.EndsWith(".meta") && !IsSceneActiv(scenePath)) return false;
        }
        return true;
    }
    #endregion LoadScene
}
#endif
