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
        public SerializedProperty StatesProperty => NodeManager.StatesProperty;
        public readonly NodeManager NodeManager;
        
        public MovementGraphView(SerializedProperty layerProperty) {
            LayerProperty = new SerializedPropertyMovementLayer(layerProperty);
            NodeManager = new NodeManager(this, LayerProperty);
            NodeManager.LoadExistingNodes();

            AddGridBackground();
            AddManipulators();
            styleSheets.Add(LoadStyleSheet("MovementGraph.uss"));
            graphViewChanged = OnGraphViewChanged;
        }

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
                    if (element is not BaseNode node) continue;
                    NodeManager.UpdatePosition(node);
                }
            }

            if (graphViewChange.elementsToRemove != null) {
                foreach (GraphElement element in graphViewChange.elementsToRemove) {
                    switch (element) {
                        case BaseNode node:
                            NodeManager.DeleteNode(node);
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

        public BaseNode FindNode(int index, string identifier) {
            foreach (BaseNode node in NodeManager._nodes) {
                if (node is NamedNode named) {
                    if(!string.IsNullOrEmpty(identifier) && named.Identifier == identifier) return node;
                }
                else if (node.Index == index) return node;
            }
            Debug.LogWarning($"Could not find node with index {index} and identifier {identifier}" +
                             $"in layer {LayerProperty.Identifier}!");
            return null;
        }
        
        
    }
}