using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JescoDev.MovementGraph.Attributes;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace JescoDev.MovementGraph.Editor.Attributes {
    
    [CustomPropertyDrawer(typeof(PrintReferenceAttribute))]
    public class PrintReferenceAttributeDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            position = EditorGUI.PrefixLabel(position, new GUIContent(property.displayName));
            
            object reference = property.managedReferenceValue;
            if (reference == null) {
                EditorGUI.LabelField(position, "Nothing Referenced");
                return;
            }

            for (int i = 0; i < 10; i++) {
                SerializedProperty current = property.serializedObject.GetIterator();
                do {
                    if (current.propertyType != SerializedPropertyType.ManagedReference) continue;
                    if (current.managedReferenceValue != property.managedReferenceValue) continue;
                    string path = current.propertyPath.Replace(".Array.data[", "[");
                    path = Regex.Replace(path, @"(<)(\w*)(>k__BackingField)", "$2");
                    path = Regex.Replace(path, @"(_)(\w)", m => m.Groups[2].Value.ToUpper());
                    EditorGUI.LabelField(position, path);
                    return;
                    
                } while (current.Next(current.depth < i));
            }
            
            EditorGUI.LabelField(position, "Nothing Found");
        }
    }
}