using System.Collections.Generic;
using System.Linq;
using Entities.Movement.States;
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
            => RemoveTransition(port.MovementEditorNode.Guid, port.Identifier);
        public void RemoveTransition(string guid, string portIdentifier) {
            TransitionsProperty.RemoveArrayElement(element => {
                SerializedPropertyTransition transition = new(element);
                return transition.StateGuid == guid
                    && transition.PortIdentifier == portIdentifier;
            });
        }

        public void AddTransition(BoundPort port)
            => AddTransition(port.MovementEditorNode.Guid, port.Identifier);

        public void AddTransition(string guid, string portIdentifier) {
            if(HasTransitionTo(guid, portIdentifier)) return;
            TransitionsProperty.AppendArrayElement(element => {
                SerializedPropertyTransition transition = new(element);
                transition.PortIdentifier = portIdentifier;
                transition.StateGuid = guid;
            });
            ApplyModifiedProperties();
        }

        public bool HasTransitionTo(BoundPort port)
            => HasTransitionTo(port.MovementEditorNode.Identifier, port.Identifier);
        public bool HasTransitionTo(string guid, string portIdentifier) {
            return GetTransitions().Any(element
                => element.StateGuid == guid
                   && element.PortIdentifier == portIdentifier);
        }
    }

    public class SerializedPropertyTransition : SerializedPropertyExtension {

        public string StateGuid {
            get => _stateGuidProperty.stringValue;
            set => _stateGuidProperty.stringValue = value;
        }
        
        public string PortIdentifier {
            get => _portIdentifierProperty.stringValue;
            set => _portIdentifierProperty.stringValue = value;
        }
        
        private readonly SerializedProperty _stateGuidProperty;
        private readonly SerializedProperty _portIdentifierProperty;
        
        public SerializedPropertyTransition(SerializedProperty property) : base(property){
            _stateGuidProperty = property.FindPropertyRelative("_stateGuid");
            _portIdentifierProperty = property.FindPropertyRelative("_portIdentifier");
        }
            
    }
}