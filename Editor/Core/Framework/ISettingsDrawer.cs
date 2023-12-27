using UnityEditor;
using UnityEngine.UIElements;

namespace JescoDev.SmoothBrainStates.Editor {
    public interface ISettingsDrawer {
        
        public bool IsValid(SerializedObject target);
        
        public VisualElement CreateSettings(SmoothBrainStatesRoot root, SerializedObject target);
        
        public string GetName();
    }
}