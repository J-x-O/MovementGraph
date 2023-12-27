using System.Collections.Generic;
using System.Reflection;
using JescoDev.SmoothBrainStates.Attributes;
using JescoDev.SmoothBrainStates.States;
using JescoDev.SmoothBrainStates.SubStates;
using UnityEngine.UIElements;

namespace JescoDev.SmoothBrainStates.Editor {
    public class NodeElementEnter : NodeElement {
        
        public NodeElementEnter(MovementEditorNode node) : base(node) { }
        public override bool CanBeApplied() {
            return Node.StateObject.GetType().HasAttribute<SmoothStateEnter>();
        }

        public override void Rebuild(List<FieldInfo> fieldInfos) {
            fieldInfos.RemoveAll(element => typeof(SmoothInOut).IsAssignableFrom(element.FieldType));
            fieldInfos.RemoveAll(element => typeof(IEnumerable<State>).IsAssignableFrom(element.FieldType));
            Button button = new Button(() => {
                Node.View.Root.Tracker.EnterSerializedProperty(Node.State.Identifier, Node.State.Property);
            });
            button.text = "Enter";
            button.AddToClassList("EnterButton");
            Node.titleButtonContainer.Add(button);
        }
    }
}