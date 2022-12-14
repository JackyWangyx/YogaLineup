#if UTWEEN_TEXTMESHPRO
using System;
using TMPro;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("TMP Text Per-Char Rotation", "TextMeshPro", UTweenTMP.IconPath)]
    [Serializable]
    public partial class TweenPerCharRotation : TweenValueVector3<TMP_Text>, ITMPCharacterModifier
    {
        public TMPPerCharEffectData EffectData = new TMPPerCharEffectData();
        public CharacterSpaceMode CharacterSpace;

        public TMP_Text GetTarget => Target;
        public bool ChangeGeometry => true;
        public bool ChangeColor => false;

        public override bool SupportIndependentAxis => false;
        public override bool SupportSetCurrentValue => false;
        public override Vector3 Value { get; set; }

        public override void PreSample()
        {
            base.PreSample();
            EffectData.Cache(((Tweener)this).Data, Target, this);
        }

        public override void Sample(float factor)
        {
        }

        public void ModifyGeometry(int characterIndex, ref Vector3[] vertices, float progress)
        {
            var startIndex = EffectData.GetStartIndex(characterIndex) * 4;
            var from = FromGetter();
            var to = ToGetter();
            var factor = EffectData.GetFactor(progress, Factor);
            var rotation = Vector3.LerpUnclamped(from, to, factor);

            if (CharacterSpace == CharacterSpaceMode.Character)
            {
                var center = Vector3.zero;
                for (var i = startIndex; i < startIndex + 4; i++)
                {
                    center += vertices[i];
                }

                center /= 4;
                for (var i = startIndex; i < startIndex + 4; i++)
                {
                    vertices[i] = vertices[i].Rotate(center, rotation);
                }
            }
            else if (CharacterSpace == CharacterSpaceMode.Text)
            {
                for (var i = startIndex; i < startIndex + 4; i++)
                {
                    vertices[i] = vertices[i].Rotate(Vector3.zero, rotation);
                }
            }
        }

        public void ModifyColor(int characterIndex, ref Color32[] colors, float progress)
        {
        }

        public override void OnRemoved()
        {
            base.OnRemoved();
            EffectData.Remove(((Tweener)this).Data, Target, this);
        }

        public override void Reset()
        {
            base.Reset();
            EffectData.Reset();
            CharacterSpace = CharacterSpaceMode.Character;
        }
    }

#if UNITY_EDITOR

    public partial class TweenPerCharRotation : TweenValueVector3<TMP_Text>, ITMPCharacterModifier
    {
        [TweenerProperty, NonSerialized] public SerializedProperty CharacterSpaceProperty;

        public override void InitEditor(int index, TweenData data, SerializedProperty tweenerProperty)
        {
            base.InitEditor(index, data, tweenerProperty);
            EffectData.InitEditor(this, tweenerProperty);
        }

        public override void DrawBody()
        {
            EffectData.DrawCharacterModifier();
        }

        public override void DrawAppend()
        {
            base.DrawAppend();
            GUIUtil.DrawToolbarEnum(CharacterSpaceProperty, nameof(Space), typeof(CharacterSpaceMode));
        }
    }

#endif

}
#endif