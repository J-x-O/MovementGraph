using JescoDev.SmoothBrainStates.Editor;
using UnityEditor;
using UnityEngine.UIElements;

namespace JescoDev.MovementGraph.Editor.DisplayHandling {
    public abstract class DisplayHandler {
        
        public abstract bool IsValid(SerializedProperty target);
        
        public abstract VisualElement CreateDisplay(SmoothBrainStatesRoot root, SerializedProperty target);
        
    }
}