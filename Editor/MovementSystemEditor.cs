using System;
using System.Collections.Generic;
using System.Linq;
using Codice.Client.Common.TreeGrouper;
using Entities.Movement;
using Entities.Movement.States;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.MovementEditor {

    public class MovementSystemEditor : EditorWindow {

        [MenuItem("WummsenVillage/Movement System Editor")]
        public static void OpenWindow() => GetWindow<MovementSystemEditor>("Movement System Editor");

        private void OnEnable() {
            OnSelectionChange();
            Undo.undoRedoPerformed += HandleUndo;
        }
        
        private void OnDisable() => Undo.undoRedoPerformed -= HandleUndo;
        
        
        // undo resets our serialize object, which is the reason why we just rebuild
        private void HandleUndo() => OnSelectionChange();

        public void OnSelectionChange() {
            if(Selection.activeGameObject == null) return;
            UpdateSelection();
        }

        private void UpdateSelection() {
            rootVisualElement.Clear();
            MovementSystem system = Selection.activeGameObject.GetComponent<MovementSystem>();
            if (system != null) {
                MovementLayerView view = new MovementLayerView(system);
                view.StretchToParentSize();
                rootVisualElement.Add(view);
            }
        }
    }
}