#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Aya.TweenPro
{
    public static class EditorIcon
    {
        #region Tweener Group Icon

        public static Dictionary<string, Texture2D> TweenerGroupIconDic
        {
            get
            {
                if (_tweenerGroupIconDic == null) CacheTweenerGroupIcon();
                return _tweenerGroupIconDic;
            }
        }

        private static Dictionary<string, Texture2D> _tweenerGroupIconDic;

        public static void CacheTweenerGroupIcon()
        {
            _tweenerGroupIconDic = new Dictionary<string, Texture2D>
            {
                {"Transform", CreateIcon(typeof(Transform))},
                {"RectTransform", CreateIcon(typeof(RectTransform))},
                {"UGUI", CreateIcon(typeof(Canvas))},
                {"UGUI Text", CreateIcon(typeof(Text))},
                {"UGUI Layout", CreateIcon(typeof(GridLayoutGroup))},
                {"UGUI Scroll", CreateIcon(typeof(ScrollRect))},
                {"UGUI Effect", CreateIcon(typeof(Shadow))},
                {"Path", CreateIcon(typeof(EdgeCollider2D))},
                {"Physics", CreateIcon(typeof(Rigidbody))},
                {"Rendering", CreateIcon("BuildSettings.Editor")},
                {"Material", CreateIcon(typeof(Material))},
                {"Camera", CreateIcon(typeof(Camera))},
                {"Audio", CreateIcon(typeof(AudioSource))},
                {"Misc", CreateIcon("GameManager Icon")},
                {"Property", CreateIcon("cs Script Icon")},
            };
        }

        public static Texture2D GetTweenerGroupIcon(string group)
        {
            if (TweenerGroupIconDic.TryGetValue(group, out var icon)) return icon;
            return default;
        }

        #endregion

        #region Tweener Icon

        private static Dictionary<Type, Texture2D> _tweenerIconDic;

        public static Texture2D GetTweenerIcon(Type tweenerType)
        {
            if (_tweenerIconDic == null) _tweenerIconDic = new Dictionary<Type, Texture2D>();
            if (!_tweenerIconDic.TryGetValue(tweenerType, out var icon))
            {
                var tweenerAttribute = TypeCaches.TweenerTypeDic[tweenerType];
                if (string.IsNullOrEmpty(tweenerAttribute.IconName))
                {
                    var targetType = tweenerType.GetField("Target")?.FieldType;
                    icon = EditorGUIUtility.ObjectContent(null, targetType).image as Texture2D;
                }
                else
                {
                    icon = GetIcon(tweenerAttribute.IconName);
                }

                _tweenerIconDic.Add(tweenerType, icon);
            }

            return icon;
        }

        #endregion

        #region Icon Method

        public static Texture2D CreateIcon(Type type, int size = 24)
        {
            var srcIcon = GetIcon(type);
            if (srcIcon == null) return default;
            var icon = CreateIconWithSrc(srcIcon, size);
            return icon;
        }

        public static Texture2D CreateIcon(string name, int size = 24)
        {
            var srcIcon = GetIcon(name);
            if (srcIcon == null) return default;
            var icon = CreateIconWithSrc(srcIcon, size);
            return icon;
        }

        public static Texture2D GetIcon(Type type)
        {
            if (type != null) return EditorGUIUtility.ObjectContent(null, type).image as Texture2D;
            return default;
        }

        public static Texture2D GetIcon(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var icon = EditorGUIUtility.FindTexture(name);
                if (icon == null) icon = AssetDatabase.GetCachedIcon(name) as Texture2D;
                if (icon == null) icon = EditorGUIUtility.IconContent(name).image as Texture2D;
                return icon;
            }

            return default;
        }

        public static Texture2D CreateIconWithSrc(Texture2D srcIcon, int size = 24)
        {
            // Copy Built-in texture with RenderTexture
            var tempRenderTexture = RenderTexture.GetTemporary(size, size, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            Graphics.Blit(srcIcon, tempRenderTexture);
            var previousRenderTexture = RenderTexture.active;
            RenderTexture.active = tempRenderTexture;
            var icon = new Texture2D(size, size);
            icon.ReadPixels(new Rect(0, 0, tempRenderTexture.width, tempRenderTexture.height), 0, 0);
            icon.Apply();
            RenderTexture.ReleaseTemporary(tempRenderTexture);
            RenderTexture.active = previousRenderTexture;
            return icon;
        }

        #endregion
    }

}
#endif