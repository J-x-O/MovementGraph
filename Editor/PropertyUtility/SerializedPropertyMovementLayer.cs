using Gameplay.Movement.Layer;
using UnityEditor;

namespace Editor.MovementEditor.PropertyUtility {
    public class SerializedPropertyMovementLayer : SerializedPropertyExtension {

        public string Identifier {
            get => _identifierProperty.stringValue;
            set => _identifierProperty.stringValue = value;
        }

        public LayerComposition Composition {
            get => (LayerComposition) _compositionProperty.enumValueIndex;
            set => _compositionProperty.enumValueIndex = (int) value;
        }
        
        private readonly SerializedProperty _identifierProperty;
        private readonly SerializedProperty _compositionProperty;
        
        public SerializedPropertyMovementLayer(SerializedProperty property) : base(property) {
            _identifierProperty = property.FindPropertyRelative("_identifier");
            _compositionProperty = property.FindPropertyRelative("_composition");
        }
    }
}