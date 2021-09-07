#if UTWEEN_TEXTMESHPRO
using System;
using TMPro;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("TMP Text Length ", "TextMeshPro", UTweenTMP.IconPath)]
    [Serializable]
    public partial class TweenTMPTextLength : TweenValueFloat<TMP_Text>
    {
        public bool RichText;

        public override float Value
        {
            get => _value;
            set
            {
                _value = Mathf.Clamp01(value);
                Target.text = StringLerpUtil.Lerp(RecordText, _value, RichText);
            }
        }

        private float _value;

        protected string RecordText;

        public override void RecordObject()
        {
            if (Target != null) RecordText = Target.text;
        }

        public override void RestoreObject()
        {
            if (Target != null) Target.text = RecordText;
        }

        public override void Reset()
        {
            base.Reset();
            From = 0f;
            To = 1f;
            RichText = true;
        }
    }

#if UNITY_EDITOR

    public partial class TweenTMPTextLength : TweenValueFloat<TMP_Text>
    {
        [NonSerialized] public SerializedProperty RichTextProperty;

        public override void InitEditor(int index, TweenData data, SerializedProperty tweenerProperty)
        {
            base.InitEditor(index, data, tweenerProperty);
            RichTextProperty = TweenerProperty.FindPropertyRelative(nameof(RichText));
        }

        public override void DrawFromToValue()
        {
            base.DrawFromToValue();
            From = Mathf.Clamp01(From);
            To = Mathf.Clamp01(To);
        }

        public override void DrawAppend()
        {
            base.DrawAppend();
            GUIUtil.ToggleOnOffButton(RichTextProperty);
        }
    }

#endif

    #region Extension

    public partial class TweenTMPTextLength : TweenValueFloat<TMP_Text>
    {
        public TweenTMPTextLength SetRichText(bool richText)
        {
            RichText = richText;
            return this;
        }
    }

    public static partial class UTweenTMP
    {
        public static TweenTMPTextLength TextLength(TMP_Text text, float to, float duration, bool richText = true)
        {
            var tweener = UTween.Play<TweenTMPTextLength, TMP_Text, float>(text, to, duration)
                .SetRichText(richText);
            return tweener;
        }

        public static TweenTMPTextLength TextLength(TMP_Text text, float from, float to, float duration, bool richText = true)
        {
            var tweener = UTween.Play<TweenTMPTextLength, TMP_Text, float>(text, from, to, duration)
                .SetRichText(richText);
            return tweener;
        }
    }

    public static partial class TMP_TextExtension
    {
        public static TweenTMPTextLength TweenTextLength(this TMP_Text text, float to, float duration, bool richText = true)
        {
            var tweener = UTweenTMP.TextLength(text, to, duration, richText);
            return tweener;
        }

        public static TweenTMPTextLength TweenTextLength(this TMP_Text text, float from, float to, float duration, bool richText = true)
        {
            var tweener = UTweenTMP.TextLength(text, from, to, duration, richText);
            return tweener;
        }
    }

    #endregion
}
#endif