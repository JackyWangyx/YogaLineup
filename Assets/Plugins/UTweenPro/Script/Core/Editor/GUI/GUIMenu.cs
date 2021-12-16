#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Aya.TweenPro
{
    internal static class GUIMenu
    {
        #region Property Menu

        public static void DrawPropertyMenu<TValue>(object target, string propertyName, SerializedProperty property)
        {
            using (GUIHorizontal.Create())
            {
                GUILayout.Label(propertyName, EditorStyles.label, GUILayout.Width(EditorGUIUtility.labelWidth));
                var displayName = string.IsNullOrEmpty(property.stringValue) ? EditorStyle.NoneStr : property.stringValue;
                var btn = GUILayout.Button(displayName, EditorStyles.popup);
                if (btn)
                {
                    var menu = CreatePropertyMenu<TValue>(target, property.stringValue, (propertyInfo, fieldInfo) =>
                    {
                        var name = propertyInfo != null ? propertyInfo.Name : (fieldInfo != null ? fieldInfo.Name : "");
                        property.stringValue = name;
                        property.serializedObject.ApplyModifiedProperties();
                    });

                    menu.ShowAsContext();
                }
            }
        }

        public static GenericMenu CreatePropertyMenu<TValue>(object target, string currentProperty, Action<PropertyInfo, FieldInfo> onClick)
        {
            var menu = new GenericMenu();
            menu.AddItem(EditorStyle.NoneStr, string.IsNullOrEmpty(currentProperty), () => { onClick?.Invoke(null, null); });
            menu.AddSeparator("");

            var exist = false;
            var targetType = target.GetType();
            var propertyType = typeof(TValue);
            var flags = TypeCaches.DefaultBindingFlags;
            var properties = targetType.GetProperties(flags);
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.PropertyType != propertyType) continue;
                var selected = currentProperty == propertyInfo.Name;
                if (selected) exist = true;
                menu.AddItem("Property/" + propertyInfo.Name, selected, () => { onClick?.Invoke(propertyInfo, null); });
            }

            var fields = targetType.GetFields(flags);
            foreach (var fieldInfo in fields)
            {
                if (fieldInfo.FieldType != propertyType) continue;
                var selected = currentProperty == fieldInfo.Name;
                if (selected) exist = true;
                menu.AddItem("Field/" + fieldInfo.Name, selected, () => { onClick?.Invoke(null, fieldInfo); });
            }

            if (!exist)
            {
                onClick?.Invoke(null, null);
            }

            return menu;
        }

        #endregion

        #region Animation Clip Menu

        public static void SelectAnimationClipMenu(Animation animation, string propertyName, SerializedProperty clipProperty)
        {
            if (animation != null && string.IsNullOrEmpty(clipProperty.stringValue) && animation.GetClipCount() > 0)
            {
                clipProperty.stringValue = AnimationUtility.GetAnimationClips(animation.gameObject)[0].name;
            }

            using (GUIErrorColorArea.Create(string.IsNullOrEmpty(clipProperty.stringValue)))
            {
                using (GUIHorizontal.Create())
                {
                    GUILayout.Label(propertyName, GUILayout.Width(EditorStyle.LabelWidth));
                    var showName = string.IsNullOrEmpty(clipProperty.stringValue) ? EditorStyle.NoneStr : clipProperty.stringValue;
                    var button = GUILayout.Button(showName, EditorStyles.popup);
                    if (button)
                    {
                        var menu = new GenericMenu();
                        menu.AddItem(EditorStyle.NoneStr, string.IsNullOrEmpty(clipProperty.stringValue), () =>
                        {
                            clipProperty.stringValue = null;
                            clipProperty.serializedObject.ApplyModifiedProperties();
                        });

                        if (animation != null)
                        {
                            menu.AddSeparator("");
                            var clips = AnimationUtility.GetAnimationClips(animation.gameObject);
                            foreach (var clip in clips)
                            {
                                var clipName = clip.name;
                                menu.AddItem(clipName, clipProperty.stringValue == clipName, () =>
                                {
                                    clipProperty.stringValue = clipName;
                                    clipProperty.serializedObject.ApplyModifiedProperties();
                                });
                            }
                        }

                        menu.ShowAsContext();
                    }
                }
            }
        }

        #endregion

        #region Animator State Menu

        public static void SelectAnimatorStateMenu(Animator animator, string propertyName, SerializedProperty stateProperty)
        {
            using (GUIErrorColorArea.Create(string.IsNullOrEmpty(stateProperty.stringValue)))
            {
                using (GUIHorizontal.Create())
                {
                    GUILayout.Label(propertyName, GUILayout.Width(EditorStyle.LabelWidth));
                    var showName = string.IsNullOrEmpty(stateProperty.stringValue) ? EditorStyle.NoneStr : stateProperty.stringValue;
                    var button = GUILayout.Button(showName, EditorStyles.popup);
                    if (button)
                    {
                        var controller = animator.runtimeAnimatorController;
                        var menu = new GenericMenu();
                        menu.AddItem(EditorStyle.NoneStr, string.IsNullOrEmpty(stateProperty.stringValue), () =>
                        {
                            stateProperty.stringValue = null;
                            stateProperty.serializedObject.ApplyModifiedProperties();
                        });

                        if (controller != null)
                        {
                            menu.AddSeparator("");
                            var clips = controller.animationClips;
                            foreach (var clip in clips)
                            {
                                var clipName = clip.name;
                                menu.AddItem(clipName, stateProperty.stringValue == clipName, () =>
                                {
                                    stateProperty.stringValue = clipName;
                                    stateProperty.serializedObject.ApplyModifiedProperties();
                                });
                            }
                        }
                        
                        menu.ShowAsContext();
                    }
                }
            }
        }

        #endregion

        #region Material Shader Menu

        public static void SelectMaterialMenu(Renderer renderer, string propertyName, SerializedProperty materialIndexProperty)
        {
            string displayName;
            var material = renderer.GetMaterial(materialIndexProperty.intValue);
            if (material == null)
            {
                materialIndexProperty.intValue = -1;
                displayName = EditorStyle.NoneStr;
            }
            else
            {
                displayName = material.name;
            }

            using (GUIHorizontal.Create())
            {
                GUILayout.Label(propertyName, EditorStyles.label, GUILayout.Width(EditorGUIUtility.labelWidth));
                var btn = GUILayout.Button(displayName, EditorStyles.popup);
                if (btn)
                {
                    var menu = CreateMaterialMenu(renderer, materialIndexProperty);
                    menu.ShowAsContext();
                }
            }
        }

        internal static GenericMenu CreateMaterialMenu(Renderer render, SerializedProperty property)
        {
            var menu = new GenericMenu();
            menu.AddItem(EditorStyle.NoneStr, property.intValue < 0, () =>
            {
                property.intValue = -1;
                property.serializedObject.ApplyModifiedProperties();
            });

            if (render == null) return menu;

            menu.AddSeparator("");
            for (var i = 0; i < render.sharedMaterials.Length; i++)
            {
                var material = render.sharedMaterials[i];
                var index = i;
                menu.AddItem(material.name, property.intValue == index, () =>
                {
                    property.intValue = index;
                    property.serializedObject.ApplyModifiedProperties();
                });
            }

            return menu;
        }

        public static void SelectMaterialShaderMenu(Renderer render, string propertyName, int materialIndex, SerializedProperty propertyNameProperty, ShaderUtil.ShaderPropertyType propertyType)
        {
            string displayName;
            var material = render.GetMaterial(materialIndex);
            if (material == null || string.IsNullOrEmpty(propertyNameProperty.stringValue) || !material.shader.ContainsProperty(propertyNameProperty.stringValue))
            {
                propertyNameProperty.stringValue = "";
                displayName = EditorStyle.NoneStr;
            }
            else
            {
                displayName = propertyNameProperty.stringValue;
            }

            using (GUIHorizontal.Create())
            {
                GUILayout.Label(propertyName, EditorStyles.label, GUILayout.Width(EditorGUIUtility.labelWidth));
                var btn = GUILayout.Button(displayName, EditorStyles.popup);
                if (btn)
                {
                    var menu = CreateMaterialPropertyMenu(render, materialIndex, propertyNameProperty, propertyType);
                    menu.ShowAsContext();
                }
            }
        }

        internal static GenericMenu CreateMaterialPropertyMenu(Renderer render, int materialIndex, SerializedProperty property, ShaderUtil.ShaderPropertyType propertyType)
        {
            var menu = new GenericMenu();
            menu.AddItem(EditorStyle.NoneStr, string.IsNullOrEmpty(property.stringValue), () =>
            {
                property.stringValue = "";
                property.serializedObject.ApplyModifiedProperties();
            });

            if (render == null) return menu;

            menu.AddSeparator("");
            var material = render.GetMaterial(materialIndex);
            if (material == null) return menu;

            var shader = material.shader;
            var shaderCount = ShaderUtil.GetPropertyCount(shader);
            for (var i = 0; i < shaderCount; i++)
            {
                var index = i;
                var hidden = ShaderUtil.IsShaderPropertyHidden(shader, index);
                var shaderPropertyType = ShaderUtil.GetPropertyType(shader, index);
                if (propertyType == ShaderUtil.ShaderPropertyType.Float)
                {
                    if (shaderPropertyType != ShaderUtil.ShaderPropertyType.Float && shaderPropertyType != ShaderUtil.ShaderPropertyType.Range) continue;
                }
                else if (shaderPropertyType != propertyType) continue;

                var shaderPropertyName = ShaderUtil.GetPropertyName(shader, index);
                var shaderPropertyDescription = ShaderUtil.GetPropertyDescription(shader, index);
                if (shaderPropertyType == ShaderUtil.ShaderPropertyType.TexEnv)
                {
                    shaderPropertyName += "_ST";
                }

                var displayName = shaderPropertyName + "\t\t" + shaderPropertyDescription;
                menu.AddItem(!hidden, displayName, property.stringValue == shaderPropertyName, () =>
                {
                    property.stringValue = shaderPropertyName;
                    property.serializedObject.ApplyModifiedProperties();
                });
            }

            return menu;
        }

        #endregion

        #region Components Tree Menu

        public static SearchableDropdownItem CreateComponentsTreeMenu<TComponent>(Transform parent) where TComponent : Component
        {
            return CreateComponentsTreeMenu(typeof(TComponent), parent);
        }

        public static SearchableDropdownItem CreateComponentsTreeMenu(Type componentType, Transform parent)
        {
            var root = new SearchableDropdownItem(componentType.Name);
            root.AddChild(new SearchableDropdownItem(EditorStyle.NoneStr, null));
            root.AddSeparator();
            CreateComponentsTreeMenuRecursion(componentType, root, parent, "");
            return root;
        }

        private static void CreateComponentsTreeMenuRecursion<TComponent>(SearchableDropdownItem root, Transform parent, string path) where TComponent : Component
        {
            CreateComponentsTreeMenuRecursion(typeof(TComponent), root, parent, path);
        }

        private static void CreateComponentsTreeMenuRecursion(Type componentType, SearchableDropdownItem root, Transform parent, string path)
        {
            var components = parent.GetComponents(componentType);
            foreach (var component in components)
            {
                var componentName = component.GetType().Name;
                var child = new SearchableDropdownItem(path + componentName, component)
                {
                    icon = EditorGUIUtility.ObjectContent(component, component.GetType()).image as Texture2D
                };

                root.AddChild(child);
            }

            if (parent.childCount == 0) return;
            if (components.Length > 0)
            {
                root.AddSeparator();
            }

            for (var i = 0; i < parent.childCount; i++)
            {
                var childTrans = parent.GetChild(i);
                CreateComponentsTreeMenuRecursion(componentType, root, childTrans, path + childTrans.name + " \\ ");
            }
        }

        public static void ComponentTreeMenu<TComponent>(string propertyName, SerializedProperty property, Transform parent, Action<TComponent> onClick = null) where TComponent : Component
        {
            ComponentTreeMenu(typeof(TComponent), propertyName, property, parent, obj =>
            {
                var target = (TComponent) obj;
                onClick?.Invoke(target);
            });
        }

        public static void ComponentTreeMenu(Type componentType, string propertyName, SerializedProperty property, Transform parent, Action<UnityEngine.Object> onClick = null)
        {
            var menu = parent != null ? CreateComponentsTreeMenu(componentType, parent) : null;

            void OnClick(SearchableDropdownItem item)
            {
                UnityEngine.Object target = null;
                if (item.Value == null)
                {
                    property.objectReferenceValue = null;
                }
                else
                {
                    target = (UnityEngine.Object)item.Value;
                    property.objectReferenceValue = target;
                }

                property.serializedObject.ApplyModifiedProperties();
                onClick?.Invoke(target);
            }

            SearchableComponentDropdownList(componentType, propertyName, property, menu, OnClick);
        }

        public static void SearchableComponentDropdownList<TComponent>(string propertyName, SerializedProperty property, SearchableDropdownItem root, Action<SearchableDropdownItem> onClick = null) where TComponent : Component
        {
            SearchableComponentDropdownList(typeof(TComponent), propertyName, property, root, onClick);
        }

        public static void SearchableComponentDropdownList(Type componentType, string propertyName, SerializedProperty property, SearchableDropdownItem root, Action<SearchableDropdownItem> onClick = null)
        {
            using (GUIErrorColorArea.Create(property.objectReferenceValue == null))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(propertyName, EditorStyles.label, GUILayout.Width(EditorGUIUtility.labelWidth));
                property.objectReferenceValue = EditorGUILayout.ObjectField(property.objectReferenceValue, componentType, true);

                if (root != null)
                {
                    var btnRect = GUILayoutUtility.GetLastRect();
                    var btnType = GUILayout.Button("▼", EditorStyles.popup, GUILayout.Width(EditorStyle.SingleButtonWidth));
                    if (btnType)
                    {
                        btnRect.width = EditorGUIUtility.currentViewWidth;
                        var dropdown = new SearchableDropdown(root, onClick);
                        dropdown.Show(btnRect, 500f);
                    }
                }

                GUILayout.EndHorizontal();
            }
        }

        #endregion

        #region Dropdown

        public static void DropdownList(string propertyName, GenericMenu menu, Func<bool> checkNullFunc, Action resetFunc, Func<string> currentDisplayNameGetter)
        {
            var isNull = checkNullFunc();
            if (isNull)
            {
                resetFunc();
            }

            using (GUIErrorColorArea.Create(isNull))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(propertyName, EditorStyle.RichLabel, GUILayout.Width(EditorGUIUtility.labelWidth));

                var displayName = currentDisplayNameGetter();
                var currentPropertyDisplayName = isNull ? EditorStyle.NoneStr : displayName;
                var btnProperty = GUILayout.Button(currentPropertyDisplayName, EditorStyles.popup);
                if (btnProperty)
                {
                    menu.ShowAsContext();
                }

                GUILayout.EndHorizontal();
            }
        }

        public static void SearchableDropdownList(string propertyName, SearchableDropdownItem root, Func<bool> checkNullFunc, Action resetFunc, Func<string> currentDisplayNameGetter, Action<SearchableDropdownItem> onClick = null)
        {
            var isNull = checkNullFunc();
            if (isNull)
            {
                resetFunc();
            }

            using (GUIErrorColorArea.Create(isNull))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(propertyName, GUILayout.Width(EditorGUIUtility.labelWidth));

                var displayName = currentDisplayNameGetter();
                var currentPropertyDisplayName = isNull ? EditorStyle.NoneStr : displayName;
                var btnRect = GUILayoutUtility.GetRect(new GUIContent(currentPropertyDisplayName), EditorStyles.toolbarButton);
                var btnType = GUI.Button(btnRect, displayName, EditorStyles.popup);
                if (btnType)
                {
                    btnRect.width = EditorGUIUtility.currentViewWidth;
                    var dropdown = new SearchableDropdown(root, onClick);
                    dropdown.Show(btnRect, 500f);
                }

                GUILayout.EndHorizontal();
            }
        }

        #endregion
    }
}

#endif