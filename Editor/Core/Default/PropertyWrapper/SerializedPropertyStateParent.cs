using System;
using System.Collections.Generic;
using UnityEditor;

namespace JescoDev.SmoothBrainStates.Editor {
    public class SerializedPropertyStateParent : SerializedObjectOrPropertyExtension {
        
        private readonly SerializedProperty _statesProperty;
        public readonly SerializedPropertyConnector ConnectorProperty;
        
        public SerializedPropertyStateParent(SerializedObjectOrProperty property) : base(property) {
            if (property == null) return;
            _statesProperty = property.FindProperty("_states");
            ConnectorProperty = new SerializedPropertyConnector(property.FindProperty("_connector"));
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
        
        public bool RemoveState(object instance) => _statesProperty.RemoveArrayElement(MatchingInstance(instance));

        public void ClearStates() => _statesProperty.arraySize = 0;
        
        private static Func<SerializedProperty, bool> MatchingInstance(object instance) {
            return property => property.managedReferenceValue == instance;
        }

        public IEnumerable<SerializedPropertyState> GetStates() {
            List<SerializedPropertyState> states = new();
            for (int i = 0; i < _statesProperty.arraySize; i++) {
                SerializedProperty element = _statesProperty.GetArrayElementAtIndex(i);
                states.Add(new SerializedPropertyState(element));
            }
            states.AddRange(ConnectorProperty.GetStates());
            return states;
        }

        public override bool IsValid() {
            return Property != null && _statesProperty != null && ConnectorProperty.IsValid();
        }
    }
}