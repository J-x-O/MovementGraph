using UnityEditor;

namespace Editor.MovementEditor.PropertyUtility {
    public class SerializedPropertyExtension {
        
        public readonly SerializedProperty Property;

        public SerializedPropertyExtension(SerializedProperty property) {
            Property = property;
        }
        
        public void ApplyModifiedProperties() => Property.serializedObject.ApplyModifiedProperties();
        
        public static implicit operator SerializedProperty(SerializedPropertyExtension d) => d.Property;
    }
}