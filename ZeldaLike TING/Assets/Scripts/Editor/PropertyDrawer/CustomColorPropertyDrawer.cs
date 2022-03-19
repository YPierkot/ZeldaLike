using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ColorAttribute))]
public class CustomColorPropertyDrawer : PropertyDrawer {
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
        ColorAttribute attributeVar = (ColorAttribute) attribute;

        switch (attributeVar.guiType) {
            case GuiType.Text:
                GUI.contentColor = attributeVar.newGUIColor;
                break;
            case GuiType.Background:
                GUI.backgroundColor = attributeVar.newGUIColor;
                break;
            case GuiType.All:
                GUI.color = attributeVar.newGUIColor;
                break;
        }
        
        EditorGUI.BeginProperty(rect, label, property);
        EditorGUI.PropertyField(rect, property, label, true);
        EditorGUI.EndProperty();
        
        GUI.color = Color.white;
    }
}
