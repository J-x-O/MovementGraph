using System.Collections.Generic;
using System.Linq;
using Entities.Movement.States;
using JescoDev.MovementGraph.States;
using UnityEditor;

namespace Editor.MovementEditor.PropertyUtility {
    public class SerializedPropertyMovementPort : SerializedPropertyExtension {

        public string Identifier => Property.name;
        public MovementPort Instance => Property.managedReferenceValue as MovementPort;
        public readonly SerializedProperty TransitionsProperty;

        public SerializedPropertyMovementPort(SerializedProperty property) : base(property) {
            Property.managedReferenceValue ??= new MovementPort();
            ApplyModifiedProperties();
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
            => RemoveTransition(port.BaseNode.GetIdentifier(), port.BaseNode.Index, port.Identifier);
        public void RemoveTransition(string nodeIdentifier, int nodeIndex, string portIdentifier) {
            TransitionsProperty.RemoveArrayElement(element => {
                SerializedPropertyTransition transition = new(element);
                return transition.PortIdentifier == portIdentifier &&
                       string.IsNullOrEmpty(nodeIdentifier)
                        ? transition.StateIdentifier == nodeIdentifier
                        : transition.StateIndex == nodeIndex;
            });
        }

        public void AddTransition(BoundPort port)
            => AddTransition(port.BaseNode.GetIdentifier(), port.BaseNode.Index, port.Identifier);
        public void AddTransition(string nodeIdentifier, int nodeIndex, string portIdentifier) {
            TransitionsProperty.AppendArrayElement(element => {
                SerializedPropertyTransition transition = new(element);
                transition.PortIdentifier = portIdentifier;
                transition.StateIdentifier = nodeIdentifier;
                transition.StateIndex = nodeIndex;
            });
        }

        public bool HasTransitionTo(BoundPort port)
            => HasTransitionTo(port.BaseNode.GetIdentifier(), port.BaseNode.Index, port.Identifier);
        public bool HasTransitionTo(string nodeIdentifier, int nodeIndex, string portIdentifier) {
            return GetTransitions().Any(element => {
                if(element.PortIdentifier != nodeIdentifier) return false;
                if (string.IsNullOrEmpty(nodeIdentifier)) {
                    if(element.StateIdentifier != portIdentifier) return false;
                }
                else {
                    if(element.StateIndex != nodeIndex) return false;
                }

                return true;
            });
        }
    }

    public class SerializedPropertyTransition : SerializedPropertyExtension {

        public int StateIndex {
            get => _stateIndexProperty.intValue;
            set => _stateIndexProperty.intValue = value;
        }
        
        public string StateIdentifier {
            get => _stateIdentifierProperty.stringValue;
            set => _stateIdentifierProperty.stringValue = value;
        }
        
        public string PortIdentifier {
            get => _portIdentifierProperty.stringValue;
            set => _portIdentifierProperty.stringValue = value;
        }
        
        private readonly SerializedProperty _stateIndexProperty;
        private readonly SerializedProperty _stateIdentifierProperty;
        private readonly SerializedProperty _portIdentifierProperty;
        
        public SerializedPropertyTransition(SerializedProperty property) : base(property){
            _stateIndexProperty = property.FindPropertyRelative("_stateIndex");
            _stateIdentifierProperty = property.FindPropertyRelative("_stateIdentifier");
            _portIdentifierProperty = property.FindPropertyRelative("_portIdentifier");
        }
            
    }
}