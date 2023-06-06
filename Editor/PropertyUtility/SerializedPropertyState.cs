using UnityEditor;

namespace Editor.MovementEditor.PropertyUtility {
    public class SerializedPropertyNamedState : SerializedPropertyExtension {

        public string Identifier {
            get => _identifierProperty.stringValue;
            set => _identifierProperty.stringValue = value;
        }

        private readonly SerializedProperty _identifierProperty;
    
        public SerializedPropertyNamedState(SerializedProperty property) : base(property){
            _identifierProperty = property.FindPropertyRelative("_identifier");
        }
            
    }
}