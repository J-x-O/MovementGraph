﻿using Movement.States;
using UnityEditor;
using UnityEngine;

namespace Editor.MovementEditor.PropertyUtility {
    public class SerializedPropertyState : SerializedPropertyExtension {

        public State State => Property.managedReferenceValue as State;
        
        public string Identifier {
            get => _identifierProperty.stringValue;
            set => _identifierProperty.stringValue = value;
        }
        
        public Vector2 Position {
            get => _positionProperty.vector2Value;
            set => _positionProperty.vector2Value = value;
        }

        private readonly SerializedProperty _identifierProperty;
        private readonly SerializedProperty _positionProperty;
    
        public SerializedPropertyState(SerializedProperty property) : base(property){
            _identifierProperty = property.FindPropertyRelative("_identifier");
            _positionProperty = property.FindPropertyRelative("_position");
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