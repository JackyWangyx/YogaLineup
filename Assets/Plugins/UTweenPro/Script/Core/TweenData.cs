using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class TweenData
    {
        public string Identifier;
        public float Duration;
        public float Delay;

        public PlayMode PlayMode;
        public int PlayCount;
        public AutoPlayMode AutoPlay;
        public UpdateMode UpdateMode;
        public float Interval;
        public float Interval2;
        public TimeMode TimeMode;
        public float SelfScale;
        public PreSampleMode PreSample;
        public bool AutoKill;
        public bool SpeedBased;

        [SerializeReference]
        public List<Tweener> TweenerList = new List<Tweener>();

        public OnPlayEvent OnPlay = new OnPlayEvent();
        public OnStartEvent OnStart = new OnStartEvent();
        public OnLoopStartEvent OnLoopStart = new OnLoopStartEvent();
        public OnLoopEndEvent OnLoopEnd = new OnLoopEndEvent();
        public OnUpdateEvent OnUpdate = new OnUpdateEvent();
        public OnPauseEvent OnPause = new OnPauseEvent();
        public OnResumeEvent OnResume = new OnResumeEvent();
        public OnStopEvent OnStop = new OnStopEvent();
        public OnCompleteEvent OnComplete = new OnCompleteEvent();

        [SerializeField] internal bool FoldOut = true;
        [SerializeField] internal bool FoldOutEvent = false;
        [SerializeField] internal bool EnableIdentifier = false;
        [SerializeField] internal TweenEventType EventType = TweenEventType.OnPlay;

        #region State Property

        public Tweener Tweener => TweenerList.Count > 0 ? TweenerList[0] : default;
        public Tweener FirstTweener => Tweener;
        public Tweener LastTweener => TweenerList.Count > 0 ? TweenerList[TweenerList.Count - 1] : default;

        public PlayState State { get; internal set; }
        public bool IsInitialized { get; internal set; }
        public bool Forward { get; internal set; }
        public bool StartForward { get; internal set; }
        public int LoopCounter { get; internal set; }
        public int FrameCounter { get; internal set; }
        public bool IsDelaying { get; internal set; }
        public bool IsPlaying => State == PlayState.Playing;
        public bool IsInterval { get; internal set; }
        public bool IsCompleted => State == PlayState.Completed;
        public bool IsInProgress => State == PlayState.Playing || State == PlayState.Paused;
        public float CurrentInterval { get; internal set; }
        public bool SingleMode => TweenerList.Count == 1;
        public float DelayTimer { get; internal set; }
        public float PlayTimer { get; internal set; }
        public float IntervalTimer { get; internal set; }
        public float RuntimeDuration { get; internal set; }

        public float Progress
        {
            get => PlayTimer;
            internal set => PlayTimer = value;
        }

        public float NormalizedProgress
        {
            get
            {
                if (Application.isPlaying) return RuntimeNormalizedProgress;
#if UNITY_EDITOR
                if (!Application.isPlaying) return EditorNormalizedProgress;
#endif
                return default;
            }
            internal set
            {
                if (Application.isPlaying) RuntimeNormalizedProgress = value;
#if UNITY_EDITOR
                if (!Application.isPlaying) EditorNormalizedProgress = value;
#endif
            }
        }

        public float RuntimeNormalizedProgress { get; internal set; }
        public UTweenAnimation TweenAnimation { get; internal set; }
        public TweenControlMode ControlMode { get; internal set; }

        #endregion

        #region Mono Behaviour

        public virtual void Awake()
        {
            if (AutoPlay != AutoPlayMode.Awake)
            {
                if (PreSample == PreSampleMode.Awake) PreSampleImpl();
                return;
            }

            Play();
        }

        public virtual void OnEnable()
        {
            if (AutoPlay != AutoPlayMode.Enable)
            {
                if (PreSample == PreSampleMode.Enable) PreSampleImpl();
                return;
            }

            Play();
        }

        public virtual void Start()
        {
            if (AutoPlay != AutoPlayMode.Start)
            {
                if (PreSample == PreSampleMode.Start) PreSampleImpl();
                return;
            }

            Play();
        }

        public virtual void OnDisable()
        {
            if (IsPlaying)
            {
                Stop();
            }
        }

        #endregion

        #region Play / Pasue / Resume / Stop

        public TweenData Play(bool forward = true)
        {
            if (State == PlayState.Playing) return this;
            if (State != PlayState.Paused)
            {
                IsInitialized = false;
                StartForward = forward;
                foreach (var tweener in TweenerList)
                {
                    tweener.IsPrepared = false;
                }

                if (Application.isPlaying)
                {
                    UTweenManager.Ins.AddTweenData(this);
                }
                else
                {
#if UNITY_EDITOR
                    TweenAnimation.PreviewStart();
#endif
                }
            }

            State = PlayState.Playing;
            return this;
        }

        public TweenData Pause()
        {
            if (State != PlayState.Playing) return this;
            State = PlayState.Paused;
            OnPause.Invoke();
            return this;
        }

        public TweenData Resume()
        {
            if (State != PlayState.Paused) return this;
            State = PlayState.Playing;
            OnResume.Invoke();
            return this;
        }

        public TweenData Stop()
        {
            if (State != PlayState.Completed)
            {
                State = PlayState.Stopped;
            }

            RuntimeNormalizedProgress = 0f;
#if UNITY_EDITOR
            EditorNormalizedProgress = 0f;
#endif
            OnStop.Invoke();

            if (Application.isPlaying)
            {
                UTweenManager.Ins?.RemoveTweenData(this);
            }
            else
            {
#if UNITY_EDITOR
                TweenAnimation.PreviewEnd();
#endif
            }

            return this;
        }

        public TweenData PlayForward()
        {
            Play(true);
            return this;
        }

        public TweenData PlayBackward()
        {
            Play(false);
            return this;
        }

        #endregion

        #region Add / Remove Tweener

        public void AddTweener(Tweener tweener)
        {
            if (TweenerList.Contains(tweener)) return;
            if (tweener.Data != null)
            {
                tweener.Data.RemoveTweener(tweener);
                tweener.Data = null;
            }

            TweenerList.Add(tweener);
            tweener.Data = this;
        }

        public void RemoveTweener(Tweener tweener)
        {
            if (!TweenerList.Contains(tweener)) return;
            TweenerList.Remove(tweener);
            tweener.Data = null;
        }

        #endregion

        #region Initialize / Update / Sample

        internal void Initialize()
        {
            FrameCounter = 0;
            DelayTimer = 0f;
            PlayTimer = 0f;
            Forward = StartForward;
            LoopCounter = 0;

            if (SingleMode && SpeedBased)
            {
                RuntimeDuration = Tweener.GetSpeedBasedDuration();
            }
            else
            {
                RuntimeDuration = Duration;
            }

            foreach (var tweener in TweenerList)
            {
                tweener.PreSample();
            }

            if (Delay > 0f)
            {
                IsDelaying = true;
            }

            State = PlayState.Playing;
            IsInitialized = true;
        }

        internal void UpdateInternal(float scaledDeltaTime, float unscaledDeltaTime, float smoothDeltaTime)
        {
            var deltaTime = 0f;
            if (TimeMode == TimeMode.Normal) deltaTime = scaledDeltaTime;
            else if (TimeMode == TimeMode.UnScaled) deltaTime = unscaledDeltaTime;
            else if (TimeMode == TimeMode.Smooth) deltaTime = smoothDeltaTime;
            deltaTime *= SelfScale;
            Update(deltaTime);
        }

        public void Update(float deltaTime)
        {
            if (!IsInitialized)
            {
                Initialize();
            }

            if (FrameCounter == 0)
            {
                OnPlay.Invoke();
                if (PlayMode != PlayMode.Once)
                {
                    OnLoopStart.Invoke();
                }
            }

            if (IsDelaying)
            {
                DelayTimer += deltaTime;
                PlayTimer = 0f;
                if (DelayTimer >= Delay)
                {
                    IsDelaying = false;
                }
            }
            else if (IsInterval)
            {
                IntervalTimer += deltaTime;
                if (IntervalTimer >= CurrentInterval)
                {
                    IsInterval = false;
                    OnLoopStart.Invoke();
                }
            }
            else if (State == PlayState.Playing)
            {
                if (FrameCounter == 0)
                {
                    OnStart.Invoke();
                }

                PlayTimer += deltaTime;
                FrameCounter++;
            }

            if (PlayTimer < RuntimeDuration)
            {
                RuntimeNormalizedProgress = Forward ? PlayTimer / RuntimeDuration : (RuntimeDuration - PlayTimer) / RuntimeDuration;
                Sample(RuntimeNormalizedProgress);
                // TODO.. 3 ms
                OnUpdate.Invoke();
            }
            else
            {
                if (PlayMode == PlayMode.Once)
                {
                    RuntimeNormalizedProgress = 1f;
                    Sample(RuntimeNormalizedProgress);
                    Complete();
                    Stop();
                }
                else if (PlayMode == PlayMode.Loop)
                {
                    LoopCounter++;
                    PlayTimer = 0f;
                    if (LoopCounter >= PlayCount && PlayCount > 0)
                    {
                        OnLoopEnd.Invoke();
                        RuntimeNormalizedProgress = 1f;
                        Sample(RuntimeNormalizedProgress);
                        Complete();
                        Stop();
                    }
                    else
                    {
                        if (Interval > 0)
                        {
                            IsInterval = true;
                            IntervalTimer = 0f;
                            CurrentInterval = Interval;
                        }
                        else
                        {
                            OnLoopStart.Invoke();
                        }
                    }
                }
                else if (PlayMode == PlayMode.PingPong)
                {
                    PlayTimer = 0f;
                    Forward = !Forward;
                    if (Forward == StartForward) LoopCounter++;
                    if (LoopCounter >= PlayCount && PlayCount > 0)
                    {
                        OnLoopEnd.Invoke();
                        RuntimeNormalizedProgress = StartForward ? 0f : 1f;
                        Sample(RuntimeNormalizedProgress);
                        Complete();
                        Stop();
                    }
                    else
                    {
                        CurrentInterval = Forward == StartForward ? Interval : Interval2;
                        if (CurrentInterval > 0)
                        {
                            IsInterval = true;
                            IntervalTimer = 0f;
                        }
                        else
                        {
                            OnLoopStart.Invoke();
                        }
                    }
                }
            }
        }

        public void Sample(float normalizedDuration)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (Mode == TweenEditorMode.Component && !PreviewSampled)
                {
                    PreviewSampled = true;
                    RecordObject();
                }

                if (!IsPlaying)
                {
                    foreach (var tweener in TweenerList)
                    {
                        tweener.PreSample();
                    }
                }
            }
#endif

            foreach (var tweener in TweenerList)
            {
                if (tweener.Data == null) tweener.Data = this;
                if (!tweener.Active) continue;
                // TODO.. 1 ms
                var factor = tweener.GetFactor(normalizedDuration);
                if (float.IsNaN(factor)) continue;
                // TODO.. 7 ms
                tweener.Sample(factor);
            }

#if UNITY_EDITOR
            foreach (var tweener in TweenerList)
            {
                tweener.SetDirty();
            }
#endif
        }

        internal void PreSampleImpl()
        {
            foreach (var tweener in TweenerList)
            {
                tweener.Data = this;
                tweener.PreSample();
            }

            Sample(0f);
        }

        internal void Complete()
        {
            State = PlayState.Completed;
            OnComplete.Invoke();
        }

        #endregion

        #region Record / Restore

        public void RecordObject()
        {
            foreach (var tweener in TweenerList)
            {
                try
                {
                    tweener.RecordObject();
                }
                catch
                {
                    //
                }
            }
        }

        public void RestoreObject()
        {
            foreach (var tweener in TweenerList)
            {
                try
                {
                    tweener.RestoreObject();
                }
                catch
                {
                    //
                }
            }
        }

        #endregion

        #region Reset / DeSpawn

        public void Reset()
        {
            IsInitialized = false;
            Duration = 1f;
            Delay = 0f;
            PlayMode = PlayMode.Once;
            PlayCount = 1;
            AutoPlay = AutoPlayMode.None;
            UpdateMode = UpdateMode.Update;
            Interval = 0f;
            Interval2 = 0f;
            TimeMode = TimeMode.Normal;
            SelfScale = 1f;
            PreSample = PreSampleMode.Enable;
            AutoKill = false;
            SpeedBased = false;

            foreach (var tweener in TweenerList)
            {
                tweener.Reset();
            }

            ResetCallback();
        }

        public virtual void ResetCallback()
        {
            OnPlay.Reset();
            OnLoopStart.Reset();
            OnLoopEnd.Reset();
            OnUpdate.Reset();
            OnPause.Reset();
            OnResume.Reset();
            OnStop.Reset();
            OnComplete.Reset();
        }

        internal void DeSpawn()
        {
            foreach (var tweener in TweenerList)
            {
                tweener.StopSample();
            }

            if (ControlMode == TweenControlMode.Component)
            {
                if (Application.isPlaying && AutoKill) Object.Destroy(TweenAnimation.gameObject);
                return;
            }

            foreach (var tweener in TweenerList)
            {
                Pool.DeSpawn(tweener);
            }

            TweenerList.Clear();
            Pool.DeSpawn(this);
        }

        #endregion

#if UNITY_EDITOR

        public void OnDrawGizmos()
        {
            foreach (var tweener in TweenerList)
            {
                if (!tweener.Active || !tweener.FoldOut) continue;
                tweener.OnDrawGizmos();
            }
        }

#endif
    }

#if UNITY_EDITOR
    public partial class TweenData
    {
        [NonSerialized] public TweenEditorMode Mode;
        [NonSerialized] public Editor Editor;

        [NonSerialized] public SerializedProperty IdentifierProperty;
        [NonSerialized] public SerializedProperty TweenDataProperty;
        [NonSerialized] public SerializedProperty TweenerListProperty;
        [NonSerialized] public SerializedProperty DurationProperty;
        [NonSerialized] public SerializedProperty DelayProperty;
        [NonSerialized] public SerializedProperty PlayModeProperty;
        [NonSerialized] public SerializedProperty PlayCountProperty;
        [NonSerialized] public SerializedProperty AutoPlayProperty;
        [NonSerialized] public SerializedProperty UpdateModeProperty;
        [NonSerialized] public SerializedProperty IntervalProperty;
        [NonSerialized] public SerializedProperty Interval2Property;
        [NonSerialized] public SerializedProperty TimeModeProperty;
        [NonSerialized] public SerializedProperty SelfScaleProperty;
        [NonSerialized] public SerializedProperty PreSampleProperty;
        [NonSerialized] public SerializedProperty AutoKillProperty;
        [NonSerialized] public SerializedProperty SpeedBasedProperty;

        [NonSerialized] public SerializedProperty OnPlayProperty;
        [NonSerialized] public SerializedProperty OnStartProperty;
        [NonSerialized] public SerializedProperty OnLoopStartProperty;
        [NonSerialized] public SerializedProperty OnLoopEndProperty;
        [NonSerialized] public SerializedProperty OnUpdateProperty;
        [NonSerialized] public SerializedProperty OnPauseProperty;
        [NonSerialized] public SerializedProperty OnResumeProperty;
        [NonSerialized] public SerializedProperty OnStopProperty;
        [NonSerialized] public SerializedProperty OnCompleteProperty;

        [NonSerialized] internal SerializedProperty FoldOutProperty;
        [NonSerialized] internal SerializedProperty FoldOutEventProperty;
        [NonSerialized] internal SerializedProperty EnableIdentifierProperty;
        [NonSerialized] internal SerializedProperty EventTypeProperty;

        [NonSerialized] public float EditorNormalizedProgress;
        [NonSerialized] public bool PreviewSampled = false;

        public Object EditorTarget => Editor.target;
        public MonoBehaviour MonoBehaviour => EditorTarget as MonoBehaviour;
        public GameObject GameObject => EditorTarget as GameObject;
        public SerializedObject SerializedObject => Editor.serializedObject;

        public void InitEditor(TweenEditorMode mode, Editor editor)
        {
            Mode = mode;
            Editor = editor;
            PreviewSampled = false;

            TweenDataProperty = SerializedObject.FindProperty("Data");
            IdentifierProperty = TweenDataProperty.FindPropertyRelative(nameof(Identifier));
            TweenerListProperty = TweenDataProperty.FindPropertyRelative(nameof(TweenerList));
            DurationProperty = TweenDataProperty.FindPropertyRelative(nameof(Duration));
            DelayProperty = TweenDataProperty.FindPropertyRelative(nameof(Delay));
            PlayModeProperty = TweenDataProperty.FindPropertyRelative(nameof(PlayMode));
            PlayCountProperty = TweenDataProperty.FindPropertyRelative(nameof(PlayCount));
            AutoPlayProperty = TweenDataProperty.FindPropertyRelative(nameof(AutoPlay));
            UpdateModeProperty = TweenDataProperty.FindPropertyRelative(nameof(UpdateMode));
            IntervalProperty = TweenDataProperty.FindPropertyRelative(nameof(Interval));
            Interval2Property = TweenDataProperty.FindPropertyRelative(nameof(Interval2));
            TimeModeProperty = TweenDataProperty.FindPropertyRelative(nameof(TimeMode));
            SelfScaleProperty = TweenDataProperty.FindPropertyRelative(nameof(SelfScale));
            PreSampleProperty = TweenDataProperty.FindPropertyRelative(nameof(PreSample));
            AutoKillProperty = TweenDataProperty.FindPropertyRelative(nameof(AutoKill));
            SpeedBasedProperty = TweenDataProperty.FindPropertyRelative(nameof(SpeedBased));

            OnPlayProperty = TweenDataProperty.FindPropertyRelative(nameof(OnPlay));
            OnStartProperty = TweenDataProperty.FindPropertyRelative(nameof(OnStart));
            OnUpdateProperty = TweenDataProperty.FindPropertyRelative(nameof(OnUpdate));
            OnLoopStartProperty = TweenDataProperty.FindPropertyRelative(nameof(OnLoopStart));
            OnLoopEndProperty = TweenDataProperty.FindPropertyRelative(nameof(OnLoopEnd));
            OnPauseProperty = TweenDataProperty.FindPropertyRelative(nameof(OnPause));
            OnResumeProperty = TweenDataProperty.FindPropertyRelative(nameof(OnResume));
            OnStopProperty = TweenDataProperty.FindPropertyRelative(nameof(OnStop));
            OnCompleteProperty = TweenDataProperty.FindPropertyRelative(nameof(OnComplete));

            FoldOutProperty = TweenDataProperty.FindPropertyRelative(nameof(FoldOut));
            FoldOutEventProperty = TweenDataProperty.FindPropertyRelative(nameof(FoldOutEvent));
            EnableIdentifierProperty = TweenDataProperty.FindPropertyRelative(nameof(EnableIdentifier));
            EventTypeProperty = TweenDataProperty.FindPropertyRelative(nameof(EventType));

            foreach (var tweener in TweenerList)
            {
                tweener.Index = -1;
            }

            if (Mode == TweenEditorMode.Component)
            {
                RecordObject();
            }
        }

        public virtual void OnInspectorGUI()
        {
            using (GUIWideMode.Create(true))
            {
                using (GUILabelWidthArea.Create(EditorStyle.LabelWidth))
                {
                    DrawProgressBar();
                    DrawTweenData();

                    if (SerializedObject.isEditingMultipleObjects)
                    {
                        SerializedObject.ApplyModifiedProperties();
                        return;
                    }

                    if (TweenerList.Count > 0)
                    {
                        DrawTweenerList();
                    }
                    else
                    {
                        GUIUtil.DrawTipArea(EditorStyle.ErrorColor, "No Tweener");
                    }

                    using (GUIEnableArea.Create(!IsInProgress))
                    {
                        DrawAppend();
                    }

                    DrawEvent();
                }
            }
        }

        public virtual void DrawProgressBar()
        {
            if (Mode == TweenEditorMode.ScriptableObject) return;
            using (GUIGroup.Create())
            {
                using (GUIHorizontal.Create())
                {
                    var progressBarHeight = EditorGUIUtility.singleLineHeight;
                    var isInProgress = IsInProgress;

                    using (GUIColorArea.Create(EditorStyle.ProgressInRangeColor, isInProgress))
                    {
                        var btnContent = isInProgress ? EditorStyle.PlayButtonOn : EditorStyle.PlayButton;
                        var btnPlay = GUILayout.Button(btnContent, EditorStyles.miniButtonMid, GUILayout.Width(EditorGUIUtility.singleLineHeight));
                        if (btnPlay)
                        {
                            if (!isInProgress)
                            {
                                RecordObject();
                                ControlMode = TweenControlMode.Component;
                                State = PlayState.None;
                                Play();
                            }
                            else
                            {
                                Stop();
                                RestoreObject();
                            }
                        }
                    }

                    if (isInProgress)
                    {
                        EditorNormalizedProgress = RuntimeNormalizedProgress;
                    }

                    GUIUtil.DrawDraggableProgressBar(SerializedObject.targetObject, progressBarHeight, EditorNormalizedProgress,
                        value =>
                        {
                            if (isInProgress) return;
                            if (!IsInitialized) Initialize();
                            EditorNormalizedProgress = value;
                            try
                            {
                                Sample(EditorNormalizedProgress);
                            }
                            catch
                            {
                                //
                            }
                        });

                    if (!string.IsNullOrEmpty(Identifier))
                    {
                        var rect = GUILayoutUtility.GetLastRect();
                        GUI.Label(rect, Identifier, EditorStyles.centeredGreyMiniLabel);
                    }
                }
            }
        }

        #region Draw TweenData

        private float _originalDuration;
        private bool _durationChanged;

        public void DrawTweenData()
        {
            _originalDuration = DurationProperty.floatValue;
            _durationChanged = false;

            using (GUIGroup.Create())
            {
                // Header
                using (GUIHorizontal.Create())
                {
                    FoldOut = EditorGUILayout.Toggle(GUIContent.none, FoldOut, EditorStyles.foldout, GUILayout.Width(EditorStyle.CharacterWidth));
                    var btnTitle = GUILayout.Button("Animation", EditorStyles.boldLabel);

                    var btnFlexibleSpace = GUILayout.Button(GUIContent.none, EditorStyles.label);
                    if (btnTitle || btnFlexibleSpace)
                    {
                        FoldOutProperty.boolValue = !FoldOutProperty.boolValue;
                    }

                    using (GUIEnableArea.Create(!IsInProgress))
                    {
                        var btnContextMenu = GUILayout.Button(GUIContent.none, EditorStyles.foldoutHeaderIcon, GUILayout.Width(EditorStyle.CharacterWidth));
                        if (btnContextMenu)
                        {
                            var menu = CreateContextMenu();
                            menu.ShowAsContext();
                        }
                    }
                }

                if (!FoldOut) return;

                using (GUIEnableArea.Create(!IsInProgress))
                {
                    // ID
                    if (EnableIdentifier)
                    {
                        EditorGUILayout.PropertyField(IdentifierProperty, new GUIContent("ID"));
                    }

                    using (GUIHorizontal.Create())
                    {
                        using (var check = GUICheckChangeArea.Create())
                        {
                            var durationName = nameof(Duration);
                            if (SpeedBased) durationName = "Speed";
                            EditorGUILayout.PropertyField(DurationProperty, new GUIContent(durationName));
                            if (DurationProperty.floatValue < 0) DurationProperty.floatValue = 0f;

                            if (check.Changed)
                            {
                                _durationChanged = true;
                            }
                        }

                        EditorGUILayout.PropertyField(DelayProperty, new GUIContent(nameof(Delay)));
                        if (DelayProperty.floatValue < 0) DelayProperty.floatValue = 0;
                    }

                    using (GUIHorizontal.Create())
                    {
                        EditorGUILayout.PropertyField(PlayModeProperty, new GUIContent("Play"));
                        using (GUIEnableArea.Create(PlayMode != PlayMode.Once && GUI.enabled))
                        {
                            EditorGUILayout.PropertyField(PlayCountProperty, new GUIContent("Count"));
                            if (PlayMode == PlayMode.Once)
                            {
                                PlayCountProperty.intValue = 1;
                            }

                            if (PlayCount < -1) PlayCountProperty.intValue = -1;
                        }
                    }

                    using (GUIHorizontal.Create())
                    {
                        EditorGUILayout.PropertyField(UpdateModeProperty, new GUIContent("Update"));

                        if (PlayMode == PlayMode.PingPong)
                        {
                            var rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight, EditorStyles.label);
                            var width = rect.width;
                            rect.width = EditorStyle.LabelWidth;
                            GUI.Label(rect, nameof(Interval), EditorStyles.label);
                            rect.x += EditorStyle.LabelWidth + 2f;
                            rect.width = (width - rect.width - 6f) / 2f;
                            IntervalProperty.floatValue = EditorGUI.FloatField(rect, IntervalProperty.floatValue);
                            if (Interval < 0f) IntervalProperty.floatValue = 0f;
                            rect.x += rect.width + 3f;
                            Interval2Property.floatValue = EditorGUI.FloatField(rect, Interval2Property.floatValue);
                            if (Interval2 < 0f) IntervalProperty.floatValue = 0f;
                        }
                        else
                        {
                            using (GUIEnableArea.Create(PlayMode != PlayMode.Once))
                            {
                                EditorGUILayout.PropertyField(IntervalProperty);
                            }
                        }
                    }

                    using (GUIHorizontal.Create())
                    {
                        EditorGUILayout.PropertyField(AutoPlayProperty, new GUIContent("Auto"));
                        EditorGUILayout.PropertyField(PreSampleProperty, new GUIContent("Sample"));
                    }

                    using (GUIHorizontal.Create())
                    {
                        EditorGUILayout.PropertyField(TimeModeProperty, new GUIContent("Time"));
                        EditorGUILayout.PropertyField(SelfScaleProperty, new GUIContent("Scale"));
                    }

                    using (GUIHorizontal.Create())
                    {
                        using (GUIEnableArea.Create(SingleMode && GUI.enabled))
                        {
                            GUIUtil.ToggleButton(SpeedBasedProperty);
                            if (!SingleMode)
                            {
                                SpeedBasedProperty.boolValue = false;
                            }
                        }

                        GUIUtil.ToggleButton(AutoKillProperty);
                    }
                }
            }
        }

        #endregion

        public virtual void DrawTweenerList()
        {
            for (var i = 0; i < TweenerList.Count; i++)
            {
                var tweener = TweenerList[i];
                if (tweener.Index != i || tweener.TweenerProperty == null)
                {
                    var tweenerProperty = TweenerListProperty.GetArrayElementAtIndex(i);
                    tweener.InitEditor(i, this, tweenerProperty);
                }

                // Sync tweeners duration and delay
                if (_durationChanged)
                {
                    var durationChangeRate = DurationProperty.floatValue / _originalDuration;
                    tweener.DelayProperty.floatValue *= durationChangeRate;
                    tweener.DurationProperty.floatValue *= durationChangeRate;
                }

                tweener.DrawTweener();
            }
        }

        #region Draw Event

        public virtual void DrawEvent()
        {
            if (Mode != TweenEditorMode.Component) return;
            using (GUIFoldOut.Create(EditorTarget, "Event", ref FoldOutEvent))
            {
                if (!FoldOutEvent) return;
                using (GUIEnableArea.Create(!IsInProgress))
                {
                    using (GUIGroup.Create())
                    {
                        var btnStyle = EditorStyles.toolbarButton;
                        using (GUIHorizontal.Create())
                        {
                            using (GUIVertical.Create())
                            {
                                if (GUILayout.Toggle(EventType == TweenEventType.OnPlay, nameof(TweenEventType.OnPlay), btnStyle))
                                {
                                    OnPlay.InitEditor(TweenDataProperty, nameof(OnPlay));
                                    EventType = TweenEventType.OnPlay;
                                }

                                if (GUILayout.Toggle(EventType == TweenEventType.OnLoopStart, nameof(TweenEventType.OnLoopStart), btnStyle))
                                {
                                    OnLoopStart.InitEditor(TweenDataProperty, nameof(OnLoopStart));
                                    EventType = TweenEventType.OnLoopStart;
                                }

                                if (GUILayout.Toggle(EventType == TweenEventType.OnResume, nameof(TweenEventType.OnResume), btnStyle))
                                {
                                    OnResume.InitEditor(TweenDataProperty, nameof(OnResume));
                                    EventType = TweenEventType.OnResume;
                                }
                            }

                            using (GUIVertical.Create())
                            {
                                if (GUILayout.Toggle(EventType == TweenEventType.OnStart, nameof(TweenEventType.OnStart), btnStyle))
                                {
                                    OnStart.InitEditor(TweenDataProperty, nameof(OnStart));
                                    EventType = TweenEventType.OnStart;
                                }

                                if (GUILayout.Toggle(EventType == TweenEventType.OnLoopEnd, nameof(TweenEventType.OnLoopEnd), btnStyle))
                                {
                                    OnLoopEnd.InitEditor(TweenDataProperty, nameof(OnLoopEnd));
                                    EventType = TweenEventType.OnLoopEnd;
                                }

                                if (GUILayout.Toggle(EventType == TweenEventType.OnStop, nameof(TweenEventType.OnStop), btnStyle))
                                {
                                    OnStop.InitEditor(TweenDataProperty, nameof(OnStop));
                                    EventType = TweenEventType.OnStop;
                                }
                            }

                            using (GUIVertical.Create())
                            {
                                if (GUILayout.Toggle(EventType == TweenEventType.OnPause, nameof(TweenEventType.OnPause), btnStyle))
                                {
                                    OnPause.InitEditor(TweenDataProperty, nameof(OnPause));
                                    EventType = TweenEventType.OnPause;
                                }

                                if (GUILayout.Toggle(EventType == TweenEventType.OnUpdate, nameof(TweenEventType.OnUpdate), btnStyle))
                                {
                                    OnUpdate.InitEditor(TweenDataProperty, nameof(OnUpdate));
                                    EventType = TweenEventType.OnUpdate;
                                }

                                if (GUILayout.Toggle(EventType == TweenEventType.OnComplete, nameof(TweenEventType.OnComplete), btnStyle))
                                {
                                    OnComplete.InitEditor(TweenDataProperty, nameof(OnComplete));
                                    EventType = TweenEventType.OnComplete;
                                }
                            }
                        }
                    }

                    switch (EventType)
                    {
                        case TweenEventType.OnPlay:
                            OnPlay.DrawEvent(nameof(OnPlay));
                            break;
                        case TweenEventType.OnStart:
                            OnStart.DrawEvent(nameof(OnStart));
                            break;
                        case TweenEventType.OnUpdate:
                            OnUpdate.DrawEvent(nameof(OnUpdate));
                            break;
                        case TweenEventType.OnLoopStart:
                            OnLoopStart.DrawEvent(nameof(OnLoopStart));
                            break;
                        case TweenEventType.OnLoopEnd:
                            OnLoopEnd.DrawEvent(nameof(OnLoopEnd));
                            break;
                        case TweenEventType.OnPause:
                            OnPause.DrawEvent(nameof(OnPause));
                            break;
                        case TweenEventType.OnResume:
                            OnResume.DrawEvent(nameof(OnResume));
                            break;
                        case TweenEventType.OnStop:
                            OnStop.DrawEvent(nameof(OnStop));
                            break;
                        case TweenEventType.OnComplete:
                            OnComplete.DrawEvent(nameof(OnComplete));
                            break;
                    }
                }
            }
        }

        #endregion

        public virtual void DrawAppend()
        {
            DrawAddTweener();
        }

        #region Draw Add Tweener

        public virtual void DrawAddTweener()
        {
            using (GUIGroup.Create())
            {
                var buttonRect = EditorGUILayout.GetControlRect();
                var btnAddTweener = GUI.Button(buttonRect, "Add Tweener");
                if (btnAddTweener)
                {
                    var tweenTypeDic = TypeCaches.TweenerTypeDic;
                    var root = new SearchableDropdownItem($"Tweener ({tweenTypeDic.Count})");
                    var menu = new SearchableDropdown(root, item =>
                    {
                        var tweenerType = item.Value as Type;
                        if (tweenerType == null) return;
                        var tweener = Activator.CreateInstance(tweenerType) as Tweener;
                        if (tweener == null) return;
                        Undo.RecordObject(SerializedObject.targetObject, "Add Tweener");
                        tweener.Reset();
                        if (Mode == TweenEditorMode.Component)
                        {
                            tweener.InitParam(this, MonoBehaviour);
                        }
                        else
                        {
                            tweener.InitParam(this, null);
                        }

                        TweenerList.Add(tweener);
                        tweener.OnAdded();
                    });

                    foreach (var kv in EditorIcon.TweenerGroupIconDic)
                    {
                        var group = kv.Key;
                        var groupItem = new SearchableDropdownItem(group)
                        {
                            icon = kv.Value
                        };

                        root.AddChild(groupItem);
                    }

                    foreach (var kv in tweenTypeDic)
                    {
                        var tweenerType = kv.Key;
                        var tweenerAttribute = kv.Value;
                        var group = tweenerAttribute.Group;
                        var name = tweenerAttribute.DisplayName;
                        SearchableDropdownItem groupItem = null;
                        foreach (var child in root.children)
                        {
                            if (child.name != group) continue;
                            groupItem = child as SearchableDropdownItem;
                            break;
                        }

                        if (groupItem == null)
                        {
                            groupItem = new SearchableDropdownItem(group);
                            root.AddChild(groupItem);
                        }

                        var item = new SearchableDropdownItem(name, tweenerType)
                        {
                            icon = EditorIcon.GetTweenerIcon(tweenerType)
                        };

                        groupItem.AddChild(item);
                    }

                    menu.Show(buttonRect);
                }
            }
        }

        public virtual GenericMenu CreateContextMenu()
        {
            var menu = new GenericMenu();

            // Identifier
            menu.AddItem(EnableIdentifier ? "Disable Identifier" : "Enable Identifier", false, () =>
            {
                Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Switch Identifier");
                EnableIdentifier = !EnableIdentifier;
                if (!EnableIdentifier)
                {
                    Identifier = null;
                }
            });

            return menu;
        }

        #endregion
    }

#endif
}
