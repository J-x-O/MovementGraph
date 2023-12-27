using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace JescoDev.SmoothBrainStates.Editor {
    
    public class DefaultGraphViewHandler : ISmoothInspector {
        
        public bool IsValid(SerializedObjectOrProperty target)
            => new SerializedPropertyStateParent(target).IsValid();
        
        public VisualElement CreateDisplay(SmoothBrainStatesRoot root, SerializedObjectOrProperty target) {
            return new DefaultGraphView(root, target);
        }
    }
    
    public class DefaultGraphView : GraphView {
        
        public readonly SmoothBrainStatesRoot Root;
        public readonly SerializedPropertyStateParent LayerProperty;
        public string Identifier => StateProperty.IsValid() ? StateProperty.Identifier : "Root";
        public readonly SerializedPropertyState StateProperty;
        public readonly NodeManager NodeManager;

        public DefaultGraphView(SmoothBrainStatesRoot root, SerializedObjectOrProperty parentProperty) {
            Root = root;
            LayerProperty = new SerializedPropertyStateParent(parentProperty);
            StateProperty = new SerializedPropertyState(parentProperty);
            StateProperty = new SerializedPropertyState(parentProperty);
            NodeManager = new NodeManager(this, LayerProperty);
            NodeManager.LoadExistingNodes();
            AddGridBackground();
            AddManipulators();
            styleSheets.Add(Loading.LoadStyleSheet("MovementGraph.uss"));
            graphViewChanged += OnGraphViewChanged;
        }

        public void Rebuild() {
            using RebuildEvent evt = RebuildEvent.GetPooled();
            evt.target = this; 
            SendEvent(evt);
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

            Debug.LogWarning($"Could not find node \"{guid}\" in State \"{Identifier}\"!");
            return null;
        }
        
        
    }
}