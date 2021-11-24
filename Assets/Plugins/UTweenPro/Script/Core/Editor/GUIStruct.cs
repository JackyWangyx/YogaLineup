#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Aya.TweenPro
{
    public struct GUICheckChangeArea : IDisposable
    {
        private bool _end;
        private bool _changed;
        private Object _target;

        public bool Changed
        {
            get
            {
                if (_end) return _changed;
                _end = true;
                _changed = EditorGUI.EndChangeCheck();
                if (_changed && _target)
                {
                    Undo.RecordObject(_target, _target.name);
                }

                return _changed;
            }
        }

        public static GUICheckChangeArea Create(Object target = null)
        {
            EditorGUI.BeginChangeCheck();
            return new GUICheckChangeArea
            {
                _end = false,
                _changed = false,
                _target = target
            };
        }

        void IDisposable.Dispose()
        {
            if (_end) return;
            _end = true;
            _changed = EditorGUI.EndChangeCheck();
        }
    }

    public struct GUILabelWidthArea : IDisposable
    {
        public float OriginalLabelWidth;

        public static GUILabelWidthArea Create(float labelWidth)
        {
            return new GUILabelWidthArea(labelWidth);
        }

        private GUILabelWidthArea(float labelWidth)
        {
            OriginalLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = labelWidth;
        }

        public void Dispose()
        {
            EditorGUIUtility.labelWidth = OriginalLabelWidth;
        }
    }

    public struct GUIVertical : IDisposable
    {
        public static GUIVertical Create(params GUILayoutOption[] options)
        {
            return new GUIVertical(options);
        }

        public static GUIVertical Create(GUIStyle style, params GUILayoutOption[] options)
        {
            return new GUIVertical(style, options);
        }

        private GUIVertical(params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(options);
        }

        private GUIVertical(GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(style, options);
        }

        public void Dispose()
        {
            GUILayout.EndVertical();
        }
    }

    public struct GUIHorizontal : IDisposable
    {
        public static GUIHorizontal Create(params GUILayoutOption[] options)
        {
            return new GUIHorizontal(options);
        }

        public static GUIHorizontal Create(GUIStyle style, params GUILayoutOption[] options)
        {
            return new GUIHorizontal(style, options);
        }

        private GUIHorizontal(params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(options);
        }

        private GUIHorizontal(GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(style, options);
        }

        public void Dispose()
        {
            GUILayout.EndHorizontal();
        }
    }

    public struct GUIColorArea : IDisposable
    {
        public Color OriginalColor;

        public static GUIColorArea Create(Color color, bool check = true)
        {
            return new GUIColorArea(color, Color.white, check);
        }

        public static GUIColorArea Create(Color enableColor, Color disableColor, bool check = true)
        {
            return new GUIColorArea(enableColor, disableColor, check);
        }

        public GUIColorArea(Color enableColor, Color disableColor, bool check = true)
        {
            OriginalColor = GUI.color;
            if (check)
            {
                GUI.color = enableColor;
            }
            else
            {
                GUI.color = disableColor;
            }
        }

        public void Dispose()
        {
            GUI.color = OriginalColor;
        }
    }

    public struct GUIGroup : IDisposable
    {
        public static GUIGroup Create(params GUILayoutOption[] options)
        {
            return new GUIGroup(options);
        }

        private GUIGroup(params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        }

        public void Dispose()
        {
            EditorGUILayout.EndVertical();
        }
    }

    public struct GUIWideMode : IDisposable
    {
        public bool OriginalMode;

        public static GUIWideMode Create(bool enable)
        {
            return new GUIWideMode(enable);
        }

        private GUIWideMode(bool enable)
        {
            OriginalMode = EditorGUIUtility.wideMode;
            EditorGUIUtility.wideMode = enable;
        }

        public void Dispose()
        {
            EditorGUIUtility.wideMode = OriginalMode;
        }
    }

    public struct GUIEnableArea : IDisposable
    {
        public bool OriginalEnable;

        public static GUIEnableArea Create(bool enable)
        {
            return new GUIEnableArea(enable);
        }

        private GUIEnableArea(bool enable)
        {
            OriginalEnable = GUI.enabled;
            GUI.enabled = enable;
        }

        public void Dispose()
        {
            GUI.enabled = OriginalEnable;
        }
    }

    public struct GUIFoldOut : IDisposable
    {
        public static GUIFoldOut Create(Object target, string title, ref bool defaultState, params GUILayoutOption[] options)
        {
            return new GUIFoldOut(target, title, ref defaultState, options);
        }

        private GUIFoldOut(Object target, string title, ref bool defaultState, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, options);
            var rect = EditorGUILayout.GetControlRect();
            defaultState = GUI.Toggle(rect, defaultState, GUIContent.none, EditorStyles.foldout);
            rect.xMin += rect.height;
            EditorGUI.LabelField(rect, title, EditorStyles.boldLabel);
        }

        public void Dispose()
        {
            EditorGUILayout.EndVertical();
        }
    }
}
#endif