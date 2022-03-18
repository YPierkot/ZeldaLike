#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using CustomizeEditor.HierarchySO;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

static class HierarchySettingsProvider {
    #region Variables

    private static bool foldoutSceneList;
    private static bool foldoutTreeBranch;
    private static bool ignoreIconsFoldout;
    private static bool componentIconsFoldout;

    private static GUIStyle backUpStyle;
    private static GUIStyle newStyle;

    #endregion Variables

    [SettingsProvider]
    public static SettingsProvider CreateMyCustomSettingsProvider() {
        var provider = new SettingsProvider("Project/Custom Hierarchy", SettingsScope.Project) {
            // By default the last token of the path is used as display name if no label is provided.
            label = "Custom Hierarchy",


            guiHandler = (searchContext) => {
                //Get the scriptableAsset and transform it into a SerializedObject
                var settings = new SerializedObject(AssetDatabase.LoadAssetAtPath<HierarchySettingsSO>("Assets/Resources/ScriptableObject/CustomHierarchy.asset"));
                DrawProperty(settings);

                settings.ApplyModifiedPropertiesWithoutUndo();
            },

            // Populate the search keywords to enable smart search filtering and label highlighting:
            keywords = new HashSet<string>(new[] {
                "use Custom Hierarchy", "Darker Background", "Use Darker Background", "Background Group A",
                "Background Group B", "use Colorfull Background", "tree Color List A", "Tree Color List B",
                "Use tree Branch", "Ignore Icons Name", "Debug Content Name", "Use Icon Gam", "Component Icon Type",
                "Draw MonoBehaviour Script Icon", "Use Icon Component", "Separator Tag", "Separator Color",
                "Use Separator"
            })
        };
        return provider;
    }

    /// <summary>
    /// Draw property of the serializedObject
    /// </summary>
    /// <param name="serializedObject"></param>
    public static void DrawProperty(SerializedObject serializedObject) {
        using (new GUILayout.HorizontalScope(EditorStyles.helpBox)) {
            DrawProperty(serializedObject.FindProperty("useCustomHierarchy"), "USE CUSTOM HIERARCHY", true);

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Reset Variables")) {
                AssetDatabase.LoadAssetAtPath<HierarchySettingsSO>("Assets/Resources/ScriptableObject/CustomHierarchy.asset").ResetVariables();
            }

            serializedObject.ApplyModifiedProperties();
            GUI.backgroundColor = Color.white;
        }

        if (!serializedObject.FindProperty("useCustomHierarchy").boolValue) return;

        GUILayout.Space(4);
        DrawSceneList(serializedObject);
        
        GUILayout.Space(4);
        
        //Draw Background ColorsField
        DrawBackgroundBox(serializedObject);

        GUILayout.Space(4);

        //Draw tree branch color list
        DrawTreeBranchBox(serializedObject);

        GUILayout.Space(4);

        //Draw Icon Data
        DrawIconContentBox(serializedObject);

        GUILayout.Space(4);

        //Draw Separator Data
        DrawSeparatorBox(serializedObject);

        serializedObject.ApplyModifiedProperties();
    }

    #region DrawPropertyMethods

    /// <summary>
    /// Draw scene List box
    /// </summary>
    /// <param name="serializedObject"></param>
    private static void DrawSceneList(SerializedObject serializedObject) {
        using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
            backUpStyle = new GUIStyle(EditorStyles.label);
            newStyle = new GUIStyle(EditorStyles.label) {alignment = TextAnchor.MiddleLeft, fontSize = 11};
            
            GUILayout.Label("Custom Scene List Data :", newStyle);
            EditorStyles.label.alignment = backUpStyle.alignment;
            EditorStyles.label.fontSize = backUpStyle.fontSize;
            
            //Draw Ignore Icons
            using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
                foldoutSceneList = EditorGUILayout.Foldout(foldoutSceneList, new GUIContent("Scene which used the custom hierarchy", "Pick the scene that need to use the custom hierarchy"), true);
                if (foldoutSceneList) {
                    SerializedProperty sceneList = serializedObject.FindProperty("activScene").Copy();
                    sceneList.Next(true);
                    sceneList.Next(true);

                    int arrayLength = sceneList.intValue;
                    sceneList.Next(true);

                    for (int i = 0; i < arrayLength; i++) {
                        using (new GUILayout.HorizontalScope(GUILayout.ExpandWidth(false))) {
                            backUpStyle = new GUIStyle(EditorStyles.label);
                            newStyle = new GUIStyle(EditorStyles.label) {alignment = TextAnchor.MiddleRight, fontSize = 12};
                            GUILayout.Label((i + 1) + " :", newStyle, GUILayout.Width(20));
                            EditorStyles.label.alignment = backUpStyle.alignment;
                            EditorStyles.label.fontSize = backUpStyle.fontSize;

                            EditorGUILayout.PropertyField(sceneList);
                            
                            GUI.backgroundColor = new Color(256 / 256f, 75 / 256f, 0 / 256f, 1);
                            if (GUILayout.Button("-", GUILayout.Width(20))) {
                                Undo.RecordObject(serializedObject.targetObject, "Remove Scene to use");
                                serializedObject.FindProperty("activScene").DeleteArrayElementAtIndex(i);
                                return;
                            }

                            GUI.backgroundColor = Color.white;
                        }

                        if (i < arrayLength - 1) sceneList.Next(false);
                    }

                    //Draw buttons
                    using (new GUILayout.HorizontalScope()) {
                        GUI.backgroundColor = new Color(.65f, .65f, .65f, 1);
                        if (GUILayout.Button("Add new Scene")) {
                            serializedObject.FindProperty("activScene").InsertArrayElementAtIndex(arrayLength);
                        }

                        if (GUILayout.Button("Get Activ Scene")) {
                            SceneAsset currentScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(SceneManager.GetActiveScene().path);
                            
                            serializedObject.FindProperty("activScene").InsertArrayElementAtIndex(arrayLength);
                            arrayLength++;
                            serializedObject.FindProperty("activScene").GetArrayElementAtIndex(arrayLength - 1).objectReferenceValue = currentScene;
                        }

                        GUI.backgroundColor = Color.white;
                        GUILayout.Space(2);
                    }

                    GUILayout.Space(2);
                }
            }
        }
    }

    /// <summary>
    /// Draw the separator box
    /// </summary>
    /// <param name="serializedObject"></param>
    private static void DrawSeparatorBox(SerializedObject serializedObject) {
        using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
            backUpStyle = new GUIStyle(EditorStyles.label);
            newStyle = new GUIStyle(EditorStyles.label) {alignment = TextAnchor.MiddleLeft, fontSize = 11};
            
            GUILayout.Label("Custom Separator Data :", newStyle);
            EditorStyles.label.alignment = backUpStyle.alignment;
            EditorStyles.label.fontSize = backUpStyle.fontSize;

            using (new GUILayout.HorizontalScope()) {
                serializedObject.FindProperty("useSeparator").boolValue = GUILayout.Toggle(serializedObject.FindProperty("useSeparator").boolValue, GUIContent.none, GUILayout.Width(15));
                DrawProperty(serializedObject.FindProperty("separatorColor"), "Separator Color", serializedObject.FindProperty("useSeparator").boolValue, 19);
            }

            using (new GUILayout.HorizontalScope()) {
                GUI.enabled = serializedObject.FindProperty("useSeparator").boolValue;
                using (new GUILayout.HorizontalScope()) {
                    GUILayout.Space(20);
                    GUILayout.Label("separator Line Size", GUILayout.Width(180));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("separatorLineSize"), GUIContent.none, true);
                    if (serializedObject.FindProperty("separatorLineSize").intValue > 16) serializedObject.FindProperty("separatorLineSize").intValue = 16;
                }

                GUI.enabled = true;
            }

            using (new GUILayout.HorizontalScope()) {
                GUILayout.Space(19);

                GUI.enabled = serializedObject.FindProperty("useSeparator").boolValue;
                using (new GUILayout.HorizontalScope()) {
                    GUILayout.Label("Separator Tag", GUILayout.Width(200 - 19));
                    GUI.enabled = false;
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("separatorTag"), GUIContent.none, true);
                }

                GUI.enabled = serializedObject.FindProperty("useSeparator").boolValue;

                backUpStyle = new GUIStyle(GUI.skin.button);
                newStyle = new GUIStyle(GUI.skin.button);
                newStyle.padding = new RectOffset(0, 0, 0, 0);

                GUI.backgroundColor = new Color(.7f, .7f, .7f, 1);
                if (GUILayout.Button(EditorGUIUtility.IconContent("d_Preset.Context"), newStyle, GUILayout.Width(18), GUILayout.Height(18))) {
                    var menu = new GenericMenu();
                    var function = new GenericMenu.MenuFunction2((tag) => {
                        serializedObject.FindProperty("separatorTag").stringValue = (string) tag;
                        serializedObject.ApplyModifiedProperties();
                    });
                    foreach (string tag in InternalEditorUtility.tags) {
                        menu.AddItem(new GUIContent(tag), serializedObject.FindProperty("separatorTag").stringValue == tag ? true : false, function, tag);
                    }

                    menu.ShowAsContext();
                }

                GUI.backgroundColor = Color.white;
                GUI.skin.button = backUpStyle;
                GUI.enabled = true;
            }


            GUILayout.Space(2);
        }
    }

    /// <summary>
    /// Draw the Icon Box
    /// </summary>
    /// <param name="serializedObject"></param>
    private static void DrawIconContentBox(SerializedObject serializedObject) {
        using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
            backUpStyle = new GUIStyle(EditorStyles.label);
            newStyle = new GUIStyle(EditorStyles.label) {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 11
            };
            GUILayout.Label("Custom Icon Content To Show :", newStyle);
            EditorStyles.label.alignment = backUpStyle.alignment;
            EditorStyles.label.fontSize = backUpStyle.fontSize;

            using (new GUILayout.HorizontalScope()) {
                serializedObject.FindProperty("useIconGam").boolValue = GUILayout.Toggle(serializedObject.FindProperty("useIconGam").boolValue, GUIContent.none, GUILayout.Width(15));
                GUI.enabled = serializedObject.FindProperty("useIconGam").boolValue;

                //Draw Ignore Icons
                using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
                    ignoreIconsFoldout = EditorGUILayout.Foldout(ignoreIconsFoldout, new GUIContent("Icon's Name To Ignore", "Write the name of the Icons you don't want to Show in front of the GameObject"), true);
                    if (ignoreIconsFoldout) {
                        SerializedProperty IconNameToIgnore = serializedObject.FindProperty("ignoreIconsName").Copy();
                        IconNameToIgnore.Next(true);
                        IconNameToIgnore.Next(true);

                        int arrayLength = IconNameToIgnore.intValue;
                        IconNameToIgnore.Next(true);

                        for (int i = 0; i < arrayLength; i++) {
                            using (new GUILayout.HorizontalScope(GUILayout.ExpandWidth(false))) {
                                backUpStyle = new GUIStyle(EditorStyles.label);
                                newStyle = new GUIStyle(EditorStyles.label) {
                                    alignment = TextAnchor.MiddleRight,
                                    fontSize = 12
                                };
                                GUILayout.Label((i + 1) + " :", newStyle, GUILayout.Width(20));
                                EditorStyles.label.alignment = backUpStyle.alignment;
                                EditorStyles.label.fontSize = backUpStyle.fontSize;

                                IconNameToIgnore.stringValue = EditorGUILayout.TextField(IconNameToIgnore.stringValue);

                                GUILayout.Label(new GUIContent(EditorGUIUtility.IconContent(IconNameToIgnore.stringValue)), GUILayout.Width(20), GUILayout.Height(20));

                                GUI.backgroundColor = new Color(256 / 256f, 75 / 256f, 0 / 256f, 1);
                                if (GUILayout.Button("-", GUILayout.Width(20))) {
                                    Undo.RecordObject(serializedObject.targetObject, "Remove IconName to Ignore");
                                    serializedObject.FindProperty("ignoreIconsName").DeleteArrayElementAtIndex(i);
                                    return;
                                }

                                GUI.backgroundColor = Color.white;
                            }

                            if (i < arrayLength - 1) IconNameToIgnore.Next(false);
                        }

                        //Draw buttons
                        using (new GUILayout.HorizontalScope()) {
                            GUI.backgroundColor = new Color(.65f, .65f, .65f, 1);
                            if (GUILayout.Button("Add new Icon")) {
                                serializedObject.FindProperty("ignoreIconsName").InsertArrayElementAtIndex(arrayLength);
                            }

                            if (GUILayout.Button("Get GameObject Content")) {
                                List<string> contentName = GetGUIContentFromActiveGameObject();
                                foreach (string t in contentName) {
                                    serializedObject.FindProperty("ignoreIconsName").InsertArrayElementAtIndex(arrayLength);
                                    arrayLength++;
                                    serializedObject.FindProperty("ignoreIconsName").GetArrayElementAtIndex(arrayLength - 1).stringValue = t;
                                }
                            }

                            GUI.backgroundColor = Color.white;
                            GUILayout.Space(2);
                        }

                        GUILayout.Space(2);
                    }
                }

                GUI.enabled = true;
                GUILayout.Space(2);
            }

            GUILayout.Space(2);

            using (new GUILayout.HorizontalScope()) {
                serializedObject.FindProperty("useIconComponent").boolValue = GUILayout.Toggle(serializedObject.FindProperty("useIconComponent").boolValue, GUIContent.none, GUILayout.Width(15));
                GUI.enabled = serializedObject.FindProperty("useIconComponent").boolValue;

                //Draw Components Icon list
                using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
                    componentIconsFoldout = EditorGUILayout.Foldout(componentIconsFoldout, new GUIContent("Component Icon To Show", "Write the name of the Components you want to show after the GameObject in the hierarchy"), true);
                    if (componentIconsFoldout) {
                        using (new GUILayout.HorizontalScope()) {
                            backUpStyle = new GUIStyle(EditorStyles.label);
                            newStyle = new GUIStyle(EditorStyles.label) {alignment = TextAnchor.MiddleRight, fontSize = 12};
                            GUILayout.Label(0 + " :", newStyle, GUILayout.Width(20));
                            EditorStyles.label.alignment = backUpStyle.alignment;
                            EditorStyles.label.fontSize = backUpStyle.fontSize;
                            serializedObject.FindProperty("drawMonobehaviourScriptIcon").boolValue = GUILayout.Toggle(serializedObject.FindProperty("drawMonobehaviourScriptIcon").boolValue, "Check if a GameObject has a custom MonoBehaviour Script");
                        }

                        SerializedProperty ComponentNametoAdd = serializedObject.FindProperty("componentIcontype").Copy();
                        ComponentNametoAdd.Next(true);
                        ComponentNametoAdd.Next(true);

                        int arrayLength = ComponentNametoAdd.intValue;
                        ComponentNametoAdd.Next(true);

                        for (int i = 0; i < arrayLength; i++) {
                            using (new GUILayout.HorizontalScope()) {
                                backUpStyle = new GUIStyle(EditorStyles.label);
                                newStyle = new GUIStyle(EditorStyles.label) {alignment = TextAnchor.MiddleRight, fontSize = 12};
                                GUILayout.Label((i + 1) + " :", newStyle, GUILayout.Width(20));
                                EditorStyles.label.alignment = backUpStyle.alignment;
                                EditorStyles.label.fontSize = backUpStyle.fontSize;

                                ComponentNametoAdd.stringValue = EditorGUILayout.TextField(ComponentNametoAdd.stringValue);

                                GUI.backgroundColor = new Color(256 / 256f, 75 / 256f, 0 / 256f, 1);
                                if (GUILayout.Button("-", GUILayout.Width(20))) {
                                    Undo.RecordObject(serializedObject.targetObject, "Remove IconName to Ignore");
                                    serializedObject.FindProperty("componentIcontype").DeleteArrayElementAtIndex(i);
                                }

                                GUI.backgroundColor = Color.white;
                            }

                            if (i < arrayLength - 1) ComponentNametoAdd.Next(false);
                        }

                        GUI.backgroundColor = new Color(.65f, .65f, .65f, 1);
                        if (GUILayout.Button("Add new Component")) {
                            serializedObject.FindProperty("componentIcontype").InsertArrayElementAtIndex(arrayLength);
                        }

                        GUI.backgroundColor = Color.white;
                        GUILayout.Space(2);
                    }
                }

                GUI.enabled = true;
                GUILayout.Space(2);
            }

            GUILayout.Space(2);
        }
    }

    /// <summary>
    /// Draw the TreeBranch Box
    /// </summary>
    /// <param name="serializedObject"></param>
    private static void DrawTreeBranchBox(SerializedObject serializedObject) {
        using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
            backUpStyle = new GUIStyle(EditorStyles.label);
            newStyle = new GUIStyle(EditorStyles.label) {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 11
            };
            GUILayout.Label("Custom TreeBranch Color :", newStyle);
            EditorStyles.label.alignment = backUpStyle.alignment;
            EditorStyles.label.fontSize = backUpStyle.fontSize;

            GUILayout.Space(2);
            using (new GUILayout.HorizontalScope()) {
                GUILayout.Label("Use Tree Branch", GUILayout.Width(200));
                serializedObject.FindProperty("useTreeBranch").boolValue = EditorGUILayout.Toggle(serializedObject.FindProperty("useTreeBranch").boolValue);
            }

            using (new GUILayout.HorizontalScope()) {
                GUILayout.Label("Tree Branch Size", GUILayout.Width(200));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("treeSize"), GUIContent.none);
            }

            using (new GUILayout.HorizontalScope()) {
                serializedObject.FindProperty("useComplexColor").boolValue = GUILayout.Toggle(serializedObject.FindProperty("useComplexColor").boolValue, GUIContent.none, GUILayout.Width(15));

                GUI.enabled = serializedObject.FindProperty("useTreeBranch").boolValue && (serializedObject.FindProperty("useComplexColor").boolValue ? true : false);
                //Draw tree branch color A
                using (new GUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(false))) {
                    foldoutTreeBranch = EditorGUILayout.Foldout(foldoutTreeBranch, "TreeBranch Color Group A", true);
                    if (foldoutTreeBranch) {
                        SerializedProperty treeBranchpropA = serializedObject.FindProperty("treeColorListA").Copy();
                        treeBranchpropA.Next(true);
                        treeBranchpropA.Next(true);

                        int arrayLength = treeBranchpropA.intValue;
                        treeBranchpropA.Next(true);

                        for (int i = 0; i < arrayLength; i++) {
                            using (new GUILayout.HorizontalScope()) {
                                backUpStyle = new GUIStyle(EditorStyles.label);
                                newStyle = new GUIStyle(EditorStyles.label) {
                                    alignment = TextAnchor.MiddleRight,
                                    fontSize = 12
                                };
                                GUILayout.Label((i + 1) + " :", newStyle, GUILayout.Width(20));
                                EditorStyles.label.alignment = backUpStyle.alignment;
                                EditorStyles.label.fontSize = backUpStyle.fontSize;

                                treeBranchpropA.colorValue = EditorGUILayout.ColorField(treeBranchpropA.colorValue);

                                GUI.backgroundColor = new Color(256 / 256f, 75 / 256f, 0 / 256f, 1);
                                if (GUILayout.Button("-", GUILayout.Width(20))) {
                                    Undo.RecordObject(serializedObject.targetObject, "Remove list Item");
                                    serializedObject.FindProperty("treeColorListA").DeleteArrayElementAtIndex(i);
                                }

                                GUI.backgroundColor = Color.white;
                            }

                            if (i < arrayLength - 1) treeBranchpropA.Next(false);
                        }

                        GUI.backgroundColor = new Color(.65f, .65f, .65f, 1);
                        if (GUILayout.Button("Add new Color to List")) {
                            serializedObject.FindProperty("treeColorListA").InsertArrayElementAtIndex(arrayLength);
                        }

                        GUI.backgroundColor = Color.white;
                        GUILayout.Space(2);
                    }
                }

                GUILayout.Space(2);

                //Draw tree branch color B
                using (new GUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.ExpandWidth(false))) {
                    foldoutTreeBranch = EditorGUILayout.Foldout(foldoutTreeBranch, "TreeBranch Color Group B", true);
                    if (foldoutTreeBranch) {
                        SerializedProperty treeBranchpropB = serializedObject.FindProperty("treeColorListB").Copy();
                        treeBranchpropB.Next(true);
                        treeBranchpropB.Next(true);

                        int arrayLength = treeBranchpropB.intValue;
                        treeBranchpropB.Next(true);

                        for (int i = 0; i < arrayLength; i++) {
                            using (new GUILayout.HorizontalScope()) {
                                backUpStyle = new GUIStyle(EditorStyles.label);
                                newStyle = new GUIStyle(EditorStyles.label) {
                                    alignment = TextAnchor.MiddleRight,
                                    fontSize = 12
                                };
                                GUILayout.Label((i + 1) + " :", newStyle, GUILayout.Width(20));
                                EditorStyles.label.alignment = backUpStyle.alignment;
                                EditorStyles.label.fontSize = backUpStyle.fontSize;

                                treeBranchpropB.colorValue = EditorGUILayout.ColorField(treeBranchpropB.colorValue);

                                GUI.backgroundColor = new Color(256 / 256f, 75 / 256f, 0 / 256f, 1);
                                if (GUILayout.Button("-", GUILayout.Width(20))) {
                                    Undo.RecordObject(serializedObject.targetObject, "Remove list Item");
                                    serializedObject.FindProperty("treeColorListA").DeleteArrayElementAtIndex(i);
                                }

                                GUI.backgroundColor = Color.white;
                            }

                            if (i < arrayLength - 1) treeBranchpropB.Next(false);
                        }

                        GUI.backgroundColor = new Color(.65f, .65f, .65f, 1);
                        if (GUILayout.Button("Add new Color to List")) {
                            serializedObject.FindProperty("treeColorListB").InsertArrayElementAtIndex(arrayLength);
                        }

                        GUI.backgroundColor = Color.white;
                        GUILayout.Space(2);
                    }
                }

                GUI.enabled = true;
                GUILayout.Space(2);
            }

            GUILayout.Space(2);
        }
    }

    /// <summary>
    /// Draw Background Box
    /// </summary>
    /// <param name="serializedObject"></param>
    private static void DrawBackgroundBox(SerializedObject serializedObject) {
        using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
            backUpStyle = new GUIStyle(EditorStyles.label);
            newStyle = new GUIStyle(EditorStyles.label) {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 11
            };
            GUILayout.Label("Custom Background Color :", newStyle);
            EditorStyles.label.alignment = backUpStyle.alignment;
            EditorStyles.label.fontSize = backUpStyle.fontSize;

            using (new GUILayout.HorizontalScope()) {
                serializedObject.FindProperty("useDarkerBackground").boolValue = GUILayout.Toggle(
                    serializedObject.FindProperty("useDarkerBackground").boolValue, GUIContent.none,
                    GUILayout.Width(15));
                DrawProperty(serializedObject.FindProperty("darkerBackground"), "Darker Background",
                    serializedObject.FindProperty("useDarkerBackground").boolValue, 19);
            }

            using (new GUILayout.HorizontalScope()) {
                serializedObject.FindProperty("useColorFullBackground").boolValue = GUILayout.Toggle(
                    serializedObject.FindProperty("useColorFullBackground").boolValue, GUIContent.none,
                    GUILayout.Width(15));
                DrawProperty(serializedObject.FindProperty("backgroundGroupA"), "Background Color Group A",
                    serializedObject.FindProperty("useColorFullBackground").boolValue, 19);
                DrawProperty(serializedObject.FindProperty("backgroundGroupB"), "Background Color Group B",
                    serializedObject.FindProperty("useColorFullBackground").boolValue, 10);
            }

            GUILayout.Space(2);
        }
    }

    #endregion DrawPropertyMethods

    /// <summary>
    /// Draw a property based on his property and a label
    /// </summary>
    /// <param name="prop"></param>
    /// <param name="label"></param>
    private static void DrawProperty(SerializedProperty prop, string label, bool enabled, int reduceWidth = 0) {
        GUI.enabled = enabled;
        using (new GUILayout.HorizontalScope()) {
            GUILayout.Label(label, GUILayout.Width(200 - reduceWidth));
            EditorGUILayout.PropertyField(prop, GUIContent.none, true);
        }

        GUI.enabled = true;
    }

    /// <summary>
    /// Return the list of all the contents of the gameObjects selected
    /// </summary>
    /// <returns></returns>
    private static List<string> GetGUIContentFromActiveGameObject() {
        if (Selection.activeGameObject == null) {
            EditorUtility.DisplayDialog("Error when adding Icon name", "You need to select one or more GameObject in the hierarchy to use this button.It will automatically add the Icon Name to the list of unwanted icons", "close window");
            return null;
        }

        List<GameObject> selectedGam = new List<GameObject>(Selection.gameObjects);
        List<string> selectedGamContent = selectedGam.Select(gam => EditorGUIUtility.ObjectContent(gam, null).image.name).ToList();
        return selectedGamContent;
    }
}

[Icon("Assets/Art/Sprites/Icon/settingsIcon.png")]
[Overlay(typeof(SceneView), overlayID, overlayName)]
public class CustomHierarchyOverlay : IMGUIOverlay{
    private const string overlayID = "my-custom-hierarchy-overlay";
    private const string overlayName = "Custom Hierarchy Overlay";
    
    public override void OnGUI() {
        var settings = new SerializedObject(AssetDatabase.LoadAssetAtPath<HierarchySettingsSO>("Assets/Resources/ScriptableObject/CustomHierarchy.asset"));
        HierarchySettingsProvider.DrawProperty(settings);
    }
}
#endif