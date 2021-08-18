using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public abstract partial class Tweener<TTarget> : Tweener
        where TTarget : UnityEngine.Object
    {
        public TTarget Target;

        public override void SetDirty()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying && Target != null)
            {
                EditorUtility.SetDirty(Target);
            }
#endif
        }

        public override void Reset()
        {
            base.Reset();
            Target = null;
        }
    }

#if UNITY_EDITOR

    public abstract partial class Tweener<TTarget> : Tweener
        where TTarget : UnityEngine.Object
    {
        [NonSerialized] public SerializedProperty TargetProperty;
        [NonSerialized] public Texture TargetIcon;

        public override void InitParam(TweenData data, MonoBehaviour target)
        {
            base.InitParam(data, target);
            if (target == null) return;
            if (typeof(TTarget).IsSubclassOf(typeof(Component)))
            {
                Target = target.GetComponentInChildren<TTarget>(true);
            }
            else if (typeof(TTarget) == typeof(GameObject))
            {
                Target = (TTarget)(object)target.gameObject;
            }
        }

        public override void InitEditor(int index, TweenData data, SerializedProperty tweenerProperty)
        {
            base.InitEditor(index, data, tweenerProperty);
            TargetProperty = TweenerProperty.FindPropertyRelative(nameof(Target));
        }

        public override void DrawHeaderIcon()
        {
            if (TargetIcon == null)
            {
                TargetIcon = EditorIcon.GetTweenerIcon(GetType());
            }

            if (TargetIcon == null) return;
            var size = EditorStyle.TweenerHeaderIconSize;
            var btnIcon = GUILayout.Button("", EditorStyles.label, GUILayout.Width(size), GUILayout.Height(size));
            if (btnIcon)
            {
                FoldOut = !FoldOut;
            }

            var rect = GUILayoutUtility.GetLastRect();
            GUI.DrawTexture(rect, TargetIcon);
        }

        public override void DrawTarget()
        {
            if (!SupportTarget) return;
            if (!FoldOut) return;
            if (Data.Mode == TweenEditorMode.ScriptableObject) return;
            if (typeof(TTarget).IsSubclassOf(typeof(Component)) || typeof(TTarget) == typeof(Component))
            {
                GUIMenu.ComponentTreeMenu(typeof(TTarget), nameof(Target), TargetProperty, MonoBehaviour.transform, target =>
                {
                });
            }
            else
            {
                TargetProperty.objectReferenceValue = EditorGUILayout.ObjectField(nameof(Target), TargetProperty.objectReferenceValue, typeof(TTarget), true);
            }
        }
    }

#endif
}