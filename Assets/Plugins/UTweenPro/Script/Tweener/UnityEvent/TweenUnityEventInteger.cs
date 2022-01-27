﻿using System;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Unity Event Integer", "Unity Event", "BuildSettings.Editor")]
    [Serializable]
    public partial class TweenUnityEventInteger : TweenValueInteger<Object>
    {
        public override bool SupportTarget => false;

        public OnValueIntegerEvent Event = new OnValueIntegerEvent();

        public override int Value
        {
            get => _value;
            set
            {
                _value = value;
                Event.Invoke(_value);
            }
        }

        private int _value;

        public override void Reset()
        {
            base.Reset();
            Event.RemoveAllListeners();
        }
    }

#if UNITY_EDITOR

    public partial class TweenUnityEventInteger : TweenValueInteger<Object>
    {
        [TweenerProperty, NonSerialized] public SerializedProperty EventProperty;

        public override void DrawTarget()
        {
            base.DrawTarget();
            EditorGUILayout.PropertyField(EventProperty);
        }
    }

#endif
}