using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Movement;
using Entities.Movement.States;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace Editor.MovementEditor {
    public class MovementGraphView : GraphView {
        
        private readonly SerializedProperty _layer;
        private readonly NodeManager _nodeManager;
        
        public MovementGraphView(SerializedProperty layer) {
            _layer = layer;
            _nodeManager = new NodeManager(this, _layer);
            _nodeManager.LoadExistingNodes();

            AddGridBackground();
            AddStyles();
            AddManipulators();
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
                    _nodeManager.UpdatePosition(node);
                }
            }

            if (graphViewChange.elementsToRemove != null) {
                foreach (GraphElement element in graphViewChange.elementsToRemove) {
                    switch (element) {
                        case BaseNode node:
                            _nodeManager.DeleteNode(node);
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
            element.AddManipulator(_nodeManager.CreateNodeContextualMenu());
            element.AddManipulator(_nodeManager.CreateDebugContextMenu());
            Insert(1, element);
        }

        private void AddStyles() {
            styleSheets.Add(LoadStyleSheet("MovementGraph.uss"));
            styleSheets.Add(LoadStyleSheet("NodeStyles.uss"));
        }

        public static StyleSheet LoadStyleSheet(string localPath) {
            const string resourcePath = "Packages/com.j-x-o.movement-graph/EditorResources/";
            return (StyleSheet)EditorGUIUtility.Load(resourcePath + localPath);
        }
    }
}