using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editor.MovementEditor {
    public static class SerializedPropertyUtility {
        
        public static SerializedProperty AppendArrayElement(this SerializedProperty target, Action<SerializedProperty> setData) {
            target.arraySize++;
            SerializedProperty newElement = target.GetArrayElementAtIndex(target.arraySize - 1);
            setData(newElement);
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
            target.serializedObject.ApplyModifiedProperties();
        }
        
        
        public static bool Any(this SerializedProperty target, Func<SerializedProperty, bool> predicate) {
            for (int i = 0; i < target.arraySize; i++) {
                SerializedProperty element = target.GetArrayElementAtIndex(i);
                if (predicate(element)) return true;
            }
            return false;
        }
        
        public static void RemoveArrayElement(this SerializedProperty target, Func<SerializedProperty, bool> predicate) {
            for (int i = 0; i < target.arraySize; i++) {
                SerializedProperty element = target.GetArrayElementAtIndex(i);
                if (!predicate(element)) continue;
                
                target.DeleteArrayElementAtIndex(i);
                target.serializedObject.ApplyModifiedProperties();
                return;
            }
            Debug.LogError("Couldn't find element to remove!");
        }
        
        public static object GetValue(this SerializedProperty property) {
            object obj = property.serializedObject.targetObject;

            foreach(string path in property.propertyPath.Split('.') ) {
                Type type = obj.GetType();
                FieldInfo field = type.GetField(path);
                obj = field.GetValue(obj);
            }
            return obj;
        }
 
        // Sets value from SerializedProperty - even if value is nested
        public static void SetValue(this SerializedProperty property, object val) {
            object obj = property.serializedObject.targetObject;
 
            List<KeyValuePair<FieldInfo, object>> list = new();

            foreach(string path in property.propertyPath.Split('.' )) {
                Type type = obj.GetType();
                FieldInfo field = type.GetField(path);
                list.Add(new KeyValuePair<FieldInfo, object>(field, obj));
                obj = field.GetValue( obj );
            }
 
            // Now set values of all objects, from child to parent
            for( int i = list.Count - 1; i >= 0; --i ) {
                list[i].Key.SetValue( list[i].Value, val );
                // New 'val' object will be parent of current 'val' object
                val = list[i].Value;
            }
        }
        
    }
}