using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Path Spiral", "Path")]
    [Serializable]
    public partial class TweenSpiral : TweenPathBase
    {
        public TargetPositionData Start;
        public float Turn;
        public float Radius;
        public Vector3 Normal;

        public override Vector3 GetPositionByFactor(float factor)
        {
            var radius = Mathf.Lerp(0f, Radius, factor);
            var x = Mathf.Sin(factor * 2f * Mathf.PI * Turn) * radius;
            var y = Mathf.Cos(factor * 2f * Mathf.PI * Turn) * radius;
            var z = 0f;
            var point = new Vector3(x, y, z);
            var rotation = Quaternion.FromToRotation(Vector3.forward, Normal).eulerAngles;
            var position = point.Rotate(StartPosition, rotation) + Start.GetPosition();
            return position;
        }

        public override void Reset()
        {
            base.Reset();
            Start.Reset();
            Turn = 3;
            Radius = 1;
            Normal = Vector3.forward;
        }
    }

#if UNITY_EDITOR

    public partial class TweenSpiral : TweenPathBase
    {
        [NonSerialized] public SerializedProperty StartProperty;
        [NonSerialized] public SerializedProperty SpeedProperty;
        [NonSerialized] public SerializedProperty RadiusProperty;
        [NonSerialized] public SerializedProperty NormalProperty;

        public override void InitEditor(int index, TweenData data, SerializedProperty tweenerProperty)
        {
            base.InitEditor(index, data, tweenerProperty);
            Start.InitEditor(this, TweenerProperty, nameof(Start));

            StartProperty = TweenerProperty.FindPropertyRelative(nameof(Start));
            SpeedProperty = TweenerProperty.FindPropertyRelative(nameof(Turn));
            RadiusProperty = TweenerProperty.FindPropertyRelative(nameof(Radius));
            NormalProperty = TweenerProperty.FindPropertyRelative(nameof(Normal));
        }

        public override void DrawBody()
        {
            Start.DrawTargetPosition();

            using (GUIHorizontal.Create())
            {
                EditorGUILayout.PropertyField(SpeedProperty);
                EditorGUILayout.PropertyField(RadiusProperty);
            }

            using (GUIColorArea.Create(EditorStyle.ErrorColor, Normal == Vector3.zero))
            {
                EditorGUILayout.PropertyField(NormalProperty);
            }
        }
    }

#endif
}
