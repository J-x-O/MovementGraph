using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace JescoDev.MovementGraph.Editor.Utility {
    public class OpenScriptManipulator : ContextualMenuManipulator {

        public OpenScriptManipulator(Type target) : base(menuEvent => {
            menuEvent.menu.AppendAction("Open Script", _ => OpenScriptPath(target));
            menuEvent.menu.AppendSeparator();
        }) { }

        private static void OpenScriptPath(Type targetType) {
            string path = GetScriptPath(targetType);
            AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<Object>(path));
        }

        private static string GetScriptPath(Type targetType) {
            const string cachePrefix = "MovementGraph.ScriptPath.";
            string cacheName = cachePrefix + targetType.Name;

            string cachedValue = EditorPrefs.GetString(cacheName, null);
            if (cachedValue != null) {
                if (MatchFile(targetType, cachedValue)) return cachedValue;
                EditorPrefs.DeleteKey(cacheName);
            }
            
            string[] guids = AssetDatabase.FindAssets("t:Script");
            foreach (string guid in guids) {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                if (!MatchFile(targetType, path)) continue;
                EditorPrefs.SetString(cachePrefix + targetType.Name, path);
                return path;
            }
            Debug.LogWarning("Could not find script path for " + targetType.Name + " in project.");
            return null;
        }

        private static bool MatchFile(Type targetType, string filePath) {
            if (!filePath.EndsWith(".cs")) return false;
            return filePath.EndsWith(targetType.Name + ".cs");
        }

        private static bool MatchFilePrecise(Type targetType, string filePath) {
            if (!filePath.EndsWith(".cs")) return false;
            string regex = targetType.BaseType == null
                ? $"\\s*public\\s*class\\s*{targetType.Name}\\s*{{"
                : $"\\s*public\\s*class\\s*{targetType.Name}\\s*:\\s*{targetType.BaseType.Name}\\s*{{";
            
            // load the file content and check if it matches the regex
            try {
                string fileContent = File.ReadAllText(filePath);
                return Regex.IsMatch(fileContent, regex);
            }
            catch (Exception) { return false; }
        }
    }
}