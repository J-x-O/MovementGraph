using System.Collections.Generic;
using System.Reflection;
using JescoDev.MovementGraph.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

namespace JescoDev.MovementGraph.Editor.Nodes {
    public class RedirectNode : BaseNode, IConnectIn, IConnectOut {
        
        public Port InputPort { get; set; }
        public Port OutputPort { get; set; }

        public RedirectNode(SerializedProperty state, RedirectState stateObject) : base(state, stateObject) { }

        protected override void Rebuild(List<FieldInfo> fieldInfos) {
            this.RebuildInput(this, "");
            this.RebuildOutput(this, "");
            mainContainer.AddToClassList("RedirectNode");
            titleContainer.Clear();
        }
    }
}