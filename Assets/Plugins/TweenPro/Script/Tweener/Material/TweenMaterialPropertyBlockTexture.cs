﻿using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Material Texture", "Material", "Material Icon")]
    [Serializable]
    public partial class TweenMaterialPropertyBlockTexture : TweenValueVector4<Renderer>
    {
        public TweenMaterialData MaterialData;

        public override Vector4 Value
        {
            get
            {
                MaterialData.Cache(Target);
                return MaterialData.GetVector();
            }
            set
            {
                MaterialData.Cache(Target);
                MaterialData.SetVector(value);
            }
        }

        public override void Reset()
        {
            base.Reset();
            MaterialData.Reset();
            From = new Vector4(1f, 1f, 0f, 0f);
            To = new Vector4(1f, 1f, 1f, 1f);
#if UNITY_EDITOR
            EnableIndependentAxis = true;
#endif
        }
    }

#if UNITY_EDITOR

    public partial class TweenMaterialPropertyBlockTexture : TweenValueVector4<Renderer>
    {
        public override string AxisXName => "TX";
        public override string AxisYName => "TY";
        public override string AxisZName => "OX";
        public override string AxisWName => "OY";

        public override void InitEditor(int index, TweenData data, SerializedProperty tweenerProperty)
        {
            base.InitEditor(index, data, tweenerProperty);
            MaterialData.InitEditor(this, tweenerProperty);
        }

        public override void DrawIndependentAxis()
        {
            EditorStyle.CharacterWidth *= 1.5f;
            base.DrawIndependentAxis();
            EditorStyle.CharacterWidth /= 1.5f;
        }

        public override void DrawFromToValue()
        {
            EditorStyle.CharacterWidth *= 1.5f;
            base.DrawFromToValue();
            EditorStyle.CharacterWidth /= 1.5f;
        }

        public override void DrawTarget()
        {
            base.DrawTarget();
            MaterialData.DrawMaterialProperty(ShaderUtil.ShaderPropertyType.TexEnv);
        }
    }

#endif

    #region Extension

    public partial class TweenMaterialPropertyBlockTexture : TweenValueVector4<Renderer>
    {
        public TweenMaterialPropertyBlockTexture SetMaterialMode(TweenMaterialMode materialMode)
        {
            MaterialData.Mode = materialMode;
            return this;
        }

        public TweenMaterialPropertyBlockTexture SetMaterialIndex(int materialIndex)
        {
            MaterialData.Index = materialIndex;
            return this;
        }

        public TweenMaterialPropertyBlockTexture SetPropertyName(string propertyName)
        {
            MaterialData.Property = propertyName;
            return this;
        }
    }

    #endregion
}