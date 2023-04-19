using System.Collections.Generic;
using System.Reflection;
using JescoDev.MovementGraph.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

namespace JescoDev.MovementGraph.Editor.Nodes {
    public class LimitedEventNode : NamedNode, IConnectIn, IConnectOut {
        
        public new LimitedEventState StateObject { get; protected set; }
        
        public Port InputPort { get; set; }
        public Port OutputPort { get; set; }

        public LimitedEventNode(SerializedProperty state, LimitedEventState stateObject) : base(state, stateObject) {
            StateObject = stateObject;
        }
        protected override void Rebuild(List<FieldInfo> fieldInfos) {
            RebuildHeader(fieldInfos);
            this.RebuildInput(this);
            this.RebuildOutput(this);
        }
    }
}