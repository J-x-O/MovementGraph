using System;
using System.Collections.Generic;
using System.Reflection;
using JescoDev.MovementGraph.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

namespace JescoDev.MovementGraph.Editor.Nodes {
    
    [Serializable]
    public class BoundNode : NamedNode, IConnectIn, IConnectOut {

        public new MovementState StateObject { get; protected set; }

        public Port InputPort { get; set; }
        public Port OutputPort { get; set; }
        
        protected TagView TagView { get; private set; }
        
        public BoundNode(SerializedProperty state, MovementState stateObject) : base(state, stateObject) {
            StateObject = stateObject;
        }

        protected override void Rebuild(List<FieldInfo> fieldInfos) {
            RebuildHeader(fieldInfos);
            RebuildTagView(fieldInfos);
            this.RebuildInput(this);
            this.RebuildOutput(this);
            RebuildProperties(fieldInfos);
        }

        private void RebuildTagView(List<FieldInfo> fieldInfos) {
            SerializedProperty tags = State.FindPropertyRelative("_tags");
            TagView = new TagView();
            TagView.BindProperty(tags);
            titleContainer.Add(TagView);
            fieldInfos.RemoveAll(element => element.Name is "_tags");
        }

        private void RebuildProperties(List<FieldInfo> fieldInfos) {
            extensionContainer.Clear();
            foreach (FieldInfo fieldInfo in fieldInfos) {
                SerializedProperty property = State.FindPropertyRelative(fieldInfo.Name);
                if (property == null) continue;
                
                PropertyField field = new PropertyField(property);
                field.BindProperty(property);
                extensionContainer.Add(field);
            }
        }
    }
}