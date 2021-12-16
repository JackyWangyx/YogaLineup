#if UTWEEN_TEXTMESHPRO
using System;
using TMPro;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("TMP Text Per-Char Color", "TextMeshPro", UTweenTMP.IconPath)]
    [Serializable]
    public partial class TweenTMPPerCharColor : TweenValueColor<TMP_Text>, ITMPCharacterModifier
    {
        public TMPCharacterModifier Modifier = new TMPCharacterModifier();
        public ColorOverlayMode Overlay;

        public bool ChangeGeometry => false;
        public bool ChangeColor => true;

        public override bool SupportIndependentAxis => false;
        public override bool SupportSetCurrentValue => false;
        public override Color Value { get; set; }

        public override void PreSample()
        {
            base.PreSample();
            Modifier.Cache(Data, Target, this);
        }

        public override void Sample(float factor)
        {
        }

        public void ModifyGeometry(int characterIndex, ref Vector3[] vertices, int startIndex, float progress)
        {
        }

        public void ModifyColor(int characterIndex, ref Color32[] colors, int startIndex, float progress)
        {
            var from = FromGetter();
            var to = ToGetter();
            var factor = Modifier.GetFactor(progress, Factor);
            var color = ColorMode == ColorMode.FromTo ? Color.Lerp(from, to, factor) : Gradient.Evaluate(factor);
            for (var i = startIndex; i < startIndex + 4; i++)
            {
                if (Overlay == ColorOverlayMode.Multiply)
                {
                    colors[i] *= color;
                }
                else if (Overlay == ColorOverlayMode.Add)
                {
                    colors[i] += color;
                }
                else if (Overlay == ColorOverlayMode.Minus)
                {
                    colors[i] -= color;
                }
            }
        }

        public override void OnRemoved()
        {
            base.OnRemoved();
            Modifier.Remove(Data, Target, this);
        }

        public override void Reset()
        {
            base.Reset();
            Overlay = ColorOverlayMode.Multiply;
            Modifier.Reset();
        }
    }

#if UNITY_EDITOR

    public partial class TweenTMPPerCharColor : TweenValueColor<TMP_Text>, ITMPCharacterModifier
    {
        [TweenerProperty, NonSerialized] public SerializedProperty OverlayProperty;

        public override void InitEditor(int index, TweenData data, SerializedProperty tweenerProperty)
        {
            base.InitEditor(index, data, tweenerProperty);
            Modifier.InitEditor(this, tweenerProperty);
        }

        public override void DrawBody()
        {
            GUIUtil.DrawToolbarEnum(OverlayProperty, nameof(Overlay), typeof(ColorOverlayMode));
            Modifier.DrawCharacterModifier();
        }
    }

#endif

}
#endif