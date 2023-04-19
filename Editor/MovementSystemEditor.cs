using UnityEditor;
using UnityEngine.UIElements;

namespace JescoDev.MovementGraph.Editor {

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
                MovementGraphView view = new MovementGraphView(system);
                view.StretchToParentSize();
                rootVisualElement.Add(view);
            }
        }
    }
}