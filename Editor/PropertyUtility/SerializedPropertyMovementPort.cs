using System.Collections.Generic;
using System.Linq;
using Entities.Movement.States;
using JescoDev.MovementGraph.States;
using UnityEditor;

namespace Editor.MovementEditor.PropertyUtility {
    public class SerializedPropertyMovementPort : SerializedPropertyExtension {

        public string Identifier => Property.name;
        public readonly SerializedProperty TransitionsProperty;

        public SerializedPropertyMovementPort(SerializedProperty property) : base(property) {
            TransitionsProperty = Property.FindPropertyRelative("_transitions");
        }
        
        public SerializedPropertyTransition GetTransition(int i) => new(TransitionsProperty.GetArrayElementAtIndex(i));
        
        public IEnumerable<SerializedPropertyTransition> GetTransitions() {
            List<SerializedPropertyTransition> transitions = new();
            for (int i = 0; i < TransitionsProperty.arraySize; i++) {
                transitions.Add(GetTransition(i));
            }
            return transitions;
        }

        public void RemoveTransition(SerializedPropertyTransition transition) {
            TransitionsProperty.RemoveArrayElement(transition.Property);
        }
        public void RemoveTransition(BoundPort port)
            => RemoveTransition(port.BaseNode.Identifier, port.Identifier);
        public void RemoveTransition(string nodeIdentifier, string portIdentifier) {
            TransitionsProperty.RemoveArrayElement(element => {
                SerializedPropertyTransition transition = new(element);
                return transition.StateIdentifier == nodeIdentifier
                    && transition.PortIdentifier == portIdentifier;
            });
        }

        public void AddTransition(BoundPort port)
            => AddTransition(port.BaseNode.Identifier, port.Identifier);

        public void AddTransition(string nodeIdentifier, string portIdentifier) {
            if(HasTransitionTo(nodeIdentifier, portIdentifier)) return;
            TransitionsProperty.AppendArrayElement(element => {
                SerializedPropertyTransition transition = new(element);
                transition.PortIdentifier = portIdentifier;
                transition.StateIdentifier = nodeIdentifier;
            });
            ApplyModifiedProperties();
        }

        public bool HasTransitionTo(BoundPort port)
            => HasTransitionTo(port.BaseNode.Identifier, port.Identifier);
        public bool HasTransitionTo(string nodeIdentifier, string portIdentifier) {
            return GetTransitions().Any(element
                => element.StateIdentifier == nodeIdentifier
                   && element.PortIdentifier == portIdentifier);
        }
    }

    public class SerializedPropertyTransition : SerializedPropertyExtension {

        public string StateIdentifier {
            get => _stateIdentifierProperty.stringValue;
            set => _stateIdentifierProperty.stringValue = value;
        }
        
        public string PortIdentifier {
            get => _portIdentifierProperty.stringValue;
            set => _portIdentifierProperty.stringValue = value;
        }
        
        private readonly SerializedProperty _stateIdentifierProperty;
        private readonly SerializedProperty _portIdentifierProperty;
        
        public SerializedPropertyTransition(SerializedProperty property) : base(property){
            _stateIdentifierProperty = property.FindPropertyRelative("_stateIdentifier");
            _portIdentifierProperty = property.FindPropertyRelative("_portIdentifier");
        }
            
    }
}