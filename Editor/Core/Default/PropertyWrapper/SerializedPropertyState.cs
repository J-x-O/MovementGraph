using JescoDev.SmoothBrainStates.States;
using UnityEditor;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.Editor {
    public class SerializedPropertyState : SerializedPropertyExtension {

        public State State => Property.managedReferenceValue as State;
        
        public string Identifier {
            get => _identifierProperty.stringValue;
            set => _identifierProperty.stringValue = value;
        }
        
        public string Guid {
            get => _guidProperty.stringValue;
            set => _guidProperty.stringValue = value;
        }
        
        public Vector2 Position {
            get => _positionProperty.vector2Value;
            set => _positionProperty.vector2Value = value;
        }

        private readonly SerializedProperty _identifierProperty;
        private readonly SerializedProperty _guidProperty;
        private readonly SerializedProperty _positionProperty;
    
        public SerializedPropertyState(SerializedObjectOrProperty property) : this(property.AsSerializedProperty()) { }
        public SerializedPropertyState(SerializedProperty property) : base(property) {
            if (property == null) return;
            _identifierProperty = property.FindPropertySave("_identifier");
            _guidProperty = property.FindPropertySave("_guid");
            _positionProperty = property.FindPropertySave("_position");
        }
        
        public override bool IsValid() {
            return Property != null && _identifierProperty != null && _guidProperty != null && _positionProperty != null;
        }
    }

    public class SerializedPropertyMovementState : SerializedPropertyState {
        
        private readonly SerializedProperty _tagsProperty;
        
        public SerializedPropertyMovementState(SerializedProperty property) : base(property) {
            _tagsProperty = property.FindPropertyRelative("_tags");
        }

        public void AddTag(string tag) {
            
        }
        
        public void RemoveTag(string tag) {
            
        }
    }
}