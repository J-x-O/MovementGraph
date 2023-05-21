using System.Collections.Generic;
using System.Reflection;
using Entities.Movement.States;
using Movement.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

namespace Editor.MovementEditor {
    public abstract class NamedNode : BaseNode {
        
        protected PropertyField NodeHeader { get; private set; }
        
        public NamedNode(GraphView view, SerializedProperty state, State stateObject)
            : base(view, state, stateObject) { }

        protected void RebuildHeader(List<FieldInfo> fieldInfos) {
            titleContainer.Clear();
            CreateHeader();
            titleContainer.Add(NodeHeader);
            fieldInfos.RemoveAll(element => element.Name is "_identifier");
        }
        
        private void CreateHeader() {
            SerializedProperty identifier = State.FindPropertyRelative("_identifier");
            NodeHeader = new PropertyField(identifier, "");
            NodeHeader.BindProperty(identifier);
            NodeHeader.AddToClassList("NodeHeader");
        }
    }
}