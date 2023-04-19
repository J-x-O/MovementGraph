using System;
using System.Collections.Generic;
using Movement.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.MovementEditor {
    public class BoundOutputPort : Port, IEdgeConnectorListener {
        
        private readonly SerializedProperty _property;

        public BoundOutputPort(SerializedProperty property, Orientation orientation, Capacity capacity, Type type)
            : base(orientation, Direction.Output, capacity, type) {
            _property = property;
            m_EdgeConnector = new EdgeConnector<Edge>(this);
        }

        public void LoadConnection() {
            for (int i = 0; i < _property.arraySize; i++) {
                SerializedProperty element = _property.GetArrayElementAtIndex(i);
                State target = element.managedReferenceValue as State;
                if (target == null) continue;
                BaseNode targetNode = _nodes.Find(element => element.StateObject == target);
                if (targetNode == null) continue;
                Port start = FindPort(this, Direction.Output);
                Port end = FindPort(targetNode, Direction.Input);
                if (start == null || end == null) continue;
                _view.AddElement(start.ConnectTo(end));
            }
        }

        public void RemoveConnection(Edge edge) {
            
        }

        public void OnDropOutsidePort(Edge edge, Vector2 position) { }

        public void OnDrop(GraphView graphView, Edge edge) {
            graphView.AddElement(edge);
            edge.input.Connect(edge);
            edge.output.Connect(edge);
            
            SerializedPropertyUtility.AppendArrayElement(_property, element => {
                State target = (edge.input.node as BaseNode)?.StateObject;
                element.managedReferenceValue = target;
            });
        }
    }
}