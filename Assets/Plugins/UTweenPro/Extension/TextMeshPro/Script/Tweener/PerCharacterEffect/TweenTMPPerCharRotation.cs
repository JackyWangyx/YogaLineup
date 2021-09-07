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
        public TMPCharacterModifier Modifier;
        public CharacterSpaceMode CharacterSpace;

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
            CharacterSpace = CharacterSpaceMode.Character;
        }
    }

#if UNITY_EDITOR

    public partial class TweenPerCharRotation : TweenValueVector3<TMP_Text>, ITMPCharacterModifier
    {
        [NonSerialized] public SerializedProperty CharacterSpaceProperty;

        public override void InitEditor(int index, TweenData data, SerializedProperty tweenerProperty)
        {
            base.InitEditor(index, data, tweenerProperty);
            Modifier.InitEditor(this, tweenerProperty);
            CharacterSpaceProperty = TweenerProperty.FindPropertyRelative(nameof(CharacterSpace));
        }

        public override void DrawBody()
        {
            Modifier.DrawCharacterModifier();
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