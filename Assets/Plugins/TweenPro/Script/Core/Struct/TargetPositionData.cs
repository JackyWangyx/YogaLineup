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
    public partial struct TargetPositionData
    {
        public TargetPositionMode Mode;
        public Transform Transform;
        public Vector3 Position;

        public Vector3 GetPosition()
        {
            if (Mode == TargetPositionMode.Transform)
            {
                if (Transform == null) return default;
                return Transform.position;
            }

            if (Mode == TargetPositionMode.Position) return Position;
            return default;
        }

        public static implicit operator TargetPositionData(Vector3 value)
        {
            var positionData = new TargetPositionData {Position = value};
            return positionData;
        }

        public static implicit operator Vector3(TargetPositionData data)
        {
            return data.GetPosition();
        }

        public void Reset()
        {
            Mode = TargetPositionMode.Transform;
            Transform = null;
            Position = Vector3.zero;
        }
    }

#if UNITY_EDITOR

    public partial struct TargetPositionData
    {
        [NonSerialized] public Tweener Tweener;
        [NonSerialized] public SerializedProperty TweenerProperty;
        [NonSerialized] public string PropertyName;

        [NonSerialized] public SerializedProperty TargetPositionDataProperty;
        [NonSerialized] public SerializedProperty ModeProperty;
        [NonSerialized] public SerializedProperty TransformProperty;
        [NonSerialized] public SerializedProperty PositionProperty;

        public void InitEditor(Tweener tweener, SerializedProperty tweenerProperty, string propertyName)
        {
            Tweener = tweener;
            TweenerProperty = tweenerProperty;
            PropertyName = propertyName;
            TargetPositionDataProperty = TweenerProperty.FindPropertyRelative(PropertyName);
            ModeProperty = TargetPositionDataProperty.FindPropertyRelative(nameof(Mode));
            TransformProperty = TargetPositionDataProperty.FindPropertyRelative(nameof(Transform));
            PositionProperty = TargetPositionDataProperty.FindPropertyRelative(nameof(Position));
        }

        public void DrawTargetPosition()
        {
            using (GUIHorizontal.Create())
            {
                if (Mode == TargetPositionMode.Transform)
                {
                    using (GUIColorArea.Create(EditorStyle.ErrorColor, Transform == null))
                    {
                        EditorGUILayout.PropertyField(TransformProperty, new GUIContent(PropertyName));
                    }
                }

                if (Mode == TargetPositionMode.Position)
                {
                    PositionProperty.vector3Value = EditorGUILayout.Vector3Field(PropertyName, PositionProperty.vector3Value);
                }

                var btnSwitchMode = GUILayout.Button(Mode.ToString(), GUILayout.Width(EditorStyle.SettingButtonWidth));
                if (btnSwitchMode)
                {
                    ModeProperty.intValue = (int) (Mode == TargetPositionMode.Transform ? TargetPositionMode.Position : TargetPositionMode.Transform);
                }
            }
        }
    }

#endif
}
