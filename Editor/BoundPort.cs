using System;
using System.Collections.Generic;
using System.Linq;
using Codice.CM.Common.Merge;
using Editor.MovementEditor.PropertyUtility;
using Entities.Movement.States;
using JescoDev.MovementGraph.States;
using JescoDev.MovementGraph.StateTransition;
using Movement.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Port = UnityEditor.Experimental.GraphView.Port;

namespace Editor.MovementEditor {
    public class BoundPort : Port {

        public string Identifier => PortProperty.Identifier;
        
        public readonly BaseNode BaseNode;
        public readonly SerializedPropertyMovementPort PortProperty;
        private readonly SerializedProperty _baseNodeProperty;

        private bool IsInput => direction == Direction.Input;
        
        public static BoundPort Create(BaseNode baseNode, string portName, bool showName, Direction direction) {
            DefaultEdgeConnectorListener listener = new DefaultEdgeConnectorListener();
            BoundPort ele = new BoundPort(baseNode, portName, showName, direction) {
                m_EdgeConnector = new EdgeConnector<Edge>(listener)
            };
            ele.AddManipulator(ele.m_EdgeConnector);
            return ele;
        }
        
        private BoundPort(BaseNode baseNode, string portName, bool showName, Direction direction)
            : base(Orientation.Horizontal, direction, Capacity.Multi, typeof(bool)) {
            BaseNode = baseNode;
            PortProperty = new SerializedPropertyMovementPort(baseNode.State.FindPropertyRelative(portName));
            this.portName = showName ? portName : "";
        }
        
        

        public void LoadConnection() {

            foreach (SerializedPropertyTransition transition in PortProperty.GetTransitions()) {
               
                BaseNode targetNode = BaseNode.View.FindNode(transition.StateIndex, transition.StateIdentifier);
                BoundPort targetPort = targetNode?.FindPort(transition.PortIdentifier);
                if (targetPort == null) {
                    PortProperty.RemoveTransition(transition);
                    continue;
                }
                BaseNode.View.AddElement(ConnectTo(targetPort));
            }

            PortProperty.ApplyModifiedProperties();
        }

        public override void Connect(Edge edge) {
            base.Connect(edge);
            //edge.RegisterCallback<DetachFromPanelEvent>(HandleDeletion);
            
            BoundPort other = (IsInput ? edge.output : edge.input) as BoundPort; 
            if (other != null) AppendEdge(other);
        }

        private void HandleDeletion(DetachFromPanelEvent evt) {
            if(evt.target is not Edge edge) return;
            
            BoundPort other = (IsInput ? edge.output : edge.input) as BoundPort;
            if (other != null) DeleteEdge(other);
        }
        
        private void AppendEdge(BoundPort to) {
            if(PortProperty.HasTransitionTo(to)) return;
            PortProperty.AddTransition(to);
        }
        
        private void DeleteEdge(BoundPort to) {
            PortProperty.RemoveTransition(to.BaseNode.GetIdentifier(), to.BaseNode.Index, to.Identifier);
        }

        private class DefaultEdgeConnectorListener : IEdgeConnectorListener {
            private readonly GraphViewChange _mGraphViewChange;
            private readonly List<Edge> _mEdgesToCreate;

            public DefaultEdgeConnectorListener() {
                _mEdgesToCreate = new List<Edge>();
                _mGraphViewChange.edgesToCreate = _mEdgesToCreate;
            }

            public void OnDropOutsidePort(Edge edge, Vector2 position) { }

            public void OnDrop(GraphView graphView, Edge edge) {
                _mEdgesToCreate.Clear();
                _mEdgesToCreate.Add(edge);

                List<Edge> edgesToCreate = _mEdgesToCreate;
                if (graphView.graphViewChanged != null)
                    edgesToCreate = graphView.graphViewChanged(_mGraphViewChange).edgesToCreate;
                foreach (Edge edge1 in edgesToCreate) {
                    graphView.AddElement(edge1);
                    edge.input.Connect(edge1);
                    edge.output.Connect(edge1);
                }
            }
        }
    }
}