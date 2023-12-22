using JescoDev.SmoothBrainStates.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace JescoDev.SmoothBrainStates.Tags.Editor {
    public class SettingsHandlerTags : SettingsHandler {
        public override bool IsValid(SerializedObject target) {
            target.get
        }

        public override VisualElement CreateSettings(SmoothBrainStatesRoot root, SerializedObject target) {
            ColorField test = new ColorField();
        }
    }
}