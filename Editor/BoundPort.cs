using System;
using System.Collections.Generic;
using System.Linq;
using Codice.CM.Common.Merge;
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

        public string PropertyName => PortProperty.name;
        
        public readonly BaseNode BaseNode;
        public readonly MovementPort PortInstance;
        public readonly SerializedProperty PortProperty;
        private readonly SerializedProperty _transitionProperty;
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
            PortProperty = baseNode.State.FindPropertyRelative(portName);
            PortProperty.managedReferenceValue ??= new MovementPort();
            PortInstance = PortProperty.managedReferenceValue as MovementPort;
            
            _transitionProperty = PortProperty.FindPropertyRelative("_transitions");
            _baseNodeProperty = PortProperty.FindPropertyRelative("_state");
            _baseNodeProperty.managedReferenceValue = baseNode.StateObject;
            PortProperty.serializedObject.ApplyModifiedProperties();
            
            this.portName = showName ? portName : "";
        }

        public void LoadConnection() {
            List<BaseNode> nodes = BaseNode.View.nodes.OfType<BaseNode>().ToList();
            
            for (int i = 0; i < _transitionProperty.arraySize; i++) {
                SerializedProperty transition = _transitionProperty.GetArrayElementAtIndex(i);
                
                // find connected State
                SerializedProperty other = transition.FindPropertyRelative(IsInput ? "_from" : "_to");
                MovementPort otherPort = other.managedReferenceValue as MovementPort;
                State otherNode = otherPort?.State;
                if (otherNode == null) {
                    _transitionProperty.DeleteArrayElementAtIndex(i);
                    continue;
                }
                
                // remap state to visual element
                BaseNode targetNode = nodes.Find(compare => compare.StateObject == otherNode);
                BoundPort targetPort = (IsInput ? targetNode.OutputPorts : targetNode.InputPorts)
                    .FirstOrDefault(port => port.PortInstance == otherPort);
                if (targetPort == null) {
                    _transitionProperty.DeleteArrayElementAtIndex(i);
                    continue;
                }
                
                BaseNode.View.AddElement(ConnectTo(targetPort));
            }

            PortProperty.serializedObject.ApplyModifiedProperties();
        }

        public override void Connect(Edge edge) {
            base.Connect(edge);
            //edge.RegisterCallback<DetachFromPanelEvent>(HandleDeletion);
            
            BoundPort other = (IsInput ? edge.output : edge.input) as BoundPort; 
            if (other != null) AppendEdge(other.PortInstance);
        }

        private void HandleDeletion(DetachFromPanelEvent evt) {
            if(evt.target is not Edge edge) return;
            
            BoundPort other = (IsInput ? edge.output : edge.input) as BoundPort;
            if (other != null)  DeleteEdge(other.PortInstance);
        }
        
        private void AppendEdge(MovementPort to) {
            if(_transitionProperty.Any(element => IsConnectionTo(element, to))) return;
            _transitionProperty.AppendArrayElement(element => {
                element.FindPropertyRelative("_from").managedReferenceValue = PortInstance;
                element.FindPropertyRelative("_to").managedReferenceValue = to;
            });
        }
        
        private void DeleteEdge(MovementPort to) {
            _transitionProperty.RemoveArrayElement(element => IsConnectionTo(element, to));
        }

        private bool IsConnectionTo(SerializedProperty element, MovementPort to) {
            if(element.FindPropertyRelative("_from").managedReferenceValue is not MovementPort fromCompare) return false;
            if(element.FindPropertyRelative("_to").managedReferenceValue is not MovementPort toCompare) return false;
            return fromCompare == PortInstance && toCompare == to;
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