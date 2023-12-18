using System;
using System.Collections.Generic;
using System.Linq;
using Editor.MovementEditor.PropertyUtility;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static Editor.MovementEditor.MovementLayerView;

namespace Editor.MovementEditor {
    public class MovementGraphView : GraphView {
        
        public readonly SerializedPropertyMovementLayer LayerProperty;
        public readonly NodeManager NodeManager;
        private Action _rebuild;
        
        public MovementGraphView(SerializedProperty layerProperty, Action rebuild) {
            LayerProperty = new SerializedPropertyMovementLayer(layerProperty);
            NodeManager = new NodeManager(this, LayerProperty);
            NodeManager.LoadExistingNodes();
            _rebuild = rebuild;
            AddGridBackground();
            AddManipulators();
            styleSheets.Add(LoadStyleSheet("MovementGraph.uss"));
            graphViewChanged += OnGraphViewChanged;
        }

        public void Rebuild() => _rebuild();

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) {

            List<Port> compatiblePorts = new List<Port>();
            foreach (Port port in ports) {
                if (startPort == port) continue;
                if (startPort.node == port.node) continue;
                if (startPort.direction == port.direction) continue;
                compatiblePorts.Add(port);
            }
            return compatiblePorts;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange) {
            if (graphViewChange.movedElements != null) {
                foreach (GraphElement element in graphViewChange.movedElements) {
                    if (element is not MovementEditorNode node) continue;
                    NodeManager.UpdatePosition(node);
                }
            }

            if (graphViewChange.elementsToRemove != null) {
                foreach (GraphElement element in graphViewChange.elementsToRemove) {
                    switch (element) {
                        case MovementEditorNode node:
                            NodeManager.DeleteNode(node);
                            break;
                        case Edge edge:
                            if (edge.input is BoundPort boundInput) boundInput.HandleDeletion(edge);
                            if (edge.output is BoundPort boundOutput) boundOutput.HandleDeletion(edge);
                            break;
                    }
                }
            }

            return graphViewChange;
        }
        
        private void AddManipulators() {
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }

        private void AddGridBackground() {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
            
            // Context menu for nodes should only be at the background
            VisualElement element = new VisualElement { name = "HitBoxContextMenu"};
            element.StretchToParentSize();
            element.AddManipulator(NodeManager.CreateNodeContextualMenu());
            element.AddManipulator(NodeManager.CreateDebugContextMenu());
            Insert(1, element);
        }

        public MovementEditorNode FindNode(string guid) {
            foreach (MovementEditorNode node in NodeManager._nodes) {
                if(node.Guid == guid) return node;
            }

            Debug.LogWarning($"Could not find node \"{guid}\" in layer \"{LayerProperty.Identifier}\"!");
            return null;
        }
        
        
    }
}