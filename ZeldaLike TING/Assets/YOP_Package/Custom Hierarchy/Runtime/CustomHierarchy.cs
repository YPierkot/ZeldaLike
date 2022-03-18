#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using CustomizeEditor.HierarchySO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Component = UnityEngine.Component;

[InitializeOnLoad]
public class CustomHierarchy {
    #region Variables

    //Const Size
    private const int leftSizeHierarchy = 28;
    private const int rightSizeHierarchy = 16;
    private const int childTabulationSizeHierarchy = 14;
    private const int gameObjectIconSize = 17;
    private const int leftColumnHierarchy = 32;
    private const int horizontalLineSize = 9;
    private const int leftWidthIconsComponent = 70;

    //ScriptableObject
    private static HierarchySettingsSO data;

    //Modified Data
    private static List<InstanceInfo> sceneGamInformations = new List<InstanceInfo>();
    private static bool changeBackgroundColor;
    private static InstanceInfo actualInstanceData;
    private static InstanceInfo lastInstanceData;

    private static int lastGroupBackground;
    private static bool changeBackgroundGroupColor;

    private static GameObject mouseObjectDown;
    private static GameObject forceHoverObject;
    private static Event ev;

    private static GUIStyle backUpStyle;

    private static List<CustomRect> rectToDraw = new List<CustomRect>();
    private static int whichInstanceDraw;

    private static bool lastItem = true;

    #endregion Variables

    /// <summary>
    /// Called when need Initialisation
    /// </summary>
    private static void CustomHierarchyInitialisation(bool forceUse) {
        while (true) {
            data = (HierarchySettingsSO) DoesScriptableExist();

            if (data == null) return;

            if (data.UseCustomHierarchy && !forceUse) {
                GetDataFromScene();
                Selection.selectionChanged += SetNewMouseDownObject;
                EditorApplication.hierarchyChanged += GetDataFromScene;
                EditorApplication.hierarchyWindowItemOnGUI += DrawCustomHierarchy;
            }
            else {
                Selection.selectionChanged -= SetNewMouseDownObject;
                EditorApplication.hierarchyChanged -= GetDataFromScene;
                EditorApplication.hierarchyWindowItemOnGUI -= DrawCustomHierarchy;
            }

            if (forceUse) {
                forceUse = false;
                continue;
            }

            break;
        }
    }

    /// <summary>
    /// Call the function to draw the Inspector properly
    /// </summary>
    /// <param name="instanceID"></param>
    /// <param name="selectionRect"></param>
    private static void DrawCustomHierarchy(int instanceID, Rect selectionRect) {
        //Set settings icon
        if (EditorUtility.InstanceIDToObject(instanceID) == null) {
            changeBackgroundColor = false;
            changeBackgroundGroupColor = false;
            lastGroupBackground = 0;
            DrawSettingsIcon(selectionRect);
        }

        if (lastItem) {
            lastItem = false;
            changeBackgroundColor = false;
            changeBackgroundGroupColor = false;
            lastGroupBackground = 0;
        }
        else {
            if (instanceID == sceneGamInformations[^1].instanceID) lastItem = true;
        }
        
        if (!data.ActivScene.Contains(AssetDatabase.LoadAssetAtPath<SceneAsset>(SceneManager.GetActiveScene().path))) return;
        
        lastInstanceData = actualInstanceData;
        actualInstanceData = GetActualInstance(instanceID);

        CheckMouseState();
        getActivWindowName();

        Rect backgroundRect = AlternateBackgroundColor(selectionRect);
        actualInstanceData.instanceRect = backgroundRect;

        drawIconsManager(selectionRect);
        DrawTree(backgroundRect);
    }

    #region InitMethods

    /// <summary>
    /// Function called when unity Loads (when open, after saving, ...)
    /// </summary>
    static CustomHierarchy() => CustomHierarchyInitialisation(true);
    
    public static void ReloadData() {
        EditorApplication.hierarchyChanged -= GetDataFromScene;
        EditorApplication.hierarchyWindowItemOnGUI -= DrawCustomHierarchy;
        
        CustomHierarchyInitialisation(false);
        EditorApplication.RepaintHierarchyWindow();
    }

    #region SCRIPTABLE
    /// <summary>
    /// Check if the ScriptableObject and the folder exist. If not create it
    /// </summary>
    private static object DoesScriptableExist() {
        CreateFolderIfNull("Assets", "Resources");
        CreateFolderIfNull("Assets/Resources", "ScriptableObject");

        HierarchySettingsSO hierarchySO = Resources.Load<HierarchySettingsSO>("ScriptableObject/CustomHierarchy");
        if (hierarchySO != null) return hierarchySO;
        
        hierarchySO = ScriptableObject.CreateInstance<HierarchySettingsSO>();
        AssetDatabase.CreateAsset(hierarchySO, "Assets/Resources/ScriptableObject/CustomHierarchy.asset");
        hierarchySO = Resources.Load<HierarchySettingsSO>("ScriptableObject/CustomHierarchy");
        
        return hierarchySO;
    }

    /// <summary>
    /// Create a folder if it doesn't exist
    /// </summary>
    /// <param name="parentFolderPath"></param>
    /// <param name="folderToCheck"></param>
    private static void CreateFolderIfNull(string parentFolderPath, string folderToCheck) {
        if (!AssetDatabase.IsValidFolder($"{parentFolderPath}/{folderToCheck}"))
            AssetDatabase.CreateFolder(parentFolderPath, folderToCheck);
    }
    #endregion SCRIPTABLE
    
    #endregion InitMethods

    #region globalData

    /// <summary>
    /// Check if the mouse click somewhere else than a gameObject
    /// </summary>
    private static void CheckMouseState() {
        ev = Event.current;
        mouseObjectDown = ev.type switch {
            EventType.MouseDown => null,
            _ => mouseObjectDown
        };
    }

    /// <summary>
    /// Called when Selection changed. Changed the selected gameObject in the hierarchy
    /// </summary>
    private static void SetNewMouseDownObject() {
        mouseObjectDown = Selection.activeGameObject;
    }

    /// <summary>
    /// SelectedGam return null if the active window isn't hierarchy
    /// </summary>
    private static void getActivWindowName() {
        if (EditorWindow.focusedWindow == null) return;
        if (EditorWindow.focusedWindow.ToString() != " (UnityEditor.SceneHierarchyWindow)") {
            if (mouseObjectDown != null) {
                forceHoverObject = mouseObjectDown;
                mouseObjectDown = null;
            }

            if (Selection.activeGameObject == null) forceHoverObject = null;
        }
        else if (EditorWindow.focusedWindow.ToString() == " (UnityEditor.SceneHierarchyWindow)") {
            forceHoverObject = null;
        }
    }

    #endregion globalData

    #region DrawBackground

    /// <summary>
    /// Draw a darker Background every two GameObjects
    /// </summary>
    /// <param name="instanceID"></param>
    /// <param name="selectionRect"></param>
    private static Rect AlternateBackgroundColor(Rect selectionRect) {
        //Draw background Rect
        Rect backgroundRect = new Rect() {
            x = selectionRect.x - leftSizeHierarchy - childTabulationSizeHierarchy * actualInstanceData.nestingLevel,
            y = selectionRect.y,
            width = selectionRect.width + leftSizeHierarchy + rightSizeHierarchy + childTabulationSizeHierarchy * actualInstanceData.nestingLevel,
            height = selectionRect.height
        };

        //Draw SeparatorRect
        DrawSeparator(backgroundRect);

        //Draw a foreground Over the text to hide it
        DrawForegroundtext(selectionRect, backgroundRect);

        //Draw the colorBackground if the gameObject has children
        if (!actualInstanceData.isSeparator && data.UseColorFullBackground) DrawGroupBackground(backgroundRect);

        //Draw the background to see the alternation between one and the next gameObject
        if (changeBackgroundColor && !actualInstanceData.isSeparator && data.UseDarkerBackground &&
            actualInstanceData.name != " ") EditorGUI.DrawRect(backgroundRect, data.DarkerBackground);


        //Draw the text
        DrawText(selectionRect, backgroundRect);

        changeBackgroundColor = !changeBackgroundColor;
        return backgroundRect;
    }

    /// <summary>
    /// Draw the a rect behind the text to hide it
    /// </summary>
    /// <param name="foregroundRect"></param>
    /// <param name="instanceRect"></param>
    private static void DrawForegroundtext(Rect foregroundRect, Rect instanceRect) {
        if (EditorUtility.InstanceIDToObject(actualInstanceData.instanceID) == null) return;

        //Basic Rect rect
        Color overTextCol = new Color(57 / 256f, 57 / 256f, 57 / 256f, 1);
        if (!actualInstanceData.isSeparator && actualInstanceData.gam.name != " ")
            EditorGUI.DrawRect(foregroundRect, overTextCol);
        //Rect for GameObject before Separator
        else if (actualInstanceData.gam.name == " ") {
            overTextCol = new Color(57 / 256f, 57 / 256f, 57 / 256f, 1);
            Rect sepRect = new Rect() {
                x = instanceRect.x - leftSizeHierarchy - 5,
                y = instanceRect.y,
                width = instanceRect.width + leftSizeHierarchy + 5,
                height = instanceRect.height
            };
            EditorGUI.DrawRect(sepRect, overTextCol);
            foregroundRect = sepRect;
        }
        else foregroundRect = instanceRect;

        //When a gameObject is selected
        if (mouseObjectDown == actualInstanceData.gam) {
            overTextCol = new Color(42 / 256f, 97 / 256f, 144 / 256f, .9f);
            EditorGUI.DrawRect(foregroundRect, overTextCol);
            return;
        }

        //When hover a gameObject
        if (instanceRect.Contains(Event.current.mousePosition)) {
            //Check if a gameObject is selected
            if (ev.type == EventType.MouseDown) mouseObjectDown = actualInstanceData.gam;

            overTextCol = new Color(67 / 256f, 67 / 256f, 67 / 256f, 1);
            EditorGUI.DrawRect(foregroundRect, overTextCol);
            overTextCol = new Color(.8f, .8f, .8f, .1f);
            EditorGUI.DrawRect(instanceRect, overTextCol);
        }
        else if (forceHoverObject == actualInstanceData.gam) {
            overTextCol = new Color(75 / 256f, 75 / 256f, 75 / 256f, 1);
            EditorGUI.DrawRect(foregroundRect, overTextCol);
            overTextCol = new Color(.8f, .8f, .8f, .1f);
            EditorGUI.DrawRect(instanceRect, overTextCol);
        }
    }

    /// <summary>
    /// Draw an additionalBackground Color for gameObject which got children
    /// </summary>
    /// <param name="instanceRect"></param>
    private static void DrawGroupBackground(Rect instanceRect) {
        if (EditorUtility.InstanceIDToObject(actualInstanceData.instanceID) == null) return;

        //If the gameObject has no child and he doesn't belong to the lastGroup value
        if (actualInstanceData.gam.transform.childCount == 0 && (actualInstanceData.nestingGroup != lastGroupBackground || EditorUtility.InstanceIDToObject(lastInstanceData.instanceID) == null)) return;

        if (actualInstanceData.nestingGroup != lastGroupBackground) changeBackgroundGroupColor = !changeBackgroundGroupColor;

        Color colorToUse = changeBackgroundGroupColor ? data.BackgroundGroupA : data.BackgroundGroupB;

        EditorGUI.DrawRect(instanceRect, colorToUse);
        lastGroupBackground = actualInstanceData.nestingGroup;
    }

    /// <summary>
    /// if the gameObject get the tag of the separator it will apply a opaque color on the entire instance
    /// </summary>
    /// <param name="instanceRect"></param>
    private static void DrawSeparator(Rect instanceRect) {
        if (EditorUtility.InstanceIDToObject(actualInstanceData.instanceID) == null || !data.UseSeparator) return;

        if (actualInstanceData.gam.CompareTag(data.SeparatorTag)) {
            EditorGUI.DrawRect(instanceRect, data.SeparatorColor);
            actualInstanceData.isSeparator = true;
            CheckLastInstance();
            changeBackgroundColor = true;
        }
        else {
            string[] endName = actualInstanceData.gam.name.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
            if (endName.Length == 0) return;
            if (endName[^1] == "(hasSpawnGam)") {
                string objName = endName.Where(name => name != "(hasSpawnGam)").Aggregate("", (current, name) => current + name);
                actualInstanceData.gam.name = objName;

                if (lastInstanceData.gam.name == " ") UnityEngine.Object.DestroyImmediate(lastInstanceData.gam);
            }
        }

        if (lastInstanceData.gam == null) return;
        if (lastInstanceData.gam.name == " ") {
            Rect sepRect = new Rect() {
                x = instanceRect.x - leftSizeHierarchy - 5,
                y = instanceRect.y - instanceRect.height,
                width = instanceRect.width + leftSizeHierarchy + 5,
                height = data.SeparatorLineSize
            };
            EditorGUI.DrawRect(sepRect, data.SeparatorColor);
        }
    }

    /// <summary>
    /// Check if the actual Instance is a gameObject => (true) then create a empty gameObject before to make a space.
    /// </summary>
    /// <param name="instanceRect"></param>
    private static void CheckLastInstance() {
        if (EditorUtility.InstanceIDToObject(lastInstanceData.instanceID) == null) return;

        string[] endName = actualInstanceData.gam.name.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);

        if (endName[endName.Length - 1] != "(hasSpawnGam)") CreateNewGam();
    }

    /// <summary>
    /// Create a new basic object
    /// </summary>
    private static void CreateNewGam() {
        GameObject gam = new GameObject {
            name = " "
        };
        gam.transform.SetSiblingIndex(actualInstanceData.nestingGroup);
        actualInstanceData.gam.name = actualInstanceData.gam.name + " (hasSpawnGam)";
        actualInstanceData.previousGamSeparator = gam;
    }

    /// <summary>
    /// Draw the text of the GameObject
    /// </summary>
    /// <param name="selectionRect"></param>
    private static void DrawText(Rect selectionRect, Rect instanceRect) {
        if (EditorUtility.InstanceIDToObject(actualInstanceData.instanceID) == null) return;

        selectionRect.position += new Vector2(gameObjectIconSize, -1);
        backUpStyle = new GUIStyle(EditorStyles.label);
        GUIStyle newStyle = new GUIStyle(EditorStyles.label) {
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };

        string finalName = actualInstanceData.name;
        if (actualInstanceData.isSeparator) {
            string[] endName = actualInstanceData.gam.name.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
            string objName = endName.Where(name => name != "(hasSpawnGam)").Aggregate("", (current, name) => current + (current != ""? " " : "") + name);
            finalName = objName;
            GUI.Label(instanceRect, objName.ToUpper(), newStyle);
        }
        else GUI.Label(selectionRect, finalName, backUpStyle);

        //Draw strikeout
        if (!actualInstanceData.gam.activeSelf) EditorGUI.DrawRect(new Rect(selectionRect.x, selectionRect.y + selectionRect.height / 2 + 1, GUI.skin.label.CalcSize(new GUIContent(finalName)).x - 1, 1), new Color(1,1,1,.75f));

        EditorStyles.label.alignment = backUpStyle.alignment;
        EditorStyles.label.fontStyle = backUpStyle.fontStyle;
    }

    #endregion DrawBackground

    #region DrawIcons
    /// <summary>
    /// Method Manager to draw all the icon
    /// </summary>
    /// <param name="selectionRect"></param>
    private static void drawIconsManager(Rect selectionRect) {
        if (actualInstanceData.isSeparator) return;
        if (data.UseIconGam) DrawObjectIcon(selectionRect);
        if (data.UseIconComponent) DrawComponentIcons(selectionRect);
    }

    /// <summary>
    /// Draw the custom Icon of the gameObject if he gets one
    /// </summary>
    /// <param name="selectionRect"></param>
    private static void DrawObjectIcon(Rect selectionRect) {
        GUIContent content = EditorGUIUtility.ObjectContent(actualInstanceData.gam, null);
        Rect iconRect = new Rect() {
            x = selectionRect.x + 1,
            y = selectionRect.y + 1,
            width = selectionRect.height - 2,
            height = selectionRect.height - 2
        };

        if (content.image != null) {
            if (!data.IgnoreIconsName.Contains(content.image.name)) {
                GUI.DrawTexture(iconRect, content.image);
            }
        }
    }

    /// <summary>
    /// Draw icons for each component in the object
    /// </summary>
    /// <param name="selectionRect"></param>
    private static void DrawComponentIcons(Rect selectionRect) {
        if (actualInstanceData.gam == null) return;
        List<Component> componentsList = new List<Component>(actualInstanceData.gam.GetComponents(typeof(Component)));

        Rect iconRect = new Rect() {
            x = selectionRect.x + selectionRect.width - leftWidthIconsComponent,
            y = selectionRect.y - 1,
            width = selectionRect.height + 2,
            height = selectionRect.height + 2
        };
        
        foreach (Component c in componentsList) {
            if (c == null) {
                GUI.Label(iconRect, new GUIContent(EditorGUIUtility.IconContent("Error@2X").image, "Script is empty. Please remove it!"));
                iconRect.x += selectionRect.height + 1;
            }
            else {
                if (data.DrawMonobehaviourScriptIcon && c.GetType().IsSubclassOf(typeof(MonoBehaviour))) {
                    string componentName = c.GetType().ToString();
                    if (!componentName.StartsWith("UnityEngine.")) {
                        GUI.Label(iconRect, new GUIContent(EditorGUIUtility.ObjectContent(c, Type.GetType(componentName)).image, componentName));
                        iconRect.x += selectionRect.height + 1;
                    }
                }
                else if (data.ComponentIcontype.Contains(c.GetType().ToString())) {
                    string componentName = c.GetType().ToString();
                    string tooltipName = componentName.StartsWith("UnityEngine.") ? componentName.Split(new[] {"UnityEngine."}, StringSplitOptions.None)[1] : componentName;
                    GUI.Label(iconRect, new GUIContent(EditorGUIUtility.ObjectContent(c, Type.GetType(componentName)).image, tooltipName));
                    iconRect.x += selectionRect.height + 1;
                }
            }
        }
    }

    #endregion DrawIcons

    #region DrawTree

    /// <summary>
    /// Draw the tree branch to have a better vision of the hierarchy
    /// </summary>
    /// <param name="instanceRect"></param>
    private static void DrawTree(Rect instanceRect) {
        if (!data.UseTreeBranch) return;
        DrawHorizontalLine(instanceRect);
        DrawLastHorizontalLine();
        
        if (actualInstanceData.nestingLevel == 0) return;
        DrawVerticalLine(instanceRect);
    }

    /// <summary>
    /// Draw HorizontalLine for the tree Hierarchy
    /// </summary>
    /// <param name="instanceRect"></param>
    private static void DrawHorizontalLine(Rect instanceRect) {
        if (actualInstanceData.gam == null) return;

        Color col = changeBackgroundGroupColor ? data.TreeColorListA[(int) Mathf.Clamp((actualInstanceData.nestingLevel - 1), 0, Mathf.Infinity) % data.TreeColorListA.Count] : data.TreeColorListB[(int) Mathf.Clamp((actualInstanceData.nestingLevel - 1), 0, Mathf.Infinity) % data.TreeColorListB.Count];

        if (!data.UseComplexColor) col = Color.white;

        //Draw The closest Horizontal Line to the Object
        if (actualInstanceData.nestingLevel != 0) {
            Rect horizontalLineRect = new Rect() {
                x = leftColumnHierarchy + 20 + (actualInstanceData.nestingLevel - 1) * childTabulationSizeHierarchy,
                y = instanceRect.y + instanceRect.height / 2,
                width = actualInstanceData.gam.transform.childCount != 0 ? horizontalLineSize : childTabulationSizeHierarchy, height = data.TreeSize};

            EditorGUI.DrawRect(horizontalLineRect, col);
        }

        //Draw Horizontal Line for previous Object in Hierarchy
        if (EditorUtility.InstanceIDToObject(lastInstanceData.instanceID) != null) {
            for (int i = 0; i < lastInstanceData.nestingLevel - (actualInstanceData.nestingLevel + 1); i++) {
                if (actualInstanceData.name == " ") col = changeBackgroundGroupColor ? data.TreeColorListA[(i + actualInstanceData.nestingLevel) % data.TreeColorListA.Count] : data.TreeColorListB[(i + actualInstanceData.nestingLevel) % data.TreeColorListB.Count];
                else col = changeBackgroundGroupColor ? data.TreeColorListA[(i + actualInstanceData.nestingLevel) % data.TreeColorListA.Count] : data.TreeColorListB[(i + actualInstanceData.nestingLevel) % data.TreeColorListB.Count];
                
                if (!data.UseComplexColor) col = Color.white;

                Rect newHorizontalLineRect = new Rect() {
                    x = leftColumnHierarchy + 20 + (actualInstanceData.nestingLevel + i) * childTabulationSizeHierarchy,
                    y = instanceRect.y - instanceRect.height / 2,
                    width = childTabulationSizeHierarchy,
                    height = data.TreeSize
                };
                EditorGUI.DrawRect(newHorizontalLineRect, col);
            }
        }

        //Draw Horizontal Line if last object in hierarchy
        if (actualInstanceData.instanceID == whichInstanceDraw) {
            foreach (CustomRect r in rectToDraw) {
                EditorGUI.DrawRect(r.rectToDraw, r.colorToApply);
            }
        }
    }

    /// <summary>
    /// Draw the last horizontal line of the list
    /// </summary>
    private static void DrawLastHorizontalLine() {
        if (lastInstanceData.gam == null) return;
        
        if (EditorUtility.InstanceIDToObject(actualInstanceData.instanceID) == null) {
            whichInstanceDraw = lastInstanceData.instanceID;
            rectToDraw.Clear();
            for (int i = 0; i < lastInstanceData.nestingLevel - 1; i++) {
                Color col = !changeBackgroundGroupColor ? data.TreeColorListA[(i + actualInstanceData.nestingLevel) % data.TreeColorListA.Count] : data.TreeColorListB[(i + actualInstanceData.nestingLevel) % data.TreeColorListB.Count];
                if (!data.UseComplexColor) col = Color.white;

                Rect newHorizontalLineRect = new Rect() {
                    x = leftColumnHierarchy + 20 + i * childTabulationSizeHierarchy,
                    y = lastInstanceData.instanceRect.y + lastInstanceData.instanceRect.height / 2,
                    width = childTabulationSizeHierarchy,
                    height = data.TreeSize
                };

                rectToDraw.Add(new CustomRect(newHorizontalLineRect, col));
            }
        }
    }

    /// <summary>
    /// Draw VerticalLine for the tree Hierarchy
    /// </summary>
    /// <param name="instanceRect"></param>
    private static void DrawVerticalLine(Rect instanceRect) {
        if (actualInstanceData.gam == null) return;

        Color col = changeBackgroundGroupColor ? data.TreeColorListA[0] : data.TreeColorListB[0];
        if (!data.UseComplexColor) col = Color.white;

        Rect verticalLineRect = new Rect() {
            x = leftColumnHierarchy + 20,
            y = instanceRect.y - (lastInstanceData.nestingGroup == actualInstanceData.nestingGroup
                ? (lastInstanceData.nestingLevel != 0 ? instanceRect.height / 2 : 0)
                : 0),
            width = data.TreeSize,
            height = instanceRect.height / 2 + (lastInstanceData.nestingGroup == actualInstanceData.nestingGroup
                ? (lastInstanceData.nestingLevel != 0 ? instanceRect.height / 2 : 0)
                : 0)
        };
        EditorGUI.DrawRect(verticalLineRect, col);

        for (int i = 1; i < actualInstanceData.nestingLevel; i++) {
            col = changeBackgroundGroupColor
                ? data.TreeColorListA[i % data.TreeColorListA.Count]
                : data.TreeColorListB[i % data.TreeColorListB.Count];
            if (!data.UseComplexColor) col = Color.white;

            Rect newVerticalLineRect = new Rect() {
                x = verticalLineRect.x + i * childTabulationSizeHierarchy,
                y = instanceRect.y - (lastInstanceData.nestingLevel != actualInstanceData.nestingLevel
                    ? (i == lastInstanceData.nestingLevel ? 0 : instanceRect.height / 2)
                    : instanceRect.height / 2),
                width = data.TreeSize,
                height = instanceRect.height / 2 + (lastInstanceData.nestingLevel != actualInstanceData.nestingLevel
                    ? (i == lastInstanceData.nestingLevel ? 0 : instanceRect.height / 2)
                    : instanceRect.height / 2)
            };
            EditorGUI.DrawRect(newVerticalLineRect, col);
        }
    }

    #endregion DrawTree

    #region GetSceneInformations

    /// <summary>
    /// get sceneRoots GameObject which will allow us to get all the sceneObjects information
    /// </summary>
    private static void GetDataFromScene() {
        sceneGamInformations.Clear();
        actualInstanceData = new InstanceInfo();

        List<GameObject> sceneRootsGameObject = new List<GameObject>();
        for (int i = 0; i < SceneManager.sceneCount; i++) {
            sceneRootsGameObject.AddRange(SceneManager.GetSceneAt(i).GetRootGameObjects());
        }
        
        for (int i = 0; i < sceneRootsGameObject.Count; i++) {
            AddInstanceInfo(sceneRootsGameObject[i], sceneRootsGameObject[i].GetInstanceID(), 0, i);
        }
    }

    /// <summary>
    /// Get all the information about all gameObjects in the scene
    /// </summary>
    /// <param name="gam"></param>
    /// <param name="instanceID"></param>
    /// <param name="nestingLevel"></param>
    /// <param name="nestingGroup"></param>
    private static void AddInstanceInfo(GameObject gam, int instanceID, int nestingLevel, int nestingGroup) {
        InstanceInfo instance = new InstanceInfo() {
            name = gam.name,
            instanceID = instanceID,
            gam = gam,
            nestingGroup = nestingGroup,
            nestingLevel = nestingLevel,
        };

        if (gam != null) {
            if (gam.transform.parent != null) {
                instance.parentGam = gam.transform.parent.gameObject;
                instance.isLastChild =
                    gam.transform.parent.GetChild(gam.transform.parent.childCount - 1).gameObject == gam;
            }
            else instance.isLastChild = false;
        }
        else instance.isLastChild = false;

        sceneGamInformations.Add(instance);

        int childCount = gam.transform.childCount;
        for (int x = 0; x < childCount; x++) {
            AddInstanceInfo(gam.transform.GetChild(x).gameObject, gam.transform.GetChild(x).gameObject.GetInstanceID(),
                nestingLevel + 1, nestingGroup);
        }
    }

    /// <summary>
    /// Return the InstanceInfo link to the instanceID in parameter
    /// </summary>
    /// <param name="instanceID"></param>
    /// <returns></returns>
    private static InstanceInfo GetActualInstance(int instanceID) {
        for (int i = 0; i < sceneGamInformations.Count; i++) {
            if (sceneGamInformations[i].instanceID == instanceID) return sceneGamInformations[i];
        }

        return new InstanceInfo("");
    }

    #endregion GetSceneInformations

    #region DrawSettingsIcon

    /// <summary>
    /// Draw data in the first Instance which is the scene name
    /// </summary>
    /// <param name="selectionRect"></param>
    private static void DrawSettingsIcon(Rect selectionRect) {
        Rect buttonRect = new Rect() {
            x = selectionRect.x + selectionRect.width - 20,
            y = selectionRect.y,
            width = 15,
            height = 15
        };
        GUIStyle buttonStyle = new GUIStyle(EditorStyles.label) {
            padding = new RectOffset(0, 0, 0, 0),
            alignment = TextAnchor.LowerCenter
        };


        if (GUI.Button(buttonRect, EditorGUIUtility.IconContent("d_SettingsIcon"), buttonStyle)) {
            SettingsService.OpenProjectSettings("Project/Custom Hierarchy");
        }
    }

    #endregion DrawSettingsIcon
}

#region CustomClass

/// <summary>
/// Custom Information of each object in the scene
/// </summary>
public struct InstanceInfo {
    //BASIC DATA
    public string name;
    public int instanceID;
    public GameObject gam;

    //GROUP/PARENT DATA
    public int nestingGroup;
    public int nestingLevel;
    public GameObject parentGam;
    public bool isLastChild;

    //OTHER DATA
    public bool isSeparator;
    public GameObject previousGamSeparator;
    public Rect instanceRect;

    public InstanceInfo(string name) {
        this.name = name;
        instanceID = 0;
        gam = null;
        nestingGroup = 0;
        nestingLevel = 0;
        parentGam = null;
        isLastChild = false;
        isSeparator = false;
        previousGamSeparator = null;
        instanceRect = default;
    }
}

[Serializable]
public class CustomRect {
    public Rect rectToDraw;
    public Color colorToApply;

    public CustomRect(Rect rectToDraw, Color colorToApply) {
        this.rectToDraw = rectToDraw;
        this.colorToApply = colorToApply;
    }
}

#endregion CustomClass

#endif