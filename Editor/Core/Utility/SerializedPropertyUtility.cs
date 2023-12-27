using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.Editor {
    public static class SerializedPropertyUtility {
        
        /// <summary> Calling serializedObject. </summary>
        public static SerializedProperty FindPropertySave(this SerializedProperty target, string propertyPath) {
            return target.depth == -1 
                ? target.serializedObject.FindProperty(propertyPath)
                : target.FindPropertyRelative(propertyPath);
        }
        
        public static SerializedProperty AppendArrayElement(this SerializedProperty target, Action<SerializedProperty> setData) {
            target.arraySize++;
            SerializedProperty newElement = target.GetArrayElementAtIndex(target.arraySize - 1);
            setData(newElement);
            target.serializedObject.ApplyModifiedProperties();
            return newElement;
        }
        
        public static SerializedProperty AppendArrayElement<T>(this SerializedProperty target, Action<T> setData) {
            target.arraySize++;
            SerializedProperty newElement = target.GetArrayElementAtIndex(target.arraySize - 1);
            setData((T) Activator.CreateInstance(typeof(T), newElement));
            target.serializedObject.ApplyModifiedProperties();
            return newElement;
        }
        
        public static int GetArrayIndex(this SerializedProperty target, SerializedProperty p, bool hideLog = false) {
            
            for (int i = 0; i < target.arraySize; i++) {
                SerializedProperty element = target.GetArrayElementAtIndex(i);
                if (SerializedProperty.EqualContents(element, p)) return i;
            }
            if(!hideLog) Debug.LogError("Couldn't find element in array!");
            return -1;
        }
        
        public static SerializedProperty GetArrayElement(this SerializedProperty target, Func<SerializedProperty, bool> predicate, bool hideLog = false) {
            
            for (int i = 0; i < target.arraySize; i++) {
                SerializedProperty element = target.GetArrayElementAtIndex(i);
                if (predicate.Invoke(element)) return element;
            }
            if(!hideLog) Debug.LogError("Couldn't find element in array!");
            return null;
        }

        public static void RemoveArrayElement(this SerializedProperty target, SerializedProperty p) {
            int i = target.GetArrayIndex(p);
            if(i == -1) return;
            target.DeleteArrayElementAtIndex(i);
        }
        
        
        public static bool Any(this SerializedProperty target, Func<SerializedProperty, bool> predicate) {
            for (int i = 0; i < target.arraySize; i++) {
                SerializedProperty element = target.GetArrayElementAtIndex(i);
                if (predicate(element)) return true;
            }
            return false;
        }
        
        public static IEnumerable<SerializedProperty> ForEach(this SerializedProperty target) {
            for (int i = 0; i < target.arraySize; i++) {
                SerializedProperty element = target.GetArrayElementAtIndex(i);
                yield return element;
            }
        }
        
        public static void RemoveAll(this SerializedProperty target, Func<SerializedProperty, bool> predicate) {
            for (int i = 0; i < target.arraySize; i++) {
                SerializedProperty element = target.GetArrayElementAtIndex(i);
                if (!predicate(element)) continue;
                target.DeleteArrayElementAtIndex(i);
                i--;
            }
            target.serializedObject.ApplyModifiedProperties();
        }
        
        public static bool RemoveArrayElement(this SerializedProperty target, Func<SerializedProperty, bool> predicate) {
            for (int i = 0; i < target.arraySize; i++) {
                SerializedProperty element = target.GetArrayElementAtIndex(i);
                if (!predicate(element)) continue;
                
                target.DeleteArrayElementAtIndex(i);
                target.serializedObject.ApplyModifiedProperties();
                return true;
            }
            return false;
        }
        
        public static Type GetFieldType(this SerializedProperty property) {
            Type parentType = property.serializedObject.targetObject.GetType();
            FieldInfo fi = parentType.GetField(property.propertyPath);  
            return fi.FieldType;
        }
        
        public static object GetValue(this SerializedProperty property) {
            Type parentType = property.serializedObject.targetObject.GetType();
            FieldInfo fi = parentType.GetField(property.propertyPath);  
            return fi.GetValue(property.serializedObject.targetObject);
        }
 
        // Sets value from SerializedProperty - even if value is nested
        public static void SetValue(this SerializedProperty property, object val) {
            Type parentType = property.serializedObject.targetObject.GetType();
            FieldInfo fi = parentType.GetField(property.propertyPath);  
            fi.SetValue(property.serializedObject.targetObject, val);
        }
        
    }
}