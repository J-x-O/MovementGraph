using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace JescoDev.SmoothBrainStates.Editor {

    public class RebuildEvent : EventBase<RebuildEvent> {
        protected override void Init()
        {
            base.Init();
            bubbles = true;
            tricklesDown = true;
        }
    }
    
    public class SmoothBrainStatesEditorWindow : EditorWindow {
        
        [MenuItem("WummsenVillage/Movement System Editor")]
        public static void OpenWindow() => GetWindow<SmoothBrainStatesEditorWindow>("Movement System Editor");

        private SmoothBrainStatesRoot _root;
        
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
        
        private void OnRebuildInvoked(RebuildEvent evt) => _root?.RefreshView();

        private void UpdateSelection() {
            _root?.Root?.ApplyModifiedProperties();
            rootVisualElement.Clear();
            
            SmoothBrainStateMashine system = Selection.activeGameObject.GetComponent<SmoothBrainStateMashine>();
            if (system != null) {
                SerializedObject serializedObject = new SerializedObject(system);
                _root = new SmoothBrainStatesRoot(serializedObject);
                _root.StretchToParentSize();
                rootVisualElement.Add(_root);
                _root.RegisterCallback<RebuildEvent>(OnRebuildInvoked);
            }
        }
    }
}