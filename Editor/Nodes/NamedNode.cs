using System.Collections.Generic;
using System.Reflection;
using Entities.Movement.States;
using Movement.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

namespace Editor.MovementEditor {
    public abstract class NamedNode : BaseNode {
        
        public string Identifier => _identifier.stringValue;
        private readonly SerializedProperty _identifier;
        protected PropertyField NodeHeader { get; private set; }

        public NamedNode(MovementGraphView view, SerializedProperty state, State stateObject)
            : base(view, state, stateObject) {
            _identifier = State.FindPropertyRelative("_identifier");
        }

        protected void RebuildHeader(List<FieldInfo> fieldInfos) {
            titleContainer.Clear();
            CreateHeader();
            titleContainer.Add(NodeHeader);
            fieldInfos.RemoveAll(element => element.Name is "_identifier");
        }
        
        private void CreateHeader() {
            NodeHeader = new PropertyField(_identifier, "");
            NodeHeader.BindProperty(_identifier);
            NodeHeader.AddToClassList("NodeHeader");
        }
    }
    
    public static class BaseNodeUtility {
        public static string GetIdentifier(this BaseNode node) {
            return node is NamedNode namedNode ? namedNode.Identifier : "";
        }
    }
}