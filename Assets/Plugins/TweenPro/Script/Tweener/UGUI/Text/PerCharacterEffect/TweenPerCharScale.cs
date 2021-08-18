﻿using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Text Per-Char Scale", "UGUI Text")]
    [Serializable]
    public partial class TweenPerCharScale : TweenValueVector3<Text>, ICharacterModifier
    {
        public CharacterModifier Modifier;
        public CharacterSpaceMode CharacterSpace;

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

        public void Modify(int characterIndex, ref UIVertex[] vertices, float progress)
        {
            var from = FromGetter();
            var to = ToGetter();
            var factor = Modifier.GetFactor(progress, Factor);
            var scale = Vector3.LerpUnclamped(from, to, factor);

            if (CharacterSpace == CharacterSpaceMode.Character)
            {
                var center = Vector3.zero;
                for (var i = 0; i < vertices.Length; i++)
                {
                    center += vertices[i].position;
                }

                center /= vertices.Length;
                for (var i = 0; i < vertices.Length; i++)
                {
                    vertices[i].position.x = Mathf.LerpUnclamped(center.x, vertices[i].position.x, scale.x);
                    vertices[i].position.y = Mathf.LerpUnclamped(center.y, vertices[i].position.y, scale.y);
                    vertices[i].position.z = Mathf.LerpUnclamped(center.z, vertices[i].position.z, scale.z);
                }
            }
            else if (CharacterSpace == CharacterSpaceMode.Text)
            {
                for (var i = 0; i < vertices.Length; i++)
                {
                    vertices[i].position.x *= scale.x;
                    vertices[i].position.y *= scale.y;
                    vertices[i].position.z *= scale.z;
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
            Modifier.Reset();
            CharacterSpace = CharacterSpaceMode.Character;
        }
    }

#if UNITY_EDITOR

    public partial class TweenPerCharScale : TweenValueVector3<Text>, ICharacterModifier
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