#if UTWEEN_TEXTMESHPRO
using System;
using TMPro;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("TMP Text Per-Char Position", "TextMeshPro", UTweenTMP.IconPath)]
    [Serializable]
    public partial class TweenTMPPerCharPosition : TweenValueVector3<TMP_Text>, ITMPCharacterModifier
    {
        public TMPCharacterModifier Modifier;

        public bool ChangeGeometry => true;
        public bool ChangeColor => false;

        public override bool SupportIndependentAxis => false;
        public override bool SupportSetCurrentValue => false;
        public override Vector3 Value { get; set; }

        public override void PreSample()
        {
            base.PreSample();
            Modifier.Cache(Data, Target, this);
            Modifier.Effect.SyncModifiers(Data);
        }

        public override void Sample(float factor)
        {
        }

        public void ModifyGeometry(int characterIndex, ref Vector3[] vertices, int startIndex, float progress)
        {
            var from = FromGetter();
            var to = ToGetter();
            var factor = Modifier.GetFactor(progress, Factor);
            var position = Vector3.LerpUnclamped(from, to, factor);
            for (var i = startIndex; i < startIndex + 4; i++)
            {
                vertices[i] += position;
            }
        }

        public void ModifyColor(int characterIndex, ref Color32[] colors, int startIndex, float progress)
        {
        }

        public override void OnRemoved()
        {
            base.OnRemoved();
            Modifier.Remove(Data, Target, this);
        }

        public override void Reset()
        {
            base.Reset();
            Modifier.Reset();
        }
    }

#if UNITY_EDITOR

    public partial class TweenTMPPerCharPosition : TweenValueVector3<TMP_Text>, ITMPCharacterModifier
    {
        public override void InitEditor(int index, TweenData data, SerializedProperty tweenerProperty)
        {
            base.InitEditor(index, data, tweenerProperty);
            Modifier.InitEditor(this, tweenerProperty);
        }

        public override void DrawBody()
        {
            Modifier.DrawCharacterModifier();
        }
    }

#endif

}
#endif