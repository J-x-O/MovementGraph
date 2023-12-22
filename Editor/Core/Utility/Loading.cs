﻿using UnityEditor;
using UnityEngine.UIElements;

namespace JescoDev.MovementGraph.Editor.Utility {
    public class Loading {
        public static StyleSheet LoadStyleSheet(string localPath) {
            const string resourcePath = "Packages/com.j-x-o.movement-graph/EditorResources/";
            return (StyleSheet)EditorGUIUtility.Load(resourcePath + localPath);
        }
    }
}