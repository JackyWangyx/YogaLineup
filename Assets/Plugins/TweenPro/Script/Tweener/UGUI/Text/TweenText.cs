using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Text", "UGUI Text")]
    [Serializable]
    public partial class TweenText : TweenValueString<Text>
    {
        public override string Value
        {
            get => Target.text;
            set => Target.text = value;
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenText Text(Text text, string to, float duration, bool richText = true)
        {
            var tweener = Play<TweenText, Text, string>(text, to, duration)
                .SetRichText(richText) as TweenText;
            return tweener;
        }

        public static TweenText Text(Text text, string from, string to, float duration, bool richText = true)
        {
            var tweener = Play<TweenText, Text, string>(text, from, to, duration)
                .SetRichText(richText) as TweenText;
            return tweener;
        }
    }

    public static partial class TextExtension
    {
        public static TweenText TweenText(this Text text, string to, float duration, bool richText = true)
        {
            var tweener = UTween.Text(text, to, duration, richText);
            return tweener;
        }

        public static TweenText TweenText(this Text text, string from, string to, float duration, bool richText = true)
        {
            var tweener = UTween.Text(text, from, to, duration, richText);
            return tweener;
        }
    }

    #endregion
}