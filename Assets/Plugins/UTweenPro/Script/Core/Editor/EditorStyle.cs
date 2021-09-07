#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Aya.TweenPro
{
    public static class EditorStyle
    {
        #region String

        public const string NoneStr = "<None>";

        #endregion

        #region Param

        // public static PropertyInfo ViewWidthProperty = typeof(EditorGUIUtility).GetProperty("contextWidth", BindingFlags.NonPublic | BindingFlags.Static);
        // public static float ViewWidth => (float)ViewWidthProperty.GetValue(null, null);
        public static float ViewWidth => EditorGUIUtility.currentViewWidth;

        public static float LabelWidth => 60f;
        public static float FieldWidth => HalfWidth - LabelWidth;
        public static float HalfWidth => (ViewWidth - 42f) / 2f;

        public static float CharacterWidth = GUI.skin.label.CalcSize(new GUIContent("X")).x;
        public static float SingleButtonWidth = EditorGUIUtility.singleLineHeight;
        public static float SettingButtonWidth = 70f;
        public static float MinWidth => CharacterWidth;

        public static float TweenerHeaderIconSize => SingleButtonWidth * 0.9f;
        public static float HoldButtonWidth => SingleButtonWidth * 0.9f;
        public static float FromToValueLabelWidth => LabelWidth - HoldButtonWidth - 3;

        public static Color ErrorColor = new Color(1f, 0.5f, 0.5f);
        public static Color EnableColor = Color.green * 0.8f;
        public static Color DisableColor = Color.gray * 1.5f;

        public static Color ProgressBackColor = Color.black * 0.65f;
        public static Color ProgressValueColor = Color.cyan * 0.85f;
        public static Color ProgressDisableColor = Color.black * 0.65f;
        public static Color ProgressOutOfRangeColor = Color.green * 0.5f;
        public static Color ProgressInRangeColor = Color.green * 0.85f;

        #endregion

        #region GUIStyle

        public static GUIStyle RichLabel
        {
            get
            {
                if (_richLabel == null)
                {
                    _richLabel = EditorStyles.label;
                    _richLabel.richText = true;
                }

                return _richLabel;
            }
        }

        private static GUIStyle _richLabel;

        #endregion

        #region GUIContent

        public static GUIContent PlayButton = new GUIContent(EditorIcon.GetIcon("PlayButton"));
        public static GUIContent PlayButtonOn = new GUIContent(EditorIcon.GetIcon("PlayButton On"));

        #endregion
    }
}
#endif