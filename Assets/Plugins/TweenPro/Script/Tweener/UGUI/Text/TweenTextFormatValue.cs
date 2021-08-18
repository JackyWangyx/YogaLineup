using System;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Text Format Value", "UGUI Text")]
    [Serializable]
    public partial class TweenTextFormatValue : TweenValueFloat<Text>
    {
        public string Text;
        public override bool SupportSetCurrentValue => false;

        public override float Value
        {
            get => _value;
            set
            {
                _value = value;
                var str = string.Format(Text, _value);
                Target.text = str;
            }
        }

        private float _value;

        public override void Reset()
        {
            base.Reset();
            Text = "{0:F2}";
        }
    }

#if UNITY_EDITOR

    public partial class TweenTextFormatValue : TweenValueFloat<Text>
    {
        [NonSerialized] public SerializedProperty TextProperty;

        public override void InitEditor(int index, TweenData data, SerializedProperty tweenerProperty)
        {
            base.InitEditor(index, data, tweenerProperty);
            TextProperty = TweenerProperty.FindPropertyRelative(nameof(Text));
        }

        public override void DrawFromToValue()
        {
            base.DrawFromToValue();
            EditorGUILayout.PropertyField(TextProperty);
        }
    }

#endif

    #region Extension

    public partial class TweenTextFormatValue : TweenValueFloat<Text>
    {
        public TweenTextFormatValue SetFormatText(string formatText)
        {
            Text = formatText;
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenTextFormatValue FormatText(Text text, string formatText, float to, float duration)
        {
            var tweener = Play<TweenTextFormatValue, Text, float>(text, to, duration)
                .SetFormatText(formatText);
            return tweener;
        }

        public static TweenTextFormatValue FormatText(Text text, string formatText, float from, float to, float duration)
        {
            var tweener = Play<TweenTextFormatValue, Text, float>(text, from, to, duration)
                .SetFormatText(formatText);
            return tweener;
        }
    }

    public static partial class TextExtension
    {
        public static TweenTextFormatValue TweenFormatText(this Text text, string formatText, float to, float duration)
        {
            var tweener = UTween.FormatText(text, formatText, to, duration);
            return tweener;
        }

        public static TweenTextFormatValue TweenFormatText(this Text text, string formatText, float from, float to, float duration)
        {
            var tweener = UTween.FormatText(text, formatText, from, to, duration);
            return tweener;
        }
    }

    #endregion
}
