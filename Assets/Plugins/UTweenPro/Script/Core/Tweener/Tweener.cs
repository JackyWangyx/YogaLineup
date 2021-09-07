using System;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public abstract partial class Tweener
    {
        public bool Active;
        public float Duration;
        public float Delay;
        public bool HoldStart;
        public bool HoldEnd;
        public int Ease;
        public float Strength;
        public AnimationCurve Curve;
        public SpaceMode Space;

        [SerializeField] internal bool FoldOut = true;
        [SerializeField] internal TweenDurationMode DurationMode = TweenDurationMode.DurationDelay;

        public virtual bool SupportTarget => true;
        public virtual bool SupportSpace => false;
        public virtual bool SupportSpeedBased => false;
        public virtual bool SupportSetCurrentValue => true;
        public virtual bool SupportOnUpdate => true;
        public virtual bool SupportIndependentAxis => false;
        public virtual int AxisCount => 1;

        [NonSerialized] public float Factor;

        public bool IsPrepared { get; internal set; }
        public bool SingleMode => Data != null && Data.SingleMode;
        public float TotalDuration => Delay + Duration;

        #region Cache

        public float DurationFrom
        {
            get => Delay;
            set
            {
                if (value < 0) Delay = 0;
                else Delay = value;
            }
        }

        public float DurationTo
        {
            get => Delay + Duration;
            set
            {
                if (value > Delay)
                {
                    Duration = value - Delay;
                }
                else
                {
                    if (value < 0)
                    {
                        Delay = 0;
                    }
                    else
                    {
                        Delay = value;
                    }

                    Duration = 0;
                }
            }
        }


        public float DurationFromNormalized
        {
            get => DurationFrom / Data.Duration;
            set => DurationFrom = Data.Duration * value;
        }

        public float DurationToNormalized
        {
            get => DurationTo / Data.Duration;
            set => DurationTo = Data.Duration * value;
        }

        public EaseFunction CacheEaseFunction
        {
            get
            {
                if (_cacheEaseFunction == null || _cacheEaseFunction.Type != Ease)
                {
                    _cacheEaseFunction = EaseType.FunctionDic[Ease];
                }

                return _cacheEaseFunction;
            }
        }

        private EaseFunction _cacheEaseFunction;

        public TweenData Data { get; internal set; }
        public bool IsCustomCurve => Ease < 0;

        #endregion

        public virtual void PreSample()
        {
            IsPrepared = true;
        }

        public virtual float GetSpeedBasedDuration()
        {
            return Duration;
        }

        public virtual float GetFactor(float normalizedDuration)
        {
            var currentDuration = Data.Duration * normalizedDuration;
            if (!Data.SingleMode)
            {
                if (currentDuration < Delay)
                {
                    Factor = 0f;
                    return HoldStart ? 0f : float.NaN;
                }

                if (currentDuration > Delay + Duration)
                {
                    Factor = 1f;
                    return HoldEnd ? 1f : float.NaN;
                }
            }

            var delta = (currentDuration - Delay) / Duration;
            var factor = IsCustomCurve ? Curve.Evaluate(delta) : CacheEaseFunction.Ease(0f, 1f, delta, Strength);
            Factor = factor;
            return factor;
        }

        public abstract void Sample(float factor);

        public virtual void RecordObject()
        {

        }

        public virtual void RestoreObject()
        {

        }

        public virtual void SetDirty()
        {

        }

        public virtual void OnAdded()
        {

        }

        public virtual void OnRemoved()
        {

        }

        public virtual void Reset()
        {
            ResetCallback();
            IsPrepared = false;
            Active = true;
            Duration = 1f;
            Delay = 0f;
            HoldStart = false;
            HoldEnd = false;
            Ease = EaseType.Linear;
            Strength = 1f;
            Curve = new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));
            Space = SpaceMode.World;
#if UNITY_EDITOR
            // ShowEvent = false;
#endif
        }

        public virtual void ResetCallback()
        {

        }

        public virtual void ReverseFromTo()
        {

        }

        public virtual void OnDrawGizmos()
        {

        }
    }

#if UNITY_EDITOR

    public abstract partial class Tweener
    {
        [NonSerialized] public SerializedProperty TweenerProperty;
        [NonSerialized] public SerializedProperty ActiveProperty;
        [NonSerialized] public SerializedProperty DurationProperty;
        [NonSerialized] public SerializedProperty DelayProperty;
        [NonSerialized] public SerializedProperty HoldStartProperty;
        [NonSerialized] public SerializedProperty HoldEndProperty;
        [NonSerialized] public SerializedProperty EaseProperty;
        [NonSerialized] public SerializedProperty StrengthProperty;
        [NonSerialized] public SerializedProperty CurveProperty;
        [NonSerialized] public SerializedProperty SpaceProperty;

        public Editor Editor => Data.Editor;
        public Object EditorTarget => Data.EditorTarget;
        public MonoBehaviour MonoBehaviour => Data.MonoBehaviour;
        public GameObject GameObject => Data.GameObject;
        public SerializedObject SerializedObject => Data.SerializedObject;

        [NonSerialized] public int Index = -1;

        public bool CanMoveDown => Index < Data.TweenerList.Count - 1;
        public bool CanMoveUp => Index > 0;

        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(_displayName))
                {
                    _displayName = TypeCaches.TweenerTypeDic[GetType()].DisplayName;
                }

                return _displayName;
            }
        }

        private static Tweener _clipboard;
        private string _displayName;

        public virtual void InitParam(TweenData data, MonoBehaviour target)
        {
            Data = data;
            Duration = Data.Duration;
        }

        public virtual void InitEditor(int index, TweenData data, SerializedProperty tweenerProperty)
        {
            Index = index;
            Data = data;
            TweenerProperty = tweenerProperty;

            ActiveProperty = TweenerProperty.FindPropertyRelative(nameof(Active));
            DurationProperty = TweenerProperty.FindPropertyRelative(nameof(Duration));
            DelayProperty = TweenerProperty.FindPropertyRelative(nameof(Delay));
            HoldStartProperty = TweenerProperty.FindPropertyRelative(nameof(HoldStart));
            HoldEndProperty = TweenerProperty.FindPropertyRelative(nameof(HoldEnd));
            EaseProperty = TweenerProperty.FindPropertyRelative(nameof(Ease));
            StrengthProperty = TweenerProperty.FindPropertyRelative(nameof(Strength));
            CurveProperty = TweenerProperty.FindPropertyRelative(nameof(Curve));
            SpaceProperty = TweenerProperty.FindPropertyRelative(nameof(Space));

            _clearEaseCurvePreview();
        }

        public virtual void DrawTweener()
        {
            using (GUIGroup.Create())
            {
                DrawTweenerHeader();

                if (Data.SpeedBased && !SupportSpeedBased)
                {
                    GUIUtil.DrawTipArea(EditorStyle.ErrorColor, "Not Support Speed Based!");
                }

                if (!FoldOut) return;
                using (GUIEnableArea.Create(ActiveProperty.boolValue && !Data.IsInProgress && GUI.enabled))
                {
                    DrawTarget();
                    DrawIndependentAxis();
                    DrawFromToValue();

                    DrawBody();
                    DrawDurationDelay();
                    DrawEaseCurve();
                    DrawAppend();

                    // if (Data.Mode == TweenEditorMode.Component && SupportOnUpdate && ShowEvent)
                    // {
                    //     DrawEvent();
                    // }
                }
            }
        }

        public virtual void DrawTweenerHeader()
        {
            var name = DisplayName;
            using (GUIHorizontal.Create())
            {
                // Foldout Toggle
                FoldOut = EditorGUILayout.Toggle(GUIContent.none, FoldOut, EditorStyles.foldout, GUILayout.Width(EditorStyle.CharacterWidth));

                // Icon
                DrawHeaderIcon();
                GUILayout.Space(2);

                // Active Toggle
                using (GUIEnableArea.Create(!Data.IsInProgress))
                {
                    ActiveProperty.boolValue = EditorGUILayout.Toggle(ActiveProperty.boolValue, GUILayout.Width(EditorStyle.CharacterWidth));
                    GUILayout.Space(2);
                }

                // Title
                var btnTitle = GUILayout.Button(name, EditorStyles.boldLabel);
                var btnFlexibleSpace = GUILayout.Button(GUIContent.none, EditorStyles.label);
                if (btnTitle || btnFlexibleSpace)
                {
                    FoldOut = !FoldOut;
                }

                // Menu Button
                using (GUIEnableArea.Create(!Data.IsInProgress))
                {
                    var btnContextMenu = GUILayout.Button(GUIContent.none, EditorStyles.foldoutHeaderIcon, GUILayout.Width(EditorStyle.CharacterWidth));
                    if (btnContextMenu)
                    {
                        var menu = CreateContextMenu();
                        menu.ShowAsContext();
                    }
                }
            }

            DrawProgressBar();
        }

        public virtual void DrawHeaderIcon()
        {

        }

        public virtual void DrawProgressBar()
        {
            if (Data.SingleMode) return;
            using (GUIGroup.Create())
            {
                using (GUIEnableArea.Create(Active))
                {
                    var currentHeight = FoldOut ? 5f : 3f;
                    var normalized = Data.EditorNormalizedProgress;
                    if (!Active) normalized = 0f;
                    GUIUtil.DrawDraggableProgressBar(SerializedObject.targetObject, currentHeight, DurationFromNormalized, DurationToNormalized, normalized, (from, to) =>
                    {
                        if (Data.IsPlaying) return;
                        DurationFromNormalized = from;
                        DurationToNormalized = to;
                    });
                }
            }
        }

        public virtual void DrawTarget()
        {

        }

        public virtual void DrawIndependentAxis()
        {

        }

        public virtual void DrawFromToValue()
        {

        }

        public virtual void DrawDurationDelay()
        {
            using (GUIHorizontal.Create())
            {
                if (Data.SingleMode)
                {
                    DurationProperty.floatValue = Data.Duration;
                    DelayProperty.floatValue = 0f;
                    return;
                }

                if (DurationMode == TweenDurationMode.DurationDelay)
                {
                    DurationProperty.floatValue = (float) Math.Round(EditorGUILayout.FloatField(nameof(Duration), DurationProperty.floatValue), 3);
                    DelayProperty.floatValue = (float) Math.Round(EditorGUILayout.FloatField(nameof(Delay), DelayProperty.floatValue), 3);
                }

                if (DurationMode == TweenDurationMode.FromTo)
                {
                    GUILayout.Label("Time", EditorStyles.label, GUILayout.Width(EditorStyle.LabelWidth));
                    using (GUILabelWidthArea.Create(EditorStyle.CharacterWidth))
                    {
                        DelayProperty.floatValue = (float) Math.Round(GUIUtil.DrawFloatProperty("F", DelayProperty.floatValue, true), 3);
                        DurationProperty.floatValue = (float) Math.Round(GUIUtil.DrawFloatProperty("T", DelayProperty.floatValue + DurationProperty.floatValue, true) - DelayProperty.floatValue, 3);
                    }
                }

                if (Duration < 0) DurationProperty.floatValue = 0f;
                if (Delay < 0) DelayProperty.floatValue = 0f;
                if (Duration + Delay > Data.Duration)
                {
                    DelayProperty.floatValue = Data.Duration - Duration;
                    if (Delay < 0) DelayProperty.floatValue = 0f;
                    if (Duration > Data.Duration) DurationProperty.floatValue = Data.Duration;
                }
            }
        }

        public virtual void DrawEaseCurve()
        {
            using (GUIHorizontal.Create())
            {
                // Ease
                GUILayout.Label(nameof(Ease), EditorStyles.label, GUILayout.Width(EditorStyle.LabelWidth));
                var displayEaseName = EaseType.FunctionInfoDic[Ease].DisplayName;
                var easeRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, EditorStyles.popup);
                var easeTypeBtn = GUI.Button(easeRect, displayEaseName, EditorStyles.popup);
                if (easeTypeBtn)
                {
                    var menu = CreateEaseTypeMenu();
                    menu.ShowAsContext();
                }

                // Curve
                GUILayout.Label(nameof(Curve), EditorStyles.label, GUILayout.Width(EditorStyle.LabelWidth));
                // var curveRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, EditorStyles.popup);
                if (Ease < 0)
                {
                    CurveProperty.animationCurveValue = EditorGUILayout.CurveField(CurveProperty.animationCurveValue);
                    // TODO.. curve menu
                    // var buttonWidth = EditorStyle.CharacterWidth;
                    // var btnCurveMenu = GUILayout.Button("∵", EditorStyles.miniButton, GUILayout.Width(buttonWidth));
                    // if (btnCurveMenu)
                    // {
                    //     var menu = CreateCustomCurveMenu();
                    //     menu.ShowAsContext();
                    // }
                }
                else
                {
                    using (GUIEnableArea.Create(false))
                    {
                        _cacheEaseCurve(CacheEaseFunction);
                        EditorGUILayout.CurveField(_cacheCurve); //, GUILayout.Height(EditorStyle.SingleButtonWidth * 2f));
                    }
                }
            }

            // Strength
            if(CacheEaseFunction.SupportStrength)
            {
                using (GUIHorizontal.Create())
                {
                    GUILayout.Label(nameof(Strength), EditorStyles.label, GUILayout.Width(EditorStyle.LabelWidth));
                    StrengthProperty.floatValue = GUILayout.HorizontalSlider(StrengthProperty.floatValue, 0f, 1f);
                }
            }
        }

        private AnimationCurve _cacheCurve;
        private int _cacheType;
        private float _cacheStrength;

        private void _clearEaseCurvePreview()
        {
            _cacheCurve = null;
            _cacheType = -1;
            _cacheStrength = 1f;
        }

        private void _cacheEaseCurve(EaseFunction easeFunction)
        {
            if (_cacheType == easeFunction.Type && Math.Abs(_cacheStrength - Strength) > 1e-6f && _cacheCurve != null) return;
            _cacheType = easeFunction.Type;
            _cacheStrength = Strength;
            var step = 0.01f;
            _cacheCurve = new AnimationCurve();
            for (var i = 0f; i <= 1f; i += step)
            {
                var j = easeFunction.Ease(0f, 1f, i, Strength);
                _cacheCurve.AddKey(i, j);
            }
        }

        internal GenericMenu CreateEaseTypeMenu()
        {
            var menu = new GenericMenu();
            foreach (var kv in EaseType.FunctionInfoDic)
            {
                var easeType = kv.Key;
                var easeFunctionAttribute = kv.Value;
                menu.AddItem(easeFunctionAttribute.MenuPath, Ease == easeType, () =>
                {
                    var easeFunction = EaseType.FunctionDic[easeType];
                    EaseProperty.intValue = easeType;
                    StrengthProperty.floatValue = easeFunction.SupportStrength ? easeFunction.DefaultStrength : 1f;
                    EaseProperty.serializedObject.ApplyModifiedProperties();
                });

                if (easeType < 0)
                {
                    menu.AddSeparator("");
                }
            }

            return menu;
        }

        internal GenericMenu CreateCustomCurveMenu()
        {
            var menu = new GenericMenu();
            menu.AddItem("Reset", false, () =>
            {
                CurveProperty.animationCurveValue = new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));
                CurveProperty.serializedObject.ApplyModifiedProperties();
            });

            menu.AddSeparator("");
            menu.AddItem("Reverse", false, () =>
            {
                CurveProperty.animationCurveValue = CurveProperty.animationCurveValue.Reverse();
                CurveProperty.serializedObject.ApplyModifiedProperties();
            });

            return menu;
        }

        public virtual void DrawBody()
        {
        }

        public virtual void DrawEvent()
        {
        }

        public virtual void DrawAppend()
        {
            if (SupportSpace)
            {
                GUIUtil.DrawToolbarEnum(SpaceProperty, nameof(Space), typeof(SpaceMode));
            }
        }

        #region Context Menu

        public void MoveUp()
        {
            if (!CanMoveUp) return;
            Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Move Up");
            var swapTemp = Data.TweenerList[Index - 1];
            Data.TweenerList[Index - 1] = Data.TweenerList[Index];
            Data.TweenerList[Index] = swapTemp;
        }

        public void MoveDown()
        {
            if (!CanMoveDown) return;
            Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Move Down");
            var swapTemp = Data.TweenerList[Index + 1];
            Data.TweenerList[Index + 1] = Data.TweenerList[Index];
            Data.TweenerList[Index] = swapTemp;
        }

        public virtual GenericMenu CreateContextMenu()
        {
            var menu = new GenericMenu();

            // Reset
            menu.AddItem(new GUIContent("Reset"), false, () =>
            {
                Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Reset Tweener");
                Reset();
            });

            // Remove
            menu.AddItem(new GUIContent("Remove Tweener"), false, () =>
            {
                Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Remove Tweener");
                OnRemoved();
                Data.TweenerList.RemoveAt(Index);
            });
            menu.AddSeparator("");

            // Move Up / Move Down
            menu.AddItem(CanMoveUp, "Move Up", false, MoveUp);

            menu.AddItem(CanMoveDown, "Move Down", false, MoveDown);
            menu.AddSeparator("");

            // Copy / Paste
            menu.AddItem("Copy Tweener", false, () =>
            {
                _clipboard = (Tweener)Activator.CreateInstance(GetType());
                EditorUtility.CopySerializedManagedFieldsOnly(this, _clipboard);
            });
            var canPasteAsNew = _clipboard != null;
            menu.AddItem(canPasteAsNew, "Paste Tweener As New", false, () =>
            {
                Undo.RecordObject(SerializedObject.targetObject, "Paste Tweener As New");
                var tweener = (Tweener)Activator.CreateInstance(_clipboard.GetType());
                EditorUtility.CopySerializedManagedFieldsOnly(_clipboard, tweener);
                Data.AddTweener(tweener);
            });
            var canPasteValues = _clipboard != null && _clipboard.GetType() == GetType();
            menu.AddItem(canPasteValues, "Paste Tweener Values", false, () =>
            {
                Undo.RecordObject(SerializedObject.targetObject, "Paste Tweener Values");
                EditorUtility.CopySerializedManagedFieldsOnly(_clipboard, this);
            });
            menu.AddSeparator("");

            // Duration Mode / Event
            menu.AddItem("Switch Duration Mode", false, () =>
            {
                Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Switch Duration Mode");
                if (DurationMode == TweenDurationMode.DurationDelay) DurationMode = TweenDurationMode.FromTo;
                else if (DurationMode == TweenDurationMode.FromTo) DurationMode = TweenDurationMode.DurationDelay;
            });

            // Show / Hide Event
            // if (SupportOnUpdate && Data.Mode == TweenEditorMode.Component)
            // {
            //     menu.AddItem(ShowEvent ? "Hide Callback Event" : "Show Callback Event", false, () =>
            //     {
            //         Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Switch Event");
            //         ShowEvent = !ShowEvent;
            //         if (!ShowEvent) ResetCallback();
            //     });
            // }

            return menu;
        } 

        #endregion
    }

#endif
}
