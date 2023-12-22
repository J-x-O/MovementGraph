using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UIElements;

namespace JescoDev.SmoothBrainStates.Editor {

    public class RebuildEvent : EventBase<RebuildEvent> { }
    
    public class SmoothBrainStatesEditorWindow : EditorWindow {
        
        [MenuItem("WummsenVillage/Movement System Editor")]
        public static void OpenWindow() => GetWindow<SmoothBrainStatesEditorWindow>("Movement System Editor");

        private void OnEnable() {
            OnSelectionChange();
            // undo resets our serialize object, which is the reason why we just rebuild
            Undo.undoRedoPerformed += OnSelectionChange;
            PrefabStage.prefabStageOpened += OnPrefabChange;
            PrefabStage.prefabStageClosing += OnPrefabChange;
        }
        
        private void OnDisable() {
            Undo.undoRedoPerformed -= OnSelectionChange;
            PrefabStage.prefabStageOpened -= OnPrefabChange;
            PrefabStage.prefabStageClosing -= OnPrefabChange;
        }

        private void OnPrefabChange(PrefabStage obj) => OnSelectionChange();

        public void OnSelectionChange() {
            if(Selection.activeGameObject == null) return;
            UpdateSelection();
        }
        
        private void OnRebuildInvoked(RebuildEvent evt) => UpdateSelection();

        private void UpdateSelection() {
            rootVisualElement.Clear();
            
            
            SmoothBrainStateMashine system = Selection.activeGameObject.GetComponent<SmoothBrainStateMashine>();
            if (system != null) {
                SerializedObject serializedObject = new SerializedObject(system);
                SmoothBrainStatesRoot root = new SmoothBrainStatesRoot(serializedObject);
                root.StretchToParentSize();
                rootVisualElement.Add(root);
                root.RegisterCallback<RebuildEvent>(OnRebuildInvoked);
            }
        }
    }
}