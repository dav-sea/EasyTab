using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EasyTab.Internals
{
    internal enum ConditionOperator
    {
        // A field is visible/enabled only if all conditions are true.
        AND,

        // A field is visible/enabled if at least ONE condition is true.
        OR,
    }

    internal enum ActionOnConditionFail
    {
        // If condition(s) are false, don't draw the field at all.
        DONT_DRAW,

        // If condition(s) are false, just set the field as disabled.
        JUST_DISABLE,
    }

    internal class EnableIfAttribute : ShowIfAttribute
    {
        public EnableIfAttribute(params string[] conditions)
            : base(ActionOnConditionFail.JUST_DISABLE, ConditionOperator.AND, conditions)
        {
        }
    }

    internal class ShowIfAttribute : PropertyAttribute
    {
        public ActionOnConditionFail Action { get; private set; }
        public ConditionOperator Operator { get; private set; }
        public string[] Conditions { get; private set; }

        public ShowIfAttribute(ActionOnConditionFail action, ConditionOperator conditionOperator,
            params string[] conditions)
        {
            Action = action;
            Operator = conditionOperator;
            Conditions = conditions;
        }

        public ShowIfAttribute(params string[] conditions)
        {
            Action = ActionOnConditionFail.DONT_DRAW;
            Operator = ConditionOperator.AND;
            Conditions = conditions;
        }
    }

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(ShowIfAttribute), true)]
    internal class ShowIfAttributeDrawer : PropertyDrawer
    {
        #region Reflection helpers.

        private static MethodInfo GetMethod(object target, string methodName)
        {
            return GetAllMethods(target, m => m.Name.Equals(methodName,
                StringComparison.InvariantCulture)).FirstOrDefault();
        }

        private static FieldInfo GetField(object target, string fieldName)
        {
            return GetAllFields(target, f => f.Name.Equals(fieldName,
                StringComparison.InvariantCulture)).FirstOrDefault();
        }

        private static IEnumerable<FieldInfo> GetAllFields(object target, Func<FieldInfo,
            bool> predicate)
        {
            var types = new List<Type>()
            {
                target.GetType()
            };

            while (types.Last().BaseType != null)
            {
                types.Add(types.Last().BaseType);
            }

            for (var i = types.Count - 1; i >= 0; i--)
            {
                var fieldInfos = types[i]
                    .GetFields(BindingFlags.Instance | BindingFlags.Static |
                               BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(predicate);

                foreach (var fieldInfo in fieldInfos)
                {
                    yield return fieldInfo;
                }
            }
        }

        private static IEnumerable<MethodInfo> GetAllMethods(object target,
            Func<MethodInfo, bool> predicate)
        {
            var methodInfos = target.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Static |
                            BindingFlags.NonPublic | BindingFlags.Public)
                .Where(predicate);

            return methodInfos;
        }

        #endregion

        private bool MeetsConditions(SerializedProperty property)
        {
            var showIfAttribute = this.attribute as ShowIfAttribute;
            var target = property.serializedObject.targetObject;
            var conditionValues = new List<bool>();

            foreach (var condition in showIfAttribute.Conditions)
            {
                var conditionField = GetField(target, condition);
                if (conditionField != null &&
                    conditionField.FieldType == typeof(bool))
                {
                    conditionValues.Add((bool)conditionField.GetValue(target));
                }

                var conditionMethod = GetMethod(target, condition);
                if (conditionMethod != null &&
                    conditionMethod.ReturnType == typeof(bool) &&
                    conditionMethod.GetParameters().Length == 0)
                {
                    conditionValues.Add((bool)conditionMethod.Invoke(target, null));
                }
            }

            if (conditionValues.Count > 0)
            {
                bool met;
                if (showIfAttribute.Operator == ConditionOperator.AND)
                {
                    met = true;
                    foreach (var value in conditionValues)
                    {
                        met = met && value;
                    }
                }
                else
                {
                    met = false;
                    foreach (var value in conditionValues)
                    {
                        met = met || value;
                    }
                }

                return met;
            }
            else
            {
                Debug.LogError("Invalid boolean condition fields or methods used!");
                return true;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Calculate the property height, if we don't meet the condition and the draw mode is DontDraw, then height will be 0.
            var meetsCondition = MeetsConditions(property);
            var showIfAttribute = this.attribute as ShowIfAttribute;

            if (!meetsCondition && showIfAttribute.Action ==
                ActionOnConditionFail.DONT_DRAW)
                return 0;
            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent
            label)
        {
            bool meetsCondition = MeetsConditions(property);
            // Early out, if conditions met, draw and go.
            if (meetsCondition)
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }

            var showIfAttribute = this.attribute as ShowIfAttribute;
            if (showIfAttribute.Action == ActionOnConditionFail.DONT_DRAW)
            {
                return;
            }
            else if (showIfAttribute.Action == ActionOnConditionFail.JUST_DISABLE)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.PropertyField(position, property, label, true);
                EditorGUI.EndDisabledGroup();
            }
        }
    }

#endif
}