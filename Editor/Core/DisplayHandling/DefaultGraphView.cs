using System.Collections.Generic;
using JescoDev.MovementGraph.Editor;
using JescoDev.MovementGraph.Editor.DisplayHandling;
using JescoDev.MovementGraph.Editor.Utility;
using JescoDev.SmoothBrainStates.Editor;
using JescoDev.SmoothBrainStates.Editor.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.MovementEditor {
    
    public class DefaultGraphViewHandler : DisplayHandler {
        
        public override bool IsValid(SerializedProperty target)
            => new SerializedPropertyStateParent(target).IsValid();
        
        public override VisualElement CreateDisplay(SmoothBrainStatesRoot root, SerializedProperty target) {
            return new DefaultGraphView(root.Tracker, target);
        }
    }
    
    public class DefaultGraphView : GraphView {
        
        public readonly PathTracker Tracker;
        public readonly SerializedPropertyStateParent LayerProperty;
        public readonly NodeManager NodeManager;
        
        public DefaultGraphView(PathTracker tracker, SerializedProperty parentProperty) {
            Tracker = tracker;
            LayerProperty = new SerializedPropertyStateParent(parentProperty);
            NodeManager = new NodeManager(this, LayerProperty);
            NodeManager.LoadExistingNodes();
            AddGridBackground();
            AddManipulators();
            styleSheets.Add(Loading.LoadStyleSheet("MovementGraph.uss"));
            graphViewChanged += OnGraphViewChanged;
        }

        public void Rebuild() {
            using RebuildEvent e = RebuildEvent.GetPooled();
            SendEvent(e);
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
            foreach (MovementEditorNode node in NodeManager.Nodes) {
                if(node.Guid == guid) return node;
            }

            Debug.LogWarning($"Could not find node \"{guid}\" in layer \"{LayerProperty.Identifier}\"!");
            return null;
        }
        
        
    }
}