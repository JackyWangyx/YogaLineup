using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Animator Normalized Time", "Rendering")]
    [Serializable]
    public partial class TweenAnimatorNormalizedTime : TweenValueFloat<Animator>
    {
        public string State;
        public int Layer;
        public float Fade;

        public override float Value
        {
            get
            {
                var stateInfo = Target.GetCurrentAnimatorStateInfo(Layer);
                return stateInfo.normalizedTime;
            }
            set
            {
                if (Application.isPlaying)
                {
                    Target.CrossFade(State, Fade, Layer, value);
                }
                else
                {
                    Target.Play(State, Layer, value);
                    Target.Update(0);
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            State = null;
            Layer = 0;
            Fade = 0f;
        }
    }

#if UNITY_EDITOR

    public partial class TweenAnimatorNormalizedTime : TweenValueFloat<Animator>
    {
        [NonSerialized] public SerializedProperty StateProperty;
        [NonSerialized] public SerializedProperty LayerProperty;
        [NonSerialized] public SerializedProperty FadeProperty;

        public override void InitEditor(int index, TweenData data, SerializedProperty tweenerProperty)
        {
            base.InitEditor(index, data, tweenerProperty);

            StateProperty = TweenerProperty.FindPropertyRelative(nameof(State));
            LayerProperty = TweenerProperty.FindPropertyRelative(nameof(Layer));
            FadeProperty = TweenerProperty.FindPropertyRelative(nameof(Fade));
        }

        public override void DrawTarget()
        {
            base.DrawTarget();

            if (Target == null)
            {
                StateProperty.stringValue = null;
                return;
            }

            // State
            using (GUIColorArea.Create(EditorStyle.ErrorColor, string.IsNullOrEmpty(State)))
            {
                using (GUIHorizontal.Create())
                {
                    GUILayout.Label(nameof(State), GUILayout.Width(EditorStyle.LabelWidth));
                    var showName = string.IsNullOrEmpty(State) ? EditorStyle.NoneStr : State;
                    var btnClip = GUILayout.Button(showName, EditorStyles.popup);
                    if (btnClip)
                    {
                        var menu = new GenericMenu();
                        menu.AddItem(EditorStyle.NoneStr, string.IsNullOrEmpty(State), () =>
                        {
                            StateProperty.stringValue = null;
                            StateProperty.serializedObject.ApplyModifiedProperties();
                        });
                        menu.AddSeparator("");

                        var clips = Target.runtimeAnimatorController.animationClips;
                        foreach (var clip in clips)
                        {
                            var clipName = clip.name;
                            menu.AddItem(clipName, State == clipName, () =>
                            {
                                StateProperty.stringValue = clipName;
                                StateProperty.serializedObject.ApplyModifiedProperties();
                            });
                        }

                        menu.ShowAsContext();
                    }
                }
            }

            using (GUIHorizontal.Create())
            {
                EditorGUILayout.PropertyField(LayerProperty);
                EditorGUILayout.PropertyField(FadeProperty);
            }
        }
    }

#endif

    #region Extension

    public partial class TweenAnimatorNormalizedTime : TweenValueFloat<Animator>
    {
        public TweenAnimatorNormalizedTime SetState(string stateMName)
        {
            State = stateMName;
            return this;
        }

        public TweenAnimatorNormalizedTime SetLayerIndex(int layerIndex)
        {
            Layer = layerIndex;
            return this;
        }

        public TweenAnimatorNormalizedTime SetFadeTime(float fadeTime)
        {
            Fade = Mathf.Clamp01(fadeTime);
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenAnimatorNormalizedTime NormalizedTime(Animator animator, string clipName, float to, float duration)
        {
            var tweener = Play<TweenAnimatorNormalizedTime, Animator, float>(animator, to, duration)
                .SetLayerIndex(0)
                .SetState(clipName)
                .SetFadeTime(0f);
            return tweener;
        }

        public static TweenAnimatorNormalizedTime NormalizedTime(Animator animator, string clipName, float from, float to, float duration)
        {
            var tweener = Play<TweenAnimatorNormalizedTime, Animator, float>(animator, from, to, duration)
                .SetLayerIndex(0)
                .SetState(clipName)
                .SetFadeTime(0f);
            return tweener;
        }

        public static TweenAnimatorNormalizedTime NormalizedTime(Animator animator, int layerIndex, string clipName, float to, float duration)
        {
            var tweener = Play<TweenAnimatorNormalizedTime, Animator, float>(animator, to, duration)
                .SetLayerIndex(layerIndex)
                .SetState(clipName)
                .SetFadeTime(0f);
            return tweener;
        }

        public static TweenAnimatorNormalizedTime NormalizedTime(Animator animator, int layerIndex, string clipName, float from, float to, float duration)
        {
            var tweener = Play<TweenAnimatorNormalizedTime, Animator, float>(animator, from, to, duration)
                .SetLayerIndex(layerIndex)
                .SetState(clipName)
                .SetFadeTime(0f);
            return tweener;
        }
    }

    public static partial class AnimatorExtension
    {
        public static TweenAnimatorNormalizedTime TweenNormalizedTime(this Animator animator, string clipName, float to, float duration)
        {
            var tweener = UTween.NormalizedTime(animator, clipName, to, duration);
            return tweener;
        }

        public static TweenAnimatorNormalizedTime TweenNormalizedTime(this Animator animator, string clipName, float from, float to, float duration)
        {
            var tweener = UTween.NormalizedTime(animator, clipName, from, to, duration);
            return tweener;
        }

        public static TweenAnimatorNormalizedTime TweenNormalizedTime(this Animator animator, int layerIndex, string clipName, float to, float duration)
        {
            var tweener = UTween.NormalizedTime(animator, layerIndex, clipName, to, duration);
            return tweener;
        }

        public static TweenAnimatorNormalizedTime TweenNormalizedTime(this Animator animator, int layerIndex, string clipName, float from, float to, float duration)
        {
            var tweener = UTween.NormalizedTime(animator, layerIndex, clipName, from, to, duration);
            return tweener;
        }
    }

    #endregion
}