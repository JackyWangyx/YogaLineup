using System;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("TMP Text Format Value", "TextMeshPro", UTweenTMP.IconPath)]
    [Serializable]
    public partial class TweenTMPTextFormatValue : TweenValueFloat<TMP_Text>
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

    public partial class TweenTMPTextFormatValue : TweenValueFloat<TMP_Text>
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

    public partial class TweenTMPTextFormatValue : TweenValueFloat<TMP_Text>
    {
        public TweenTMPTextFormatValue SetFormatText(string formatText)
        {
            Text = formatText;
            return this;
        }
    }

    public static partial class UTweenTMP
    {
        public static TweenTMPTextFormatValue FormatText(TMP_Text text, string formatText, float to, float duration)
        {
            var tweener = UTween.Play<TweenTMPTextFormatValue, TMP_Text, float>(text, to, duration)
                .SetFormatText(formatText);
            return tweener;
        }

        public static TweenTMPTextFormatValue FormatText(TMP_Text text, string formatText, float from, float to, float duration)
        {
            var tweener = UTween.Play<TweenTMPTextFormatValue, TMP_Text, float>(text, from, to, duration)
                .SetFormatText(formatText);
            return tweener;
        }
    }

    public static partial class TMP_TextExtension
    {
        public static TweenTMPTextFormatValue TweenFormatText(this TMP_Text text, string formatText, float to, float duration)
        {
            var tweener = UTweenTMP.FormatText(text, formatText, to, duration);
            return tweener;
        }

        public static TweenTMPTextFormatValue TweenFormatText(this TMP_Text text, string formatText, float from, float to, float duration)
        {
            var tweener = UTweenTMP.FormatText(text, formatText, from, to, duration);
            return tweener;
        }
    }

    #endregion
}
