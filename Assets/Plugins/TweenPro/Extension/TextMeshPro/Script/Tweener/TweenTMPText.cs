#if UTWEEN_TEXTMESHPRO
using System;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("TMP Text", "TextMeshPro", UTweenTMP.IconPath)]
    [Serializable]
    public partial class TweenTMPText : TweenValueString<TMP_Text>
    {
        public override string Value
        {
            get => Target.text;
            set => Target.text = value;
        }
    }

    #region Extension

    public static partial class UTweenTMP
    {
        public static TweenTMPText Text(TMP_Text text, string to, float duration, bool richText = true)
        {
            var tweener = UTween.Play<TweenTMPText, TMP_Text, string>(text, to, duration)
                .SetRichText(richText) as TweenTMPText;
            return tweener;
        }

        public static TweenTMPText Text(TMP_Text text, string from, string to, float duration, bool richText = true)
        {
            var tweener = UTween.Play<TweenTMPText, TMP_Text, string>(text, from, to, duration)
                .SetRichText(richText) as TweenTMPText;
            return tweener;
        }
    }

    public static partial class TMP_TextExtension
    {
        public static TweenTMPText TweenText(this TMP_Text text, string to, float duration, bool richText = true)
        {
            var tweener = UTweenTMP.Text(text, to, duration, richText);
            return tweener;
        }

        public static TweenTMPText TweenText(this TMP_Text text, string from, string to, float duration, bool richText = true)
        {
            var tweener = UTweenTMP.Text(text, from, to, duration, richText);
            return tweener;
        }
    }

    #endregion
}
#endif