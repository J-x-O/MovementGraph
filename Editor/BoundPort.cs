using System;
using System.Collections.Generic;
using System.Linq;
using Codice.CM.Common.Merge;
using JescoDev.MovementGraph.States;
using Movement.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Port = UnityEditor.Experimental.GraphView.Port;

namespace Editor.MovementEditor {
    public class BoundPort : Port, IEdgeConnectorListener {
        
        private readonly SerializedProperty _baseProperty;
        private readonly SerializedProperty _portProperty;

        public BoundPort(SerializedProperty baseProperty, string portName, bool showName, Direction direction)
            : base(Orientation.Horizontal, direction, Capacity.Multi, typeof(bool)) {
            _baseProperty = baseProperty;
            _portProperty = baseProperty.FindPropertyRelative(portName);
            this.portName = showName ? portName : "";
            m_EdgeConnector = new EdgeConnector<Edge>(this);
            this.AddManipulator(m_EdgeConnector);
        }

        public void LoadConnection(List<BaseNode> allNodes) {
            for (int i = 0; i < _portProperty.arraySize; i++) {
                SerializedProperty element = _portProperty.GetArrayElementAtIndex(i);
                State target = element.managedReferenceValue as State;
                if (target == null) continue;
                
                BaseNode targetNode = allNodes.Find(baseNode => baseNode.StateObject == target);
                if (targetNode == null) continue;
                
                Port start = FindPort(this, Direction.Output);
                Port end = FindPort(targetNode, Direction.Input);
                if (start == null || end == null) continue;
                _view.AddElement(start.ConnectTo(end));
            }
        }
        
        private static Port FindPort(Node node, Direction target) {
            return node switch {
                IConnectOut outNode when target == Direction.Output => outNode.OutputPort,
                IConnectIn inNode when target == Direction.Input => inNode.InputPort,
                _ => null
            };
        }

        public void OnDropOutsidePort(Edge edge, Vector2 position) { }

        public void OnDrop(GraphView graphView, Edge edge) {
            graphView.AddElement(edge);
            edge.input.Connect(edge);
            edge.output.Connect(edge);
            edge.RegisterCallback<DetachFromPanelEvent>(HandleDeletion);
            
            _portProperty.AppendArrayElement(element => {
                State target = (edge.input.node as BaseNode)?.StateObject;
                element.managedReferenceValue = target;
            });
        }

        private void HandleDeletion(DetachFromPanelEvent evt) {
            if(evt.target is not Edge edge) return;
            if(edge.input.node is not BaseNode input) return;
            if(edge.output.node is not BaseNode output) return;
            
            // find the correct element to remove
            _portProperty.RemoveArrayElement(element => {
                if(element.managedReferenceValue is not Transition transition) return false;
                return transition.From.State == input.StateObject
                       && transition.To.State == output.StateObject;
            });
        }
    }
}