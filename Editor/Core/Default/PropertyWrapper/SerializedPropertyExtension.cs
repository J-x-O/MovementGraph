using UnityEditor;

namespace JescoDev.SmoothBrainStates.Editor {
    public abstract class SerializedPropertyExtension {
        
        public readonly SerializedProperty Property;

        public SerializedPropertyExtension(SerializedProperty property) {
            Property = property;
        }
        
        public void ApplyModifiedProperties() => Property.serializedObject.ApplyModifiedProperties();
        public SerializedProperty FindPropertyRelative(string relativePropertyPath)
            => Property.FindPropertyRelative(relativePropertyPath);
        
        public static implicit operator SerializedProperty(SerializedPropertyExtension d) => d.Property;
        
        public abstract bool IsValid();
    }
}