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

        public string PropertyName => PortProperty.name;
        
        public readonly BaseNode BaseNode;
        public readonly SerializedProperty PortProperty;

        private bool IsInput => direction == Direction.Input;
        
        public BoundPort(BaseNode baseNode, string portName, bool showName, Direction direction)
            : base(Orientation.Horizontal, direction, Capacity.Multi, typeof(bool)) {
            BaseNode = baseNode;
            PortProperty = baseNode.State.FindPropertyRelative(portName);
            this.portName = showName ? portName : "";
            m_EdgeConnector = new EdgeConnector<Edge>(this);
            this.AddManipulator(m_EdgeConnector);
        }

        public void LoadConnection() {
            List<BaseNode> nodes = BaseNode.View.nodes.OfType<BaseNode>().ToList();
            SerializedProperty transitions = PortProperty.FindPropertyRelative("_transitions");
            for (int i = 0; i < transitions.arraySize; i++) {
                SerializedProperty transition = PortProperty.GetArrayElementAtIndex(i);
                
                // find connected State
                SerializedProperty other = transition.FindPropertyRelative(IsInput ? "_from" : "_to");
                MovementPort otherPort = other.managedReferenceValue as MovementPort;
                State otherNode = otherPort?.State;
                if (otherNode == null) {
                    PortProperty.DeleteArrayElementAtIndex(i);
                    i--;
                    continue;
                }
                
                // remap state to visual element
                BaseNode targetNode = nodes.Find(compare => compare.StateObject == otherNode);
                BoundPort targetPort = (IsInput ? targetNode.OutputPorts : targetNode.InputPorts)
                    .FirstOrDefault(port => SerializedProperty.EqualContents(port.PortProperty, other));
                if (targetPort == null) {
                    PortProperty.DeleteArrayElementAtIndex(i);
                    i--;
                    continue;
                }
                
                BaseNode.View.AddElement(ConnectTo(targetPort));
            }
        }

        public void OnDropOutsidePort(Edge edge, Vector2 position) { }

        public void OnDrop(GraphView graphView, Edge edge) {
            graphView.AddElement(edge);
            edge.input.Connect(edge);
            edge.output.Connect(edge);
            edge.RegisterCallback<DetachFromPanelEvent>(HandleDeletion);
            
            PortProperty.AppendArrayElement(element => {
                State target = (edge.input.node as BaseNode)?.StateObject;
                element.managedReferenceValue = target;
            });
        }

        private void HandleDeletion(DetachFromPanelEvent evt) {
            if(evt.target is not Edge edge) return;
            if(edge.input.node is not BaseNode input) return;
            if(edge.output.node is not BaseNode output) return;
            
            // find the correct element to remove
            PortProperty.RemoveArrayElement(element => {
                if(element.managedReferenceValue is not Transition transition) return false;
                return transition.From.State == input.StateObject
                       && transition.To.State == output.StateObject;
            });
        }
    }
}