using UnityEditor;
using UnityEngine.UIElements;

namespace JescoDev.SmoothBrainStates.Editor {
    public abstract class SettingsHandler {
        
        public abstract bool IsValid(SerializedObject target);
        
        public abstract VisualElement CreateSettings(SmoothBrainStatesRoot root, SerializedObject target);
        
    }
}