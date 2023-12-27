using UnityEditor;
using UnityEngine.UIElements;

namespace JescoDev.SmoothBrainStates.Editor {
    public interface ISmoothInspector {
        
        public bool IsValid(SerializedObjectOrProperty target);
        
        public VisualElement CreateDisplay(SmoothBrainStatesRoot root, SerializedObjectOrProperty target);
    }
}