using UnityEditor;

namespace JescoDev.SmoothBrainStates.Editor {
    public abstract class SerializedObjectOrPropertyExtension {
        public readonly SerializedObjectOrProperty Property;

        public SerializedObjectOrPropertyExtension(SerializedObjectOrProperty property) {
            Property = property;
        }
        
        public void ApplyModifiedProperties() => Property.serializedObject.ApplyModifiedProperties();
        public SerializedProperty FindPropertyRelative(string relativePropertyPath)
            => Property.FindProperty(relativePropertyPath);
        
        public abstract bool IsValid();
    }
}