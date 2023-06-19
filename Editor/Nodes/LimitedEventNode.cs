using System.Collections.Generic;
using System.Reflection;
using Editor.MovementEditor.PropertyUtility;
using Entities.Movement.States;
using JescoDev.MovementGraph.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

namespace Editor.MovementEditor {
    public class LimitedEventNode : NamedNode {
        
        public new LimitedEventState StateObject { get; protected set; }

        public LimitedEventNode(MovementGraphView view, SerializedPropertyState state, LimitedEventState stateObject)
            : base(view, state, stateObject) {
            StateObject = stateObject;
        }
        protected override void Rebuild(List<FieldInfo> fieldInfos) {
            RebuildHeader(fieldInfos);
        }
    }
}