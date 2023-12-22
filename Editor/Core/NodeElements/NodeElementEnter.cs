using System.Collections.Generic;
using System.Reflection;
using JescoDev.MovementGraph.Editor;
using JescoDev.MovementGraph.Editor.Editor.Utility;
using JescoDev.SmoothBrainStates.Attributes;
using UnityEngine.UIElements;

namespace JescoDev.SmoothBrainStates.Editor {
    public class NodeElementEnter : NodeElement {
        
        public NodeElementEnter(MovementEditorNode node) : base(node) { }
        public override bool CanBeApplied() {
            return Node.StateObject.GetType().HasAttribute<SmoothStateEnter>();
        }

        public override void Rebuild(List<FieldInfo> fieldInfos) {
            
            Button button = new Button(() => {
                Node.View.Tracker.EnterSerializedProperty(Node.State.Identifier, Node.State.Property);
            });
            button.text = "Enter";
            button.AddToClassList("EnterButton");
            Node.titleButtonContainer.Add(button);
        }
    }
}