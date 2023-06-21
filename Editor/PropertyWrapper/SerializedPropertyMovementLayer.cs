using System;
using System.Collections.Generic;
using Gameplay.Movement.Layer;
using UnityEditor;
using UnityEngine;

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
        
        public SerializedPropertyState InNode => new SerializedPropertyState(_inNodeProperty);
        public SerializedPropertyState OutNode => new SerializedPropertyState(_outNodeProperty);
        
        private readonly SerializedProperty _identifierProperty;
        private readonly SerializedProperty _compositionProperty;
        private readonly SerializedProperty _statesProperty;
        private readonly SerializedProperty _inNodeProperty;
        private readonly SerializedProperty _outNodeProperty;
        
        public SerializedPropertyMovementLayer(SerializedProperty property) : base(property) {
            _identifierProperty = property.FindPropertyRelative("_identifier");
            _compositionProperty = property.FindPropertyRelative("_composition");
            _statesProperty = property.FindPropertyRelative("_states");
            
            SerializedProperty connectorProperty = property.FindPropertyRelative("_connector");
            _inNodeProperty = connectorProperty.FindPropertyRelative("_inNode");
            _inNodeProperty.managedReferenceValue ??= new LayerIn();
            _outNodeProperty = connectorProperty.FindPropertyRelative("_outNode");
            _outNodeProperty.managedReferenceValue ??= new LayerOut();

            property.serializedObject.ApplyModifiedProperties();
        }

        public SerializedPropertyState AddState(object instance) {
            SerializedProperty alreadyExisting = _statesProperty.GetArrayElement(MatchingInstance(instance), true);
            if (alreadyExisting != null) return new SerializedPropertyState(alreadyExisting);
            
            SerializedProperty property = _statesProperty.AppendArrayElement(element
                => element.managedReferenceValue = instance);
            return new SerializedPropertyState(property);
        }

        public int GetStateIndex(SerializedPropertyState target) {
            return _statesProperty.GetArrayIndex(target);
        }
        
        public void RemoveState(object instance) => _statesProperty.RemoveArrayElement(MatchingInstance(instance));

        public void ClearStates() => _statesProperty.arraySize = 0;
        
        public void ResetInOut() {
            _inNodeProperty.managedReferenceValue = new LayerIn();
            _outNodeProperty.managedReferenceValue = new LayerOut();
            OutNode.Position = new Vector2(250, 0);
        }
        
        private static Func<SerializedProperty, bool> MatchingInstance(object instance) {
            return property => property.managedReferenceValue == instance;
        }

        public IEnumerable<SerializedPropertyState> GetStates() {
            List<SerializedPropertyState> states = new();
            for (int i = 0; i < _statesProperty.arraySize; i++) {
                SerializedProperty element = _statesProperty.GetArrayElementAtIndex(i);
                states.Add(new SerializedPropertyState(element));
            }
            states.Add(new SerializedPropertyState(_inNodeProperty));
            states.Add(new SerializedPropertyState(_outNodeProperty));
            return states;
        }
    }
}