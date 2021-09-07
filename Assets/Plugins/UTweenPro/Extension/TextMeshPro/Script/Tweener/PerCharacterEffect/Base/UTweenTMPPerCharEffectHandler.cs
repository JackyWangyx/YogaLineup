#if UTWEEN_TEXTMESHPRO
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Aya.TweenPro
{
    [RequireComponent(typeof(TMP_Text))]
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    [AddComponentMenu("UTween Pro/UTween TMP Per-Char Effect Handler")]
    public class UTweenTMPPerCharEffectHandler : MonoBehaviour
    {
        [NonSerialized]
        public List<ITMPCharacterModifier> Modifiers = new List<ITMPCharacterModifier>();

        public TMP_Text Text { get; set; }

        public bool ChangeGeometry { get; set; }
        public bool ChangeColor { get; set; }

        public void SyncModifiers(TweenData tween)
        {
            Modifiers.Clear();
            foreach (var tweener in tween.TweenerList)
            {
                if (!tweener.Active) continue;
                if (tweener is ITMPCharacterModifier modifier)
                {
                    Modifiers.Add(modifier);
                }
            }
        }

        public void Update()
        {
            if (Text == null)
            {
                Text = GetComponent<TMP_Text>();
            }

            if (Text == null) return;

            Text.ForceMeshUpdate(true);
            ChangeGeometry = false;
            ChangeColor = false;

            var meshCount = Text.textInfo.meshInfo.Length;
            for (var meshIndex = 0; meshIndex < meshCount; meshIndex++)
            {
                var vectorLength = Text.textInfo.meshInfo[meshIndex].vertices.Length;
                var characterIndex = 0;
                for (var i = 0; i < vectorLength; i += 4)
                {
                    var progress = i * 1f / vectorLength;
                    foreach (var modifier in Modifiers)
                    {
                        if (modifier.ChangeGeometry)
                        {
                            modifier.ModifyGeometry(characterIndex, ref Text.textInfo.meshInfo[meshIndex].vertices, i, progress);
                            ChangeGeometry = true;
                        }

                        if (modifier.ChangeColor)
                        {
                            modifier.ModifyColor(characterIndex, ref Text.textInfo.meshInfo[meshIndex].colors32, i, progress);
                            ChangeColor = true;
                        }
                    }

                    characterIndex++;
                }
            }

            if (ChangeGeometry)
            {
                for (var i = 0; i < Text.textInfo.meshInfo.Length; i++)
                {
                    Text.textInfo.meshInfo[i].mesh.vertices = Text.textInfo.meshInfo[i].vertices;
                    Text.UpdateGeometry(Text.textInfo.meshInfo[i].mesh, i);
                }

                ChangeGeometry = false;
            }

            if (ChangeColor)
            {
                Text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                ChangeColor = false;
            }
        }
    }
}

#endif