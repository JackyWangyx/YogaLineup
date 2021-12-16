using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Animator Time", "Animation")]
    [Serializable]
    public partial class TweenAnimatorTime : TweenValueFloat<Animator>
    {
        public string State;
        public int Layer;
        public float Fade;

        public override float Value
        {
            get => CurrentStateDuration;
            set
            {
                var duration = CurrentStateDuration;
                var factor = value / duration;
                if (Application.isPlaying)
                {
                    Target.CrossFade(State, Fade, Layer, factor);
                }
                else
                {
                    Target.Play(State, Layer, factor);
                    Target.Update(0);
                }
            }
        }

        public float CurrentStateDuration
        {
            get
            {
                var stateInfo = Target.GetCurrentAnimatorStateInfo(Layer);
                return stateInfo.length;
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

    public partial class TweenAnimatorTime : TweenValueFloat<Animator>
    {
        [TweenerProperty, NonSerialized] public SerializedProperty StateProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty LayerProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty FadeProperty;

        public override void DrawTarget()
        {
            base.DrawTarget();

            if (Target == null)
            {
                StateProperty.stringValue = null;
                return;
            }

            GUIMenu.SelectAnimatorStateMenu(Target, nameof(State), StateProperty);

            using (GUIHorizontal.Create())
            {
                EditorGUILayout.PropertyField(LayerProperty);
                EditorGUILayout.PropertyField(FadeProperty);
            }
        }
    }

#endif

    #region Extension

    public partial class TweenAnimatorTime : TweenValueFloat<Animator>
    {
        public TweenAnimatorTime SetState(string stateMName)
        {
            State = stateMName;
            return this;
        }

        public TweenAnimatorTime SetLayerIndex(int layerIndex)
        {
            Layer = layerIndex;
            return this;
        }

        public TweenAnimatorTime SetFadeTime(float fadeTime)
        {
            Fade = Mathf.Clamp01(fadeTime);
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenAnimatorTime Time(Animator animator, string clipName, float to, float duration)
        {
            var tweener = Play<TweenAnimatorTime, Animator, float>(animator, to, duration)
                .SetLayerIndex(0)
                .SetState(clipName)
                .SetFadeTime(0f);
            return tweener;
        }

        public static TweenAnimatorTime Time(Animator animator, string clipName, float from, float to, float duration)
        {
            var tweener = Play<TweenAnimatorTime, Animator, float>(animator, from, to, duration)
                .SetLayerIndex(0)
                .SetState(clipName)
                .SetFadeTime(0f);
            return tweener;
        }

        public static TweenAnimatorTime Time(Animator animator, int layerIndex, string clipName, float to, float duration)
        {
            var tweener = Play<TweenAnimatorTime, Animator, float>(animator, to, duration)
                .SetLayerIndex(layerIndex)
                .SetState(clipName)
                .SetFadeTime(0f);
            return tweener;
        }

        public static TweenAnimatorTime Time(Animator animator, int layerIndex, string clipName, float from, float to, float duration)
        {
            var tweener = Play<TweenAnimatorTime, Animator, float>(animator, from, to, duration)
                .SetLayerIndex(layerIndex)
                .SetState(clipName)
                .SetFadeTime(0f);
            return tweener;
        }
    }

    public static partial class AnimatorExtension
    {
        public static TweenAnimatorTime TweenTime(this Animator animator, string clipName, float to, float duration)
        {
            var tweener = UTween.Time(animator, clipName, to, duration);
            return tweener;
        }

        public static TweenAnimatorTime TweenTime(this Animator animator, string clipName, float from, float to, float duration)
        {
            var tweener = UTween.Time(animator, clipName, from, to, duration);
            return tweener;
        }

        public static TweenAnimatorTime TweenTime(this Animator animator, int layerIndex, string clipName, float to, float duration)
        {
            var tweener = UTween.Time(animator, layerIndex, clipName, to, duration);
            return tweener;
        }

        public static TweenAnimatorTime TweenTime(this Animator animator, int layerIndex, string clipName, float from, float to, float duration)
        {
            var tweener = UTween.Time(animator, layerIndex, clipName, from, to, duration);
            return tweener;
        }
    }

    #endregion
}