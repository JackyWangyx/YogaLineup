using System;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class TweenEvent
    {
        public UnityEvent Event;
        public Action Action;

        public void AddListener(Action action)
        {
            Action += action;
        }

        public void RemoveListener(Action action)
        {
            Action -= action;
        }

        public void Invoke()
        {
            Event?.Invoke();
            Action?.Invoke();
        }

        public void Reset()
        {
            Event = null;
            Action = delegate { };
        }

        public void InitEvent()
        {
            Event = new UnityEvent();
        }
    }

#if UNITY_EDITOR

    public partial class TweenEvent
    {
        [NonSerialized] public SerializedProperty TweenDataProperty;
        [NonSerialized] public SerializedProperty CallbackProperty;
        [NonSerialized] public SerializedProperty EventProperty;

        public void InitEditor(SerializedProperty tweenDataProperty, string propertyName)
        {
            TweenDataProperty = tweenDataProperty;
            CallbackProperty = TweenDataProperty.FindPropertyRelative(propertyName);
            EventProperty = CallbackProperty.FindPropertyRelative(nameof(Event));
        }

        public void DrawEvent(string eventName)
        {
            if (Event == null) InitEvent();
            EditorGUILayout.PropertyField(EventProperty, new GUIContent(eventName));
        }
    }

#endif

}
