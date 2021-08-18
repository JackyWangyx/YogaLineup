using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public abstract partial class Tweener<TTarget, TValue> : Tweener<TTarget>
        where TTarget : UnityEngine.Object
    {
        public TValue From;
        public TValue To;
        public AxisConstraint Axis;

        [NonSerialized] public Func<TValue> FromGetter;
        [NonSerialized] public Func<TValue> ToGetter;
        [NonSerialized] public Func<TValue> ValueGetter;
        [NonSerialized] public Action<TValue> ValueSetter;

        public Action<TValue> OnUpdate;

        #region Axis Constraint

        public bool AxisX
        {
            get => (Axis & AxisConstraint.X) > 0;
            set
            {
                if (value)
                {
                    Axis |= AxisConstraint.X;
                }
                else
                {
                    Axis &= ~AxisConstraint.X;
                }
            }
        }

        public bool AxisY
        {
            get => (Axis & AxisConstraint.Y) > 0;
            set
            {
                if (value)
                {
                    Axis |= AxisConstraint.Y;
                }
                else
                {
                    Axis &= ~AxisConstraint.Y;
                }
            }
        }

        public bool AxisZ
        {
            get => (Axis & AxisConstraint.Z) > 0;
            set
            {
                if (value)
                {
                    Axis |= AxisConstraint.Z;
                }
                else
                {
                    Axis &= ~AxisConstraint.Z;
                }
            }
        }

        public bool AxisW
        {
            get => (Axis & AxisConstraint.W) > 0;
            set
            {
                if (value)
                {
                    Axis |= AxisConstraint.W;
                }
                else
                {
                    Axis &= ~AxisConstraint.W;
                }
            }
        }

        #endregion

        public abstract TValue Value { get; set; }

        public virtual TValue RecordValue { get; set; }

        public override void PreSample()
        {
            base.PreSample();
            RefreshGetterSetter();
        }

        public override void RecordObject()
        {
            RecordValue = Value;
        }

        public override void RestoreObject()
        {
            Value = RecordValue;
        }

        public virtual void DisableIndependentAxis()
        {
            Axis = AxisConstraint.All;
        }

        public override void Reset()
        {
            base.Reset();
            From = default(TValue);
            To = default(TValue);
            ResetGetterSetter();
            DisableIndependentAxis();
        }

        public virtual void RefreshGetterSetter()
        {
            if (FromGetter == null) FromGetter = () => From;
            if (ToGetter == null) ToGetter = () => To;
            if (ValueGetter == null) ValueGetter = () => Value;
            if (ValueSetter == null) ValueSetter = value => Value = value;
        }

        public virtual void ResetGetterSetter()
        {
            FromGetter = () => From;
            ToGetter = () => To;
            ValueGetter = () => Value;
            ValueSetter = value => Value = value;
        }

        public override void ReverseFromTo()
        {
            var temp = From;
            From = To;
            To = temp;
        }

        public override void ResetCallback()
        {
            OnUpdate = null;
        }
    }

#if UNITY_EDITOR

    public abstract partial class Tweener<TTarget, TValue> : Tweener<TTarget>
        where TTarget : UnityEngine.Object
    {
        [SerializeField] internal bool EnableIndependentAxis = false;

        [NonSerialized] public SerializedProperty FromProperty;
        [NonSerialized] public SerializedProperty ToProperty;
        [NonSerialized] public SerializedProperty AxisProperty;
        [NonSerialized] public SerializedProperty OnUpdateEventProperty;

        public virtual string AxisXName => nameof(AxisConstraint.X);
        public virtual string AxisYName => nameof(AxisConstraint.Y);
        public virtual string AxisZName => nameof(AxisConstraint.Z);
        public virtual string AxisWName => nameof(AxisConstraint.W);

        public override void InitEditor(int index, TweenData data, SerializedProperty tweenerProperty)
        {
            base.InitEditor(index, data, tweenerProperty);
            FromProperty = TweenerProperty.FindPropertyRelative(nameof(From));
            ToProperty = TweenerProperty.FindPropertyRelative(nameof(To));
            AxisProperty = TweenerProperty.FindPropertyRelative(nameof(Axis));
        }

        public override void DrawIndependentAxis()
        {
            if (!SupportIndependentAxis || !EnableIndependentAxis) return;
            using (GUIHorizontal.Create())
            {
                GUILayout.Label(nameof(Axis), EditorStyles.label, GUILayout.Width(EditorGUIUtility.labelWidth));
                using (GUILabelWidthArea.Create(EditorStyle.CharacterWidth))
                {
                    var toggleAxises = new bool[4];
                    if (AxisCount >= 1) toggleAxises[0] = EditorGUILayout.Toggle(AxisXName, AxisX);
                    if (AxisCount >= 2) toggleAxises[1] = EditorGUILayout.Toggle(AxisYName, AxisY);
                    if (AxisCount >= 3) toggleAxises[2] = EditorGUILayout.Toggle(AxisZName, AxisZ);
                    if (AxisCount >= 4) toggleAxises[3] = EditorGUILayout.Toggle(AxisWName, AxisW);
                    var axis = 0;
                    for (var i = 0; i < 4; i++)
                    {
                        if (toggleAxises[i]) axis |= 2 << i;
                    }

                    AxisProperty.intValue = axis;
                }
            }
        }

        public override void DrawFromToValue()
        {
            using (GUIVertical.Create())
            {
                GUIUtil.DrawProperty(HoldStartProperty, FromProperty);
                GUIUtil.DrawProperty(HoldEndProperty, ToProperty);
            }
        }

        public override void DrawEvent()
        {

        }

        #region Context Menu

        public override GenericMenu CreateContextMenu()
        {
            var menu = base.CreateContextMenu();

            // Show / Hide Independent Axis
            if (SupportIndependentAxis)
            {
                menu.AddSeparator("");
                if (EnableIndependentAxis)
                {
                    menu.AddItem(new GUIContent("Disable Independent Axis"), false, () =>
                    {
                        Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Disable Independent Axis");
                        EnableIndependentAxis = !EnableIndependentAxis;
                        DisableIndependentAxis();
                    });
                }
                else
                {
                    menu.AddItem(new GUIContent("Enable Independent Axis"), false, () =>
                    {
                        Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Enable Independent Axis");
                        EnableIndependentAxis = !EnableIndependentAxis;
                    });
                }
            }

            // Reverse From To
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Reverse From - To"), false, () =>
            {
                Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Reverse From - To");
                ReverseFromTo();
            });

            // Current - From / To
            if (SupportSetCurrentValue)
            {
                menu.AddItem(Target != null, "Set Current -> From", false, () =>
                {
                    Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Set Current -> From");
                    From = Value;
                });
                menu.AddItem(Target != null, "Set Current -> To", false, () =>
                {
                    Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Set Current -> To");
                    To = Value;
                });
                menu.AddItem(Target != null, "Set From -> Current", false, () =>
                {
                    Value = From;
                });
                menu.AddItem(Target != null, "Set To -> Current", false, () =>
                {
                    Value = To;
                });
            }

            // Expand / Shrink 
            menu.AddSeparator("");
            menu.AddItem("Expand Others", false, () =>
            {
                Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Expand All Tweener");
                foreach (var tweener in Data.TweenerList)
                {
                    if (tweener == this) continue;
                    tweener.FoldOut = true;
                }
            });
            menu.AddItem("Shrink Others", false, () =>
            {
                Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Expand All Tweener");
                foreach (var tweener in Data.TweenerList)
                {
                    if (tweener == this) continue;
                    tweener.FoldOut = false;
                }
            });

            // Active / DeActive
            menu.AddSeparator("");
            menu.AddItem("Active Others", false, () =>
            {
                Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Active Others");
                foreach (var tweener in Data.TweenerList)
                {
                    if (tweener == this) continue;
                    tweener.Active = true;
                }
            });
            menu.AddItem("DeActive Others", false, () =>
            {
                Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "DeActive Others");
                foreach (var tweener in Data.TweenerList)
                {
                    if (tweener == this) continue;
                    tweener.Active = false;
                }
            });

            return menu;
        } 

        #endregion
    }

#endif
}