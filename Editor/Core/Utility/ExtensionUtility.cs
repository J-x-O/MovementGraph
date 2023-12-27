using System;
using UnityEditor;

namespace JescoDev.SmoothBrainStates.Editor {
    public static class ExtensionUtility {
        
        public static SerializedProperty GetOrCreateExtension<T>(SerializedObject target) where T : ISmoothExtension, new() {
            SerializedProperty extensions = target.FindProperty("_extensions");
            if (extensions == null) return null;
            for (int i = 0; i < extensions.arraySize; i++) {
                SerializedProperty extension = extensions.GetArrayElementAtIndex(i);
                if (extension.managedReferenceValue is T) return extension;
            }
            SerializedProperty newExt = extensions.AppendArrayElement(element => element.managedReferenceValue = new T());
            return newExt;
        }
        
    }
}