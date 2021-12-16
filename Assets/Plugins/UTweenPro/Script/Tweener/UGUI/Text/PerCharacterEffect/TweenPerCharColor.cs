using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Text Per-Char Color", "UGUI Text")]
    [Serializable]
    public partial class TweenPerCharColor : TweenValueColor<Text>, ITextCharacterModifier
    {
        public TextCharacterModifier Modifier = new TextCharacterModifier();
        public ColorOverlayMode Overlay;

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

        public void Modify(int characterIndex, ref UIVertex[] vertices, float progress)
        {
            var from = FromGetter();
            var to = ToGetter();
            var factor = Modifier.GetFactor(progress, Factor);
            var color = ColorMode == ColorMode.FromTo ? Color.Lerp(from, to, factor) : Gradient.Evaluate(factor);
            for (var i = 0; i < vertices.Length; i++)
            {
                if (Overlay == ColorOverlayMode.Multiply)
                {
                    vertices[i].color *= color;
                }
                else if (Overlay == ColorOverlayMode.Add)
                {
                    vertices[i].color += color;
                }
                else if (Overlay == ColorOverlayMode.Minus)
                {
                    vertices[i].color -= color;
                }
            }
        }

        public override void SetDirty()
        {
            base.SetDirty();
            Modifier.SetDirty();
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

    public partial class TweenPerCharColor : TweenValueColor<Text>, ITextCharacterModifier
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