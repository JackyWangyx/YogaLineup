using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Animation Normalized Time", "Rendering")]
    [Serializable]
    public partial class TweenAnimationNormalizedTime : TweenValueFloat<Animation>
    {
        public string Clip;

        public override float Value
        {
            get
            {
                var state = Target[Clip];
                if (state == null) return default;
                var progress = state.time / state.length;
                return progress;
            }
            set
            {
                var state = Target[Clip];
                if (state == null) return;
                var clip = state.clip;
                var time = clip.length * value;
                clip.SampleAnimation(Target.gameObject, time);
            }
        }

        public override void Reset()
        {
            base.Reset();
            Clip = null;
        }
    }

#if UNITY_EDITOR

    public partial class TweenAnimationNormalizedTime : TweenValueFloat<Animation>
    {
        [NonSerialized] public SerializedProperty ClipProperty;

        public override void InitEditor(int index, TweenData data, SerializedProperty tweenerProperty)
        {
            base.InitEditor(index, data, tweenerProperty);
            ClipProperty = TweenerProperty.FindPropertyRelative(nameof(Clip));
        }

        public override void DrawTarget()
        {
            base.DrawTarget();
            if (Target == null)
            {
                ClipProperty.stringValue = null;
                return;
            }

            if (Target != null && string.IsNullOrEmpty(Clip) && Target.GetClipCount() > 0)
            {
                ClipProperty.stringValue = AnimationUtility.GetAnimationClips(Target.gameObject)[0].name;
            }

            using (GUIColorArea.Create(EditorStyle.ErrorColor, string.IsNullOrEmpty(Clip)))
            {
                using (GUIHorizontal.Create())
                {
                    GUILayout.Label(nameof(Clip), GUILayout.Width(EditorStyle.LabelWidth));
                    var showName = string.IsNullOrEmpty(Clip) ? EditorStyle.NoneStr : Clip;
                    var btnClip = GUILayout.Button(showName, EditorStyles.popup);
                    if (btnClip)
                    {
                        var menu = new GenericMenu();
                        menu.AddItem(EditorStyle.NoneStr, string.IsNullOrEmpty(Clip), () =>
                        {
                            ClipProperty.stringValue = null;
                            ClipProperty.serializedObject.ApplyModifiedProperties();
                        });
                        menu.AddSeparator("");

                        var clips = AnimationUtility.GetAnimationClips(Target.gameObject);
                        foreach (var clip in clips)
                        {
                            var clipName = clip.name;
                            menu.AddItem(clipName, Clip == clipName, () =>
                            {
                                ClipProperty.stringValue = clipName;
                                ClipProperty.serializedObject.ApplyModifiedProperties();
                            });
                        }

                        menu.ShowAsContext();
                    }
                }
            }
        }
    }

#endif

    #region Extension

    public partial class TweenAnimationNormalizedTime : TweenValueFloat<Animation>
    {
        public TweenAnimationNormalizedTime SetClip(string clipName)
        {
            Clip = clipName;
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenAnimationNormalizedTime NormalizedTime(Animation animation, string clipName, float to, float duration)
        {
            var tweener = Play<TweenAnimationNormalizedTime, Animation, float>(animation, to, duration)
                .SetClip(clipName);
            return tweener;
        }

        public static TweenAnimationNormalizedTime NormalizedTime(Animation animation, string clipName, float from, float to, float duration)
        {
            var tweener = Play<TweenAnimationNormalizedTime, Animation, float>(animation, from, to, duration)
                .SetClip(clipName);
            return tweener;
        }
    }

    public static partial class AnimationExtension
    {
        public static TweenAnimationNormalizedTime TweenNormalizedTime(this Animation animation, string clipName, float to, float duration)
        {
            var tweener = UTween.NormalizedTime(animation, clipName, to, duration);
            return tweener;
        }

        public static TweenAnimationNormalizedTime TweenNormalizedTime(this Animation animation, string clipName, float from, float to, float duration)
        {
            var tweener = UTween.NormalizedTime(animation, clipName, from, to, duration);
            return tweener;
        }
    }

    #endregion

}