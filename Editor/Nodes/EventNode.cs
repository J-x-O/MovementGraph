using System;
using System.Collections.Generic;
using System.Reflection;
using JescoDev.MovementGraph.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

namespace JescoDev.MovementGraph.Editor.Nodes {
    
    [Serializable]
    public class EventNode : NamedNode, IConnectOut {
        
        public new EventState StateObject { get; protected set; }
        
        public Port OutputPort { get; set; }

        public EventNode(SerializedProperty state, EventState stateObject) : base(state, stateObject) {
            StateObject = stateObject;
        }

        protected override void Rebuild(List<FieldInfo> fieldInfos) {
            RebuildHeader(fieldInfos);
            NodeHeader.RegisterValueChangeCallback(TestForSpecialName);
            this.RebuildOutput(this);
        }

        private void TestForSpecialName(SerializedPropertyChangeEvent evt) {
            mainContainer.RemoveFromClassList("ReevaluateContainer");
            mainContainer.RemoveFromClassList("AnyStateContainer");

            switch (evt.changedProperty.stringValue) {
                case "Reevaluate":
                    mainContainer.AddToClassList("ReevaluateContainer");
                    break;
                case "Any State":
                    mainContainer.AddToClassList("AnyStateContainer");
                    break;
            }
        }
    }
}