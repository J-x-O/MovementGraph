using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Codice.CM.Common.Merge;
using Editor.MovementEditor.PropertyUtility;
using Entities.Movement.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Port = UnityEditor.Experimental.GraphView.Port;

namespace Editor.MovementEditor {
    public class BoundPort : Port {

        public string Identifier => PortProperty.Identifier;
        
        public readonly MovementEditorNode MovementEditorNode;
        public readonly SerializedPropertyMovementPort PortProperty;
        private readonly SerializedProperty _baseNodeProperty;

        public bool IsInput => direction == Direction.Input;
        
        public static BoundPort Create(MovementEditorNode movementEditorNode, string portName, bool showName, Direction direction) {

            DefaultEdgeConnectorListener listener = new DefaultEdgeConnectorListener();
            BoundPort ele = new BoundPort(movementEditorNode, portName, showName, direction) {
                m_EdgeConnector = new EdgeConnector<Edge>(listener)
            };
            ele.AddManipulator(ele.m_EdgeConnector);
            return ele;
        }
        
        private BoundPort(MovementEditorNode movementEditorNode, string portName, bool showName, Direction direction)
            : base(Orientation.Horizontal, direction, Capacity.Multi, typeof(bool)) {
            MovementEditorNode = movementEditorNode;
            PortProperty = new SerializedPropertyMovementPort(movementEditorNode.State.FindPropertyRelative(portName));

            if (!showName) this.portName = "";
            else {
                portName = Regex.Replace(portName, @"<(.*)>k__BackingField", "$1");
                portName = portName.TrimStart('_');
                portName = char.ToUpper(portName[0]) + portName[1..];
                portName = Regex.Replace(portName, @"(\p{Lu})", " $1");
                this.portName = portName;
            }
        }

        public void LoadConnection() {
            List<SerializedPropertyTransition> toBeRemoved = new();
            foreach (SerializedPropertyTransition transition in PortProperty.GetTransitions()) {
               
                MovementEditorNode targetNode = MovementEditorNode.View.FindNode(transition.StateGuid);
                BoundPort targetPort = targetNode?.FindPort(transition.PortIdentifier);

                if (targetPort == null) {
                    toBeRemoved.Add(transition);
                    continue;
                }
                
                // check if we are already connected
                if(connections.Any(edge => edge.input == targetPort || edge.output == targetPort)) continue;
                MovementEditorNode.View.AddElement(ConnectTo(targetPort));
            }

            if (!toBeRemoved.Any()) return;
            toBeRemoved.Reverse();
            foreach (SerializedPropertyTransition transition in toBeRemoved) {
                PortProperty.RemoveTransition(transition);
            }
            PortProperty.ApplyModifiedProperties();
        }

        public override void Connect(Edge edge) {
            base.Connect(edge);
            
            BoundPort other = (IsInput ? edge.output : edge.input) as BoundPort; 
            if (other != null) PortProperty.AddTransition(other);
        }

        
        internal void HandleDeletion(Edge edge) {
            BoundPort other = (IsInput ? edge.output : edge.input) as BoundPort;
            if (other != null) HandleDeletion(other);;
        }
        internal void HandleDeletion(BoundPort other) {
            PortProperty.RemoveTransition(other);
            PortProperty.ApplyModifiedProperties();
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