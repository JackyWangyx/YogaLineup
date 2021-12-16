using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class TextCharacterModifier
    {
        public TextRangeMode RangeMode;
        public float RangeValue;
        public AnimationCurve Curve;

        [NonSerialized] public int TextLength;
        [NonSerialized] public float RuntimeRangeValue;
        [NonSerialized] public Text Text;
        [NonSerialized] public UTweenPerCharEffectHandler Effect;

        public void Cache(TweenData data, Text text, ITextCharacterModifier modifier)
        {
            if (Text == text) return;
            Text = text;
            CacheRuntimeRangeValue();
            if (Effect == null) Effect = text.GetComponent<UTweenPerCharEffectHandler>();
            if (Effect == null) Effect = text.gameObject.AddComponent<UTweenPerCharEffectHandler>();
            Effect.SyncModifiers(data);
        }

        public void CacheRuntimeRangeValue()
        {
            if (Text == null) return;
            if (RangeMode == TextRangeMode.Length)
            {
                TextLength = StringLerpUtil.GetLength(Text.text, Text.supportRichText);
                RuntimeRangeValue = RangeValue / TextLength;
            }

            if (RangeMode == TextRangeMode.Percent)
            {
                RuntimeRangeValue = RangeValue;
            }
        }

        public void Remove(TweenData data, Text text, ITextCharacterModifier modifier)
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
            var from = progress * (1f - RuntimeRangeValue);
            var to = from + RuntimeRangeValue;
            if (normalizedTime <= from) return Curve.Evaluate(0f);
            if (normalizedTime >= to) return Curve.Evaluate(1f);
            var factor = (normalizedTime - from) / RuntimeRangeValue;
            factor = Curve.Evaluate(factor);
            return factor;
        }

        public void SetDirty()
        {
            Effect?.SetDirty();
        }

        public void Reset()
        {
            RangeMode = TextRangeMode.Length;
            RangeValue = 1;
            Curve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
        }
    }

#if UNITY_EDITOR

    public partial class TextCharacterModifier
    {
        [NonSerialized] public Tweener Tweener;
        [NonSerialized] public SerializedProperty TweenerProperty;
        [NonSerialized] public SerializedProperty ModifierProperty;

        [TweenerProperty, NonSerialized] public SerializedProperty RangeModeProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty RangeValueProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty CurveProperty;

        public void InitEditor(Tweener tweener, SerializedProperty tweenerProperty)
        {
            Tweener = tweener;
            TweenerProperty = tweenerProperty;
            ModifierProperty = TweenerProperty.FindPropertyRelative("Modifier");

            TweenerPropertyAttribute.CacheProperty(this, ModifierProperty);
        }

        public void DrawCharacterModifier()
        {
            using (GUIHorizontal.Create())
            {
                using (var check = GUICheckChangeArea.Create())
                {
                    EditorGUILayout.PropertyField(RangeModeProperty, new GUIContent("Range"));

                    if (RangeMode == TextRangeMode.Length)
                    {
                        RangeValueProperty.floatValue = EditorGUILayout.FloatField(nameof(TextRangeMode.Length), RangeValueProperty.floatValue);
                        RangeValueProperty.floatValue = (int)RangeValueProperty.floatValue;
                        if (RangeValueProperty.floatValue < 1f) RangeValueProperty.floatValue = 1f;
                    }
                    else if (RangeMode == TextRangeMode.Percent)
                    {
                        RangeValueProperty.floatValue = EditorGUILayout.FloatField(nameof(TextRangeMode.Percent), RangeValueProperty.floatValue);
                        RangeValueProperty.floatValue = Mathf.Clamp(RangeValueProperty.floatValue, 0.01f, 1f);
                    }

                    if (check.Changed)
                    {
                        CacheRuntimeRangeValue();
                    }
                }
            }

            using (GUIHorizontal.Create())
            {
                EditorGUILayout.PropertyField(CurveProperty, new GUIContent(nameof(Effect)));
            }
        }
    }

#endif

}