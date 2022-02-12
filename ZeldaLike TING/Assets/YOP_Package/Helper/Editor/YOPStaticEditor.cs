using System;
using UnityEditor;
using UnityEngine;

namespace YOPHelper {
    public static class YOPStaticEditor {
        #region VARIABLES

        private static GUIStyle customGuiStyle = new GUIStyle();

        #endregion

        #region PROPERTY METHOD

        /// <summary>
        /// Draw a property
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="prop"></param>
        /// <param name="labelSize"></param>
        /// <param name="drawName"></param>
        /// <param name="helpBoxStyle"></param>
        /// <param name="guiEnabled"></param>
        public static void DrawProperty(string propertyName, SerializedProperty prop, int labelSize = 110, bool drawName = true, bool helpBoxStyle = false, bool guiEnabled = true) {
            GUI.enabled = guiEnabled;

            switch (helpBoxStyle) {
                case true:
                    using (new GUILayout.HorizontalScope(EditorStyles.helpBox)) {
                        DrawPropertyData(propertyName, prop, labelSize, drawName);
                    }

                    break;
                case false:
                    using (new GUILayout.HorizontalScope()) {
                        DrawPropertyData(propertyName, prop, labelSize, drawName);
                    }

                    break;
            }

            GUI.enabled = true;
        }

        /// <summary>
        /// Draw a button and a property on the same line
        /// </summary>
        /// <param name="buttonTxt"></param>
        /// <param name="buttonWidth"></param>
        /// <param name="btnEvent"></param>
        /// <param name="propertyName"></param>
        /// <param name="prop"></param>
        /// <param name="labelSize"></param>
        /// <param name="drawName"></param>
        /// <param name="helpBoxStyle"></param>
        /// <param name="guiEnabled"></param>
        public static void DrawButtonProperty(string buttonTxt, int buttonWidth, Action btnEvent, string propertyName, SerializedProperty prop, int labelSize = 110, bool leftSide = true, bool drawName = true, bool helpBoxStyle = false, bool guiEnabled = true) {
            GUI.enabled = guiEnabled;

            switch (helpBoxStyle) {
                case true:
                    using (new GUILayout.HorizontalScope(EditorStyles.helpBox)) {
                        DrawPropertyData(propertyName, prop, labelSize, drawName, buttonTxt, buttonWidth, btnEvent, leftSide);
                    }

                    break;
                case false:
                    using (new GUILayout.HorizontalScope()) {
                        DrawPropertyData(propertyName, prop, labelSize, drawName, buttonTxt, buttonWidth, btnEvent, leftSide);
                    }

                    break;
            }

            GUI.enabled = true;
        }

        /// <summary>
        /// Draw the property
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="prop"></param>
        /// <param name="labelSize"></param>
        /// <param name="drawName"></param>
        /// <param name="buttonTxt"></param>
        /// <param name="buttonWidth"></param>
        /// <param name="btnEvent"></param>
        private static void DrawPropertyData(string propertyName, SerializedProperty prop, int labelSize, bool drawName, string buttonTxt = "", int buttonWidth = 0, Action btnEvent = null, bool leftSide = true) {
            if (btnEvent != null) {
                switch (leftSide) {
                    case true:
                        if (GUILayout.Button(buttonTxt, GUILayout.Width(buttonWidth))) {
                            btnEvent.Invoke();
                        }

                        if (drawName) GUILayout.Label(propertyName, GUILayout.Width(labelSize));
                        EditorGUILayout.PropertyField(prop, GUIContent.none, true);
                        break;
                    
                    case false:
                        if (drawName) GUILayout.Label(propertyName, GUILayout.Width(labelSize));
                        EditorGUILayout.PropertyField(prop, GUIContent.none, true);

                        if (GUILayout.Button(buttonTxt, GUILayout.Width(buttonWidth))) {
                            btnEvent.Invoke();
                        }

                        break;
                }
            }
            else {
                if (drawName) GUILayout.Label(propertyName, GUILayout.Width(labelSize));
                EditorGUILayout.PropertyField(prop, GUIContent.none, true);
            }
        }
        
        #endregion

        /// <summary>
        /// Draw a label with a customStyle
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="labelTxt"></param>
        public static void DrawCustomLabel(this string text, int fontSize = 12, FontStyle fontStyle = FontStyle.Normal) {
            customGuiStyle = GUI.skin.label;
            customGuiStyle.fontSize = fontSize;
            customGuiStyle.fontStyle = fontStyle;
            GUILayout.Label(text);
            customGuiStyle.fontSize = 12;
            customGuiStyle.fontStyle = FontStyle.Normal;
        }
    }
}
