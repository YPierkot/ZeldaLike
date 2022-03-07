#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using YOPHelper;

[CustomEditor(typeof(TrelloSO))]
public class TrelloSOEditor : Editor {
    private TrelloSO script => ((TrelloSO) target);
    private bool canDrawListID = false;
    
    /// <summary>
    /// Draw the properties
    /// </summary>
    public override void OnInspectorGUI() {
        serializedObject.Update();
        canDrawListID = serializedObject.FindProperty("trelloKey").stringValue != "" && serializedObject.FindProperty("userToken").stringValue != "" && serializedObject.FindProperty("boardId").stringValue != "";
        
        GUIStyle helpboxStyle = new GUIStyle(EditorStyles.helpBox) {
            padding = new RectOffset(5, 5, 1, 5)
        };
        
        using (new EditorGUILayout.VerticalScope(helpboxStyle)) {
            "TRELLO DATA :".DrawCustomLabel(9, FontStyle.Bold);

            //TRELLO KEY
            if (serializedObject.FindProperty("trelloKey").stringValue == "") YOPStaticEditor.DrawButtonProperty("+", 20, script.OpenTrelloKeyURL, "Trello Key", serializedObject.FindProperty("trelloKey"));
            
            //USER TOKEN
            else if (serializedObject.FindProperty("userToken").stringValue == "") {
                using (new GUILayout.HorizontalScope()) {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(EditorGUIUtility.IconContent("Toggle Icon"), GUILayout.Width(22), GUILayout.Height(22));

                    using (new EditorGUILayout.VerticalScope()) {
                        GUILayout.FlexibleSpace();
                        GUILayout.Space(4);
                        "Trello Key".ToUpper().DrawCustomLabel(12, FontStyle.Bold);
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.FlexibleSpace();
                }

                YOPStaticEditor.DrawButtonProperty("+", 20, script.OpenUserTokenURL, "User Token", serializedObject.FindProperty("userToken"));
            }
            
            //BOARD ID
            else if(serializedObject.FindProperty("boardId").stringValue == "") {
                using (new GUILayout.HorizontalScope()) {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(EditorGUIUtility.IconContent("Toggle Icon"), GUILayout.Width(22), GUILayout.Height(22));
                    
                    using (new EditorGUILayout.VerticalScope()) {
                        GUILayout.FlexibleSpace();
                        GUILayout.Space(4);
                        "Trello Key".ToUpper().DrawCustomLabel(12, FontStyle.Bold);
                        GUILayout.FlexibleSpace();
                    }
                    
                    GUILayout.Label(EditorGUIUtility.IconContent("Toggle Icon"), GUILayout.Width(22), GUILayout.Height(22));
                    
                    using (new EditorGUILayout.VerticalScope()) {
                        GUILayout.FlexibleSpace();
                        GUILayout.Space(4);
                        "User Token".ToUpper().DrawCustomLabel(12, FontStyle.Bold);
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.FlexibleSpace();
                }
                
                YOPStaticEditor.DrawProperty("Board ID", serializedObject.FindProperty("boardId"));
                YOPStaticEditor.DrawButtonProperty("Get Board", 80, script.GetAllBoardAvailable, "Username", serializedObject.FindProperty("username"), 110, false);
            }
            //DONE
            else {
                using (new GUILayout.HorizontalScope()) {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(EditorGUIUtility.IconContent("Toggle Icon"), GUILayout.Width(22), GUILayout.Height(22));
                    
                    using (new EditorGUILayout.VerticalScope()) {
                        GUILayout.FlexibleSpace();
                        GUILayout.Space(4);
                        "Trello Key".ToUpper().DrawCustomLabel(12, FontStyle.Bold);
                        GUILayout.FlexibleSpace();
                    }
                    
                    GUILayout.Label(EditorGUIUtility.IconContent("Toggle Icon"), GUILayout.Width(22), GUILayout.Height(22));
                    
                    using (new EditorGUILayout.VerticalScope()) {
                        GUILayout.FlexibleSpace();
                        GUILayout.Space(4);
                        "User Token".ToUpper().DrawCustomLabel(12, FontStyle.Bold);
                        GUILayout.FlexibleSpace();
                    }
                    
                    GUILayout.Label(EditorGUIUtility.IconContent("Toggle Icon"), GUILayout.Width(22), GUILayout.Height(22));
                    
                    using (new EditorGUILayout.VerticalScope()) {
                        GUILayout.FlexibleSpace();
                        GUILayout.Space(4);
                        "Board ID".ToUpper().DrawCustomLabel(12, FontStyle.Bold);
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.FlexibleSpace();
                }
            }


            if (GUILayout.Button("Reset Trello Data")) {
                Undo.RecordObject(this, "Reset trello data");
                serializedObject.FindProperty("trelloKey").stringValue = "";
                serializedObject.FindProperty("userToken").stringValue = "";
                serializedObject.FindProperty("boardId").stringValue = "";
            }
        }

        GUILayout.Space(4);
        
        if (canDrawListID) {
            GUIStyle helpboxStyle2 = new GUIStyle(EditorStyles.helpBox) {
                padding = new RectOffset(22, 5, 1, 5)
            };
            using (new GUILayout.VerticalScope(helpboxStyle2)) {
                using (new GUILayout.HorizontalScope()) {
                    GUILayout.Space(-18);
                    "TRELLO BOARD LISTS :".DrawCustomLabel(9, FontStyle.Bold); 
                }
                
                SerializedProperty prop = serializedObject.FindProperty("listOfListID").Copy();
                EditorGUILayout.PropertyField(prop, new GUIContent(prop.name), true);
                
                GUILayout.Space(2);

                if (GUILayout.Button("Get Lists ID")) {
                    Application.OpenURL($"https://api.trello.com/1/boards/{script.BoardID}?key={script.TrelloKey}&token={script.UserToken}&lists=all");
                }
                if (GUILayout.Button("Get Labels ID")) {
                    Application.OpenURL($"https://api.trello.com/1/boards/{script.BoardID}?key={script.TrelloKey}&token={script.UserToken}&labels=all");
                }
            }
        }

        
        serializedObject.ApplyModifiedProperties();
    }
}
#endif