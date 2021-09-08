﻿using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Text Per-Char Position", "UGUI Text")]
    [Serializable]
    public partial class TweenPerCharPosition : TweenValueVector3<Text>, ICharacterModifier
    {
        public CharacterModifier Modifier;

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
            var position = Vector3.LerpUnclamped(from, to, factor);
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i].position += position;
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
        }
    }

#if UNITY_EDITOR

    public partial class TweenPerCharPosition : TweenValueVector3<Text>, ICharacterModifier
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