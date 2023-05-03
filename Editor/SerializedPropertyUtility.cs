using System;
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
        
        public static int GetArrayIndex(this SerializedProperty target, SerializedProperty p) {
            
            for (int i = 0; i < target.arraySize; i++) {
                SerializedProperty element = target.GetArrayElementAtIndex(i);
                if (SerializedProperty.EqualContents(element, p)) return i;
            }
            Debug.LogError("Couldn't find element in array!");
            return -1;
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
        
    }
}