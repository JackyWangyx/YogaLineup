#if UTWEEN_TEXTMESHPRO
using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("TMP Text Per-Char Glitch", "TextMeshPro", UTweenTMP.IconPath)]
    [Serializable]
    public partial class TweenTMPPerCharGlitch : TweenValueVector3<TMP_Text>, ITMPCharacterModifier
    {
        public TMPCharacterModifier Modifier = new TMPCharacterModifier();

        public bool ChangeGeometry => true;
        public bool ChangeColor => false;

        public override bool SupportIndependentAxis => false;
        public override bool SupportSetCurrentValue => false;
        public override Vector3 Value { get; set; }

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
            var from = FromGetter();
            var to = ToGetter();
            var factor = Modifier.GetFactor(progress, Factor);
            var power = Vector3.LerpUnclamped(from, to, factor);
            var glitchX = Random.Range(-power.x, power.x);
            var glitchY = Random.Range(-power.y, power.y);
            var glitchZ = Random.Range(-power.z, power.z);
            var glitch = new Vector3(glitchX, glitchY, glitchZ);
            for (var i = startIndex; i < startIndex + 4; i++)
            {
                vertices[i] += glitch;
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

    public partial class TweenTMPPerCharGlitch : TweenValueVector3<TMP_Text>, ITMPCharacterModifier
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