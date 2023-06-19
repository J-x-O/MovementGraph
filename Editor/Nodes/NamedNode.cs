using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Editor.MovementEditor.PropertyUtility;
using Entities.Movement.States;
using JescoDev.MovementGraph.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

namespace Editor.MovementEditor {
    public abstract class NamedNode : BaseNode {
        
        
        protected PropertyField NodeHeader { get; private set; }
        private string _oldIdentifier;

        public NamedNode(MovementGraphView view, SerializedPropertyState state, State stateObject)
            : base(view, state, stateObject) {
        }

        protected void RebuildHeader(List<FieldInfo> fieldInfos) {
            titleContainer.Clear();
            CreateHeader();
            titleContainer.Add(NodeHeader);
            fieldInfos.RemoveAll(element => element.Name is "_identifier");
        }
        
        private void CreateHeader() {
            // this is called from the parent constructor => _identifier not yet initialized
            SerializedProperty identifier = State.FindPropertyRelative("_identifier");
            NodeHeader = new PropertyField(identifier, "");
            NodeHeader.BindProperty(identifier);
            _oldIdentifier = identifier.stringValue;
            NodeHeader.RegisterValueChangeCallback(HandleChange);
            NodeHeader.AddToClassList("NodeHeader");
        }
        
        private void HandleChange(SerializedPropertyChangeEvent evt) {
            string newIdentifier = evt.changedProperty.stringValue;
            foreach (SerializedPropertyTransition transition in
                     from node in View.NodeManager._nodes
                     from port in node.Ports
                     from transition in port.PortProperty.GetTransitions()
                     where transition.StateIdentifier == _oldIdentifier select transition) {
                transition.StateIdentifier = newIdentifier;
            }
            View.LayerProperty.ApplyModifiedProperties();
            _oldIdentifier = newIdentifier;
        }
        
        
    }
}