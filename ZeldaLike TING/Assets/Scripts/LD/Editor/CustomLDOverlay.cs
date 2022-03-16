#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.SceneManagement;
using UnityEngine;

[Overlay(typeof(SceneView),overlayID, overlayName )]
public class CustomLDOverlay : IMGUIOverlay {
    private const string overlayID = "my-custom-LD-overlay";
    private const string overlayName = "Custom LD Overlay";
    
    public override void OnGUI() {
        using (new GUILayout.HorizontalScope()) {
            GUILayout.Label("Custom LD Overlay".ToUpper(), new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold, fontSize = 12});
            GUILayout.Space(10);
        }
        
        GUILayout.Space(4);
        CustomLDData.showGizmos = GUILayout.Toggle(CustomLDData.showGizmos, "Show LD Gizmos");

        DrawLoadSceneButton();
        DrawWindowSizeButton();

    }

    /// <summary>
    /// Maximize or Minimize the SceneView
    /// </summary>
    private void DrawWindowSizeButton() {
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
            
            string scenePath = "Assets/_Scenes/Level Design/LD_ThiefCommunity.unity";
            menu.AddItem(new GUIContent("Thief Community"), isSceneActiv(scenePath), EnableOrDisableScene, scenePath);
            
            scenePath = "Assets/_Scenes/Level Design/LD_Farm.unity";
            menu.AddItem(new GUIContent("Farm"), isSceneActiv(scenePath), EnableOrDisableScene, scenePath);
            
            scenePath = "Assets/_Scenes/Level Design/LD_Lac.unity";
            menu.AddItem(new GUIContent("Lacs"), isSceneActiv(scenePath), EnableOrDisableScene, scenePath);
            
            menu.ShowAsContext();
        }
    }
    
    /// <summary>
    /// Enable or Disable a scene
    /// </summary>
    /// <param name="obj"></param>
    private void EnableOrDisableScene(object obj) {
        if (isSceneActiv((string) obj)) EditorSceneManager.CloseScene(EditorSceneManager.GetSceneByPath((string) obj), true);
        else EditorSceneManager.OpenScene((string) obj , OpenSceneMode.Additive);
    }

    /// <summary>
    /// Return true if the scene is loaded
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private bool isSceneActiv(string path) => EditorSceneManager.GetSceneByPath(path).isLoaded;
    #endregion LoadScene
}
#endif
