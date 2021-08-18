using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    [StructLayout(LayoutKind.Auto)]
    public partial struct CharacterModifier
    {
        public float Range;
        public AnimationCurve Curve;

        [NonSerialized] public Text Text;
        [NonSerialized] public UTweenPerCharEffectHandler Effect;

        public void Cache(TweenData data, Text text, ICharacterModifier modifier)
        {
            if (Text == text) return;
            Text = text;
            if (Effect == null) Effect = text.GetComponent<UTweenPerCharEffectHandler>();
            if (Effect == null) Effect = text.gameObject.AddComponent<UTweenPerCharEffectHandler>();
            Effect.SyncModifiers(data);
        }

        public void Remove(TweenData data, Text text, ICharacterModifier modifier)
        {
            if (text == null) return;
            if (Effect == null) Effect = text.GetComponent<UTweenPerCharEffectHandler>();
            if (Effect == null) return;
            Effect.SyncModifiers(data);
            if (Effect.Modifiers.Count == 0)
            {
                if (Application.isPlaying)
                {
                    UnityEngine.Object.Destroy(Effect);
                }
                else
                {
                    UnityEngine.Object.DestroyImmediate(Effect);
                }
            }
        }

        public float GetFactor(float progress, float normalizedTime)
        {
            var from = progress * (1f - Range);
            var to = from + Range;
            if (normalizedTime <= from) return Curve.Evaluate(0f);
            if (normalizedTime >= to) return Curve.Evaluate(1f);
            var factor = (normalizedTime - from) / Range;
            factor = Curve.Evaluate(factor);
            return factor;
        }

        public void SetDirty()
        {
            Effect?.SetDirty();
        }

        public void Reset()
        {
            Range = 0.5f;
            Curve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
        }
    }

#if UNITY_EDITOR

    public partial struct CharacterModifier
    {
        [NonSerialized] public Tweener Tweener;
        [NonSerialized] public SerializedProperty TweenerProperty;

        [NonSerialized] public SerializedProperty ModifierProperty;
        [NonSerialized] public SerializedProperty RangeProperty;
        [NonSerialized] public SerializedProperty CurveProperty;

        public void InitEditor(Tweener tweener, SerializedProperty tweenerProperty)
        {
            Tweener = tweener;
            TweenerProperty = tweenerProperty;
            ModifierProperty = TweenerProperty.FindPropertyRelative("Modifier");
            RangeProperty = ModifierProperty.FindPropertyRelative(nameof(Range));
            CurveProperty = ModifierProperty.FindPropertyRelative(nameof(Curve));
        }

        public void DrawCharacterModifier()
        {
            using (GUIHorizontal.Create())
            {
                EditorGUILayout.PropertyField(RangeProperty);
                Range = Mathf.Clamp01(Range);
                EditorGUILayout.PropertyField(CurveProperty, new GUIContent(nameof(Effect)));
            }
        }
    }

#endif

}