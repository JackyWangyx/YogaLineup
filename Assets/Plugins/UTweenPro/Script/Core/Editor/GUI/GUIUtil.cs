#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Aya.TweenPro
{
    public static class GUIUtil
    {
        #region Tip Area

        public static void DrawTipArea(Color color, string tip)
        {
            using (GUIColorArea.Create(color))
            {
                using (GUIGroup.Create())
                {
                    GUILayout.Label(tip, EditorStyle.MultiLineLabel);
                }
            }
        }
        public static void DrawTipArea(string tip)
        {
            using (GUIGroup.Create())
            {
                GUILayout.Label(tip, EditorStyle.MultiLineLabel);
            }
        }

        #endregion

        #region Toggle

        [Obsolete]
        public static void DrawToggleOnOffButton(SerializedProperty property, string enableStr = "On", string disableStr = "Off")
        {
            ToggleOnOffButton(property, property.displayName, enableStr, disableStr);
        }

        [Obsolete]
        public static void ToggleOnOffButton(SerializedProperty property, string propertyName, string enableStr, string disableStr)
        {
            using (GUIHorizontal.Create())
            {
                GUILayout.Label(propertyName, EditorStyles.label, GUILayout.Width(EditorStyle.LabelWidth));
                var btnStyle = EditorStyles.miniButton;
                btnStyle.margin = new RectOffset();
        
                var rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, EditorStyles.miniButton);
                rect.width = (rect.width - EditorStyle.LabelWidth) / 2f;
                var rectOff = rect;
                rectOff.width = rect.width / 2f;
                using (GUIEnableColorArea.Create(property.boolValue))
                {
                    property.boolValue = !GUI.Toggle(rectOff, !property.boolValue, disableStr, btnStyle);
                }
        
                var rectOn = rect;
                rectOn.width = rectOff.width;
                rectOn.x += rectOff.width;
                using (GUIEnableColorArea.Create(property.boolValue))
                {
                    property.boolValue = GUI.Toggle(rectOn, property.boolValue, enableStr, btnStyle);
                }
            }
        }

        public static void DrawToggleButton(SerializedProperty property)
        {
            DrawToggleButton(property, property.displayName);
        }

        public static void DrawToggleButton(SerializedProperty property, string propertyName)
        {
            property.boolValue = DrawToggleButton(propertyName, property.boolValue);
        }

        public static bool DrawToggleButton(string propertyName, bool value)
        {
            return DrawToggleButton(propertyName, value, UTweenEditorSetting.Ins.EnableColor, UTweenEditorSetting.Ins.DisableColor);
        }

        public static bool DrawToggleButton(string propertyName, bool value, Color enableColor, Color disableColor)
        {
            using (GUIHorizontal.Create())
            {
                var btnStyle = EditorStyles.miniButton;
                btnStyle.margin = new RectOffset();

                var rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, EditorStyles.miniButton);
                using (GUIColorArea.Create(enableColor, disableColor, value))
                {
                    var result = GUI.Toggle(rect, value, propertyName, btnStyle);
                    return result;
                }
            }
        }

        #endregion

        #region Enum

        public static void DrawToolbarEnum(SerializedProperty property, Type enumType)
        {
            DrawToolbarEnum(property, property.displayName, enumType);
        }

        public static void DrawToolbarEnum(SerializedProperty property, string propertyName, Type enumType)
        {
            using (GUIHorizontal.Create())
            {
                GUILayout.Label(propertyName, EditorStyles.label, GUILayout.Width(EditorStyle.LabelWidth));
                var buttons = Enum.GetNames(enumType);
                var style = EditorStyles.miniButton;
                style.margin = new RectOffset();
                var rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, style);
                var btnWidth = rect.width / buttons.Length;
                for (var i = 0; i < buttons.Length; i++)
                {
                    var button = buttons[i];
                    var index = i;
                    var btnRect = rect;
                    btnRect.x += i * btnWidth;
                    btnRect.width = btnWidth;
                    using (GUIColorArea.Create(UTweenEditorSetting.Ins.SelectedColor, UTweenEditorSetting.Ins.DisableColor, property.intValue == index))
                    {
                        var btn = GUI.Button(btnRect, button, style);
                        if (btn)
                        {
                            property.intValue = index;
                        }
                    }
                }
            }
        }
        
        #endregion

        #region FromTo Property

        public static void DrawHoldProperty(SerializedProperty holdProperty)
        {
            using (GUIEnableColorArea.Create(holdProperty.boolValue))
            {
                holdProperty.boolValue = GUILayout.Toggle(holdProperty.boolValue, GUIContent.none, EditorStyles.radioButton, GUILayout.Width(EditorStyle.HoldButtonWidth));
            }
        }

        public static float DrawFloatProperty(string name, float value, bool enable)
        {
            using (GUIEnableArea.Create(enable && GUI.enabled))
            {
                value = EditorGUILayout.FloatField(name, value, GUILayout.MinWidth(EditorStyle.MinWidth));
                return value;
            }
        }

        public static int DrawIntProperty(string name, int value, bool enable)
        {
            using (GUIEnableArea.Create(enable && GUI.enabled))
            {
                value = EditorGUILayout.IntField(name, value, GUILayout.MinWidth(EditorStyle.MinWidth));
                return value;
            }
        }

        public static void DrawProperty(SerializedProperty holdProperty, SerializedProperty property)
        {
            using (GUIHorizontal.Create())
            {
                DrawHoldProperty(holdProperty);
                using (GUILabelWidthArea.Create(EditorStyle.FromToValueLabelWidth))
                {
                    EditorGUILayout.PropertyField(property);
                }
            }
        }

        public static void DrawVector2Property(SerializedProperty holdProperty, SerializedProperty property, string name,
            string axis1Name, string axis2Name,
            bool enableAxis1, bool enableAxis2)
        {
            using (GUIHorizontal.Create())
            {
                DrawHoldProperty(holdProperty);
                var value = property.vector2Value;
                GUILayout.Label(name, EditorStyles.label, GUILayout.Width(EditorStyle.FromToValueLabelWidth));
                using (GUILabelWidthArea.Create(EditorStyle.CharacterWidth))
                {
                    value.x = DrawFloatProperty(axis1Name, value.x, enableAxis1);
                    value.y = DrawFloatProperty(axis2Name, value.y, enableAxis2);
                }

                property.vector2Value = value;
            }
        }

        public static void DrawVector3Property(SerializedProperty holdProperty, SerializedProperty valueProperty, string name,
            string axis1Name, string axis2Name, string axis3Name,
            bool enableAxis1, bool enableAxis2, bool enableAxis3)
        {
            using (GUIHorizontal.Create())
            {
                DrawHoldProperty(holdProperty);
                var value = valueProperty.vector3Value;
                GUILayout.Label(name, EditorStyles.label, GUILayout.Width(EditorStyle.FromToValueLabelWidth));
                using (GUILabelWidthArea.Create(EditorStyle.CharacterWidth))
                {
                    value.x = DrawFloatProperty(axis1Name, value.x, enableAxis1);
                    value.y = DrawFloatProperty(axis2Name, value.y, enableAxis2);
                    value.z = DrawFloatProperty(axis3Name, value.z, enableAxis3);
                }

                valueProperty.vector3Value = value;
            }
        }

        public static void DrawVector4Property(SerializedProperty holdProperty, SerializedProperty property, string name,
            string axis1Name, string axis2Name, string axis3Name, string axis4Name,
            bool enableAxis1, bool enableAxis2, bool enableAxis3, bool enableAxis4)
        {
            using (GUIHorizontal.Create())
            {
                DrawHoldProperty(holdProperty);
                var value = property.vector4Value;
                GUILayout.Label(name, EditorStyles.label, GUILayout.Width(EditorStyle.FromToValueLabelWidth));
                using (GUILabelWidthArea.Create(EditorStyle.CharacterWidth))
                {
                    value.x = DrawFloatProperty(axis1Name, value.x, enableAxis1);
                    value.y = DrawFloatProperty(axis2Name, value.y, enableAxis2);
                    value.z = DrawFloatProperty(axis3Name, value.z, enableAxis3);
                    value.w = DrawFloatProperty(axis4Name, value.w, enableAxis4);
                }

                property.vector4Value = value;
            }
        }

        public static void DrawRectProperty(SerializedProperty holdProperty, SerializedProperty property, string name,
            string axis1Name, string axis2Name, string axis3Name, string axis4Name,
            bool enableAxis1, bool enableAxis2, bool enableAxis3, bool enableAxis4)
        {
            using (GUIHorizontal.Create())
            {
                DrawHoldProperty(holdProperty);
                var value = property.rectValue;
                GUILayout.Label(name, EditorStyles.label, GUILayout.Width(EditorStyle.FromToValueLabelWidth));
                using (GUILabelWidthArea.Create(EditorStyle.CharacterWidth))
                {
                    value.x = DrawFloatProperty(axis1Name, value.x, enableAxis1);
                    value.y = DrawFloatProperty(axis2Name, value.y, enableAxis2);
                    value.width = DrawFloatProperty(axis3Name, value.width, enableAxis3);
                    value.height = DrawFloatProperty(axis4Name, value.height, enableAxis4);
                }

                property.rectValue = value;
            }
        }

        public static void DrawRectOffsetProperty(SerializedProperty holdProperty, SerializedProperty property, string name,
            string axis1Name, string axis2Name, string axis3Name, string axis4Name,
            bool enableAxis1, bool enableAxis2, bool enableAxis3, bool enableAxis4)
        {
            using (GUIHorizontal.Create())
            {
                DrawHoldProperty(holdProperty);
                GUILayout.Label(name, EditorStyles.label, GUILayout.Width(EditorStyle.FromToValueLabelWidth));
                using (GUILabelWidthArea.Create(EditorStyle.CharacterWidth))
                {

                }
            }
        }

        public static void DrawQuaternionProperty(SerializedProperty holdProperty, SerializedProperty property, string name,
            string axis1Name, string axis2Name, string axis3Name, string axis4Name,
            bool enableAxis1, bool enableAxis2, bool enableAxis3, bool enableAxis4)
        {
            using (GUIHorizontal.Create())
            {
                DrawHoldProperty(holdProperty);
                var value = property.quaternionValue;
                GUILayout.Label(name, EditorStyles.label, GUILayout.Width(EditorStyle.FromToValueLabelWidth));
                using (GUILabelWidthArea.Create(EditorStyle.CharacterWidth))
                {
                    value.x = DrawFloatProperty(axis1Name, value.x, enableAxis1);
                    value.y = DrawFloatProperty(axis2Name, value.y, enableAxis2);
                    value.z = DrawFloatProperty(axis3Name, value.z, enableAxis3);
                    value.w = DrawFloatProperty(axis4Name, value.w, enableAxis4);
                }

                property.quaternionValue = value;
            }
        } 

        #endregion

        #region Draggable ProgressBar

        public static void DrawDraggableProgressBar(UnityEngine.Object target, float height, float currentValue, Action<float> onValueChanged = null)
        {
            var disableColor = UTweenEditorSetting.Ins.ProgressDisableColor;
            var backColor = UTweenEditorSetting.Ins.ProgressBackColor;
            var rangeColor = UTweenEditorSetting.Ins.ProgressColor;
            var rect = EditorGUILayout.GetControlRect(false, height);
            var valuePos = Mathf.Round(rect.width * currentValue);

            // Back
            EditorGUI.DrawRect(rect, backColor);

            // Progress
            var rectProgress = rect;
            rectProgress.width = valuePos;
            EditorGUI.DrawRect(rectProgress, rangeColor);

            if (onValueChanged != null)
            {
                var id = GUIUtility.GetControlID(FocusType.Passive);
                Draggable(target, id, ref _progressState1, rect, rect, result =>
                {
                    currentValue = result;
                    onValueChanged(currentValue);
                }, true);
            }
            else
            {
                EditorGUI.DrawRect(rect, disableColor);
            }

            if (!GUI.enabled)
            {
                EditorGUI.DrawRect(rect, disableColor);
            }
        }

        private static int _progressState1;
        private static int _progressState2;
        private static int _progressState3;

        public static void DrawDraggableProgressBar(UnityEngine.Object target, float height, float fromValue, float toValue, float currentValue, Action<float, float> onValueChanged = null)
        {
            var disableColor = UTweenEditorSetting.Ins.ProgressDisableColor;
            var backColor = UTweenEditorSetting.Ins.ProgressBackColor;
            var rangeColor = UTweenEditorSetting.Ins.SubProgressColor;
            var inRangeColor = UTweenEditorSetting.Ins.ProgressColor;
            var outOfRangeColor = UTweenEditorSetting.Ins.ProgressColor * 0.55f;

            var rect = EditorGUILayout.GetControlRect(false, height);
            var progressWidth = Mathf.Round(rect.width * (toValue - fromValue));
            var fromValuePos = Mathf.Round(rect.width * fromValue);
            var toValuePos = Mathf.Round(rect.width * toValue);
            var currentValuePos = Mathf.Round(rect.width * currentValue);

            // Handle
            var fromRect = new Rect(rect.x, rect.y, fromValuePos + 1f, rect.height);
            var toRect = new Rect(rect.x + toValuePos, rect.y, rect.x + rect.width - toValuePos - 1f, rect.height);
            var rangeRect = new Rect(rect.x + fromValuePos + 2f, rect.y, progressWidth - 4f, rect.height);

            // Back
            EditorGUI.DrawRect(rect, backColor);

            // Range
            var rectRange = rect;
            rectRange.x = rect.x + Mathf.Round(rect.width * fromValue);
            rectRange.width = progressWidth;
            EditorGUI.DrawRect(rectRange, rangeColor);

            // Progress
            if (currentValue > 0)
            {
                var delayRect = rect;
                delayRect.width = currentValuePos > fromValuePos ? fromValuePos : currentValuePos;
                EditorGUI.DrawRect(delayRect, outOfRangeColor);

                if (currentValue > fromValue)
                {
                    var playingRect = rect;
                    playingRect.x = rect.x + fromValuePos;
                    playingRect.width = (currentValuePos > toValuePos ? toValuePos : currentValuePos) - fromValuePos;
                    EditorGUI.DrawRect(playingRect, inRangeColor);
                }

                if (currentValue > toValue)
                {
                    var afterRect = rect;
                    afterRect.x = rect.x + toValuePos;
                    afterRect.width = currentValuePos - toValuePos;
                    EditorGUI.DrawRect(afterRect, outOfRangeColor);
                }
            }

            if (onValueChanged != null)
            {
                var id = GUIUtility.GetControlID(FocusType.Passive);
                Draggable(target, id, ref _progressState1, rect, fromRect, value =>
                {
                    fromValue = value;
                    if (fromValue > toValue)
                    {
                        toValue = fromValue;
                    }

                    onValueChanged(fromValue, toValue);
                }, true);

                Draggable(target, id, ref _progressState2, rect, toRect, value =>
                {
                    toValue = value;
                    if (toValue < fromValue)
                    {
                        fromValue = toValue;
                    }

                    onValueChanged(fromValue, toValue);
                }, true);

                // Draggable(target, id, ref _progressState3, rect, rangeRect, value =>
                // {
                //     var range = toValue - fromValue;
                //     if (value < 0) value = 0;
                //     if (value > 1f - range) value = 1f - range;
                //     fromValue = value;
                //     toValue = value + range;
                //
                //     onValueChanged(fromValue, toValue);
                // });
            }
            else
            {
                EditorGUI.DrawRect(rect, disableColor);
            }

            if (!GUI.enabled)
            {
                EditorGUI.DrawRect(rect, disableColor);
            }
        }

        public static void Draggable(UnityEngine.Object target, int controlId, ref int state, Rect progressRect, Rect handleRect, Action<float> onValueChanged, bool allowClick = false)
        {
            var current = Event.current;
            switch (current.GetTypeForControl(controlId))
            {
                case EventType.MouseDown:
                    if (handleRect.Contains(current.mousePosition) && current.button == 0)
                    {
                        EditorGUIUtility.editingTextField = false;
                        GUIUtility.hotControl = controlId;
                        Undo.RegisterCompleteObjectUndo(target, "Drag Progress");
                        state = 1;
                        if (allowClick)
                        {
                            var offset = current.mousePosition.x - progressRect.x + 1f;
                            var value = Mathf.Clamp01(offset / progressRect.width);
                            value = (float)Math.Round(value, 3);
                            onValueChanged(value);
                            GUI.changed = true;
                        }

                        current.Use();
                    }

                    break;

                case EventType.MouseUp:
                    if (GUIUtility.hotControl == controlId && state != 0)
                    {
                        GUIUtility.hotControl = 0;
                        state = 0;
                        current.Use();
                    }

                    break;

                case EventType.MouseDrag:
                    if (GUIUtility.hotControl != controlId)
                    {
                        break;
                    }

                    if (state != 0)
                    {
                        var offset = current.mousePosition.x - progressRect.x + 1f;
                        var value = Mathf.Clamp01(offset / progressRect.width);
                        value = (float)Math.Round(value, 3);
                        onValueChanged(value);
                        GUI.changed = true;
                        current.Use();
                    }

                    break;

                case EventType.Repaint:
                    EditorGUIUtility.AddCursorRect(handleRect, MouseCursor.SlideArrow);
                    break;
            }
        }

        #endregion

        #region Rect

        public static void DrawEmptyRect(Rect rect, Color color)
        {
            var left = rect;
            left.width = 1;
            var right = left;
            right.x = rect.x + rect.width;
            var top = rect;
            top.height = 1;
            var bottom = top;
            bottom.y = rect.y + rect.height;

            EditorGUI.DrawRect(left, color);
            EditorGUI.DrawRect(right, color);
            EditorGUI.DrawRect(top, color);
            EditorGUI.DrawRect(bottom, color);
        }

        #endregion
    }
}
#endif