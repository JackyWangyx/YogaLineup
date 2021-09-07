using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    public static class TypeCaches
    {
        public static BindingFlags DefaultBindingFlags => BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

#if UNITY_EDITOR

        public static Dictionary<Type, TweenerAttribute> TweenerTypeDic
        {
            get
            {
                if (_tweenerTypeDic == null)
                {
                    _tweenerTypeDic = new Dictionary<Type, TweenerAttribute>();
                    var tweenerTypes = TypeCache.GetTypesWithAttribute<TweenerAttribute>();
                    foreach (var type in tweenerTypes)
                    {
                        var attribute = type.GetCustomAttribute<TweenerAttribute>();
                        _tweenerTypeDic.Add(type, attribute);
                    }
                }

                return _tweenerTypeDic;
            }
        }

        private static Dictionary<Type, TweenerAttribute> _tweenerTypeDic;

#endif
    }
}
