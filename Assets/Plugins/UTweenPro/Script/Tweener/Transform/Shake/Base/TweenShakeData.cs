using System;
using System.Runtime.InteropServices;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    [StructLayout(LayoutKind.Auto)]
    public partial struct TweenShakeData
    {
        public ShakeMode Mode;
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;
        public int Count;
        public AnimationCurve Curve;

        public void Reset()
        {
            Mode = ShakeMode.Random;
            Position = Vector3.one;
            Rotation = Vector3.zero;
            Scale = Vector3.zero;
            Count = 5;
            var defaultSlope = 5f;
            Curve = new AnimationCurve(
                new Keyframe(0f, 0f, defaultSlope, defaultSlope), 
                new Keyframe(0.25f, 1f, 0f, 0f), 
                new Keyframe(0.5f, 0f, -defaultSlope, -defaultSlope),
                new Keyframe(0.75f, -1f, 0f, 0f),
                new Keyframe(1f, 0f, defaultSlope, defaultSlope));
        }
    }

#if UNITY_EDITOR

    public partial struct TweenShakeData
    {
        [NonSerialized] public TweenShake Tweener;
        [NonSerialized] public SerializedProperty TweenerProperty;

        [NonSerialized] public SerializedProperty ShakeDataProperty;
        [NonSerialized] public SerializedProperty ModeProperty;
        [NonSerialized] public SerializedProperty PositionProperty;
        [NonSerialized] public SerializedProperty RotationProperty;
        [NonSerialized] public SerializedProperty ScaleProperty;
        [NonSerialized] public SerializedProperty CountProperty;
        [NonSerialized] public SerializedProperty CurveProperty;
        [NonSerialized] public SerializedProperty SlopeProperty;

        public void InitEditor(TweenShake tweener, SerializedProperty tweenerProperty)
        {
            Tweener = tweener;
            TweenerProperty = tweenerProperty;
            ShakeDataProperty = TweenerProperty.FindPropertyRelative(nameof(Tweener.ShakeData));
            ModeProperty = ShakeDataProperty.FindPropertyRelative(nameof(Mode));
            PositionProperty = ShakeDataProperty.FindPropertyRelative(nameof(Position));
            RotationProperty = ShakeDataProperty.FindPropertyRelative(nameof(Rotation));
            ScaleProperty = ShakeDataProperty.FindPropertyRelative(nameof(Scale));
            CountProperty = ShakeDataProperty.FindPropertyRelative(nameof(Count));
            CurveProperty = ShakeDataProperty.FindPropertyRelative(nameof(Curve));
        }

        public void DrawShakeData()
        {
            Mode = (ShakeMode) EditorGUILayout.EnumPopup(nameof(Mode), Mode);
            var labelWidth = Tweener.EnableAxis ? EditorGUIUtility.labelWidth - EditorStyle.CharacterWidth : EditorGUIUtility.labelWidth;
            using (GUILabelWidthArea.Create(labelWidth))
            {
                using (GUIHorizontal.Create())
                {
                    if (Tweener.EnableAxis)
                    {
                        Tweener.AxisX = GUILayout.Toggle(Tweener.AxisX, "", GUILayout.Width(EditorStyle.CharacterWidth));
                    }
                    
                    using (GUIEnableArea.Create(Tweener.AxisX))
                    {
                        EditorGUILayout.PropertyField(PositionProperty, new GUIContent("Pos"));
                    }
                }

                using (GUIHorizontal.Create())
                {
                    if (Tweener.EnableAxis)
                    {
                        Tweener.AxisY = GUILayout.Toggle(Tweener.AxisY, "", GUILayout.Width(EditorStyle.CharacterWidth));
                    }

                    using (GUIEnableArea.Create(Tweener.AxisY))
                    {
                        EditorGUILayout.PropertyField(RotationProperty, new GUIContent("Rot"));
                    }
                }

                using (GUIHorizontal.Create())
                {
                    if (Tweener.EnableAxis)
                    {
                        Tweener.AxisZ = GUILayout.Toggle(Tweener.AxisZ, "", GUILayout.Width(EditorStyle.CharacterWidth));
                    }

                    using (GUIEnableArea.Create(Tweener.AxisZ))
                    {
                        EditorGUILayout.PropertyField(ScaleProperty, new GUIContent("Scale"));
                    }
                }
            }

            using (GUIHorizontal.Create())
            {
                EditorGUILayout.PropertyField(CountProperty);
                EditorGUILayout.PropertyField(CurveProperty, new GUIContent("Shake"));
            }
        }
    }

#endif
}