using System.Collections.Generic;
using JescoDev.MovementGraph.Editor.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace JescoDev.MovementGraph.Editor {
    public class MovementGraphView : GraphView {

        public const string ResourcePath = "Packages/com.j-x-o.movement-graph/EditorResources";
        
        private readonly MovementSystem _link;
        private readonly SerializedObject _linkObject;
        private readonly MovementGraph.Editor.NodeManager _nodeManager;
        
        public MovementGraphView(MovementSystem link) {
            _link = link;
            _linkObject = _link ? new SerializedObject(_link) : null;
            _nodeManager = new MovementGraph.Editor.NodeManager(this, _linkObject);
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
                        case Edge edge:
                            if (edge.output.node is not (IConnectOut and BaseNode start)) break;
                            if (edge.input.node is not (IConnectIn and BaseNode end)) break;
                            MovementGraph.Editor.NodeManager.RemoveConnection(start, end);

                            break;
                    }
                }
            }
            
            if (graphViewChange.edgesToCreate != null) {
                foreach (Edge edge in graphViewChange.edgesToCreate) {
                    if (edge.output.node is not (IConnectOut and BaseNode start)) break;
                    if (edge.input.node is not (IConnectIn and BaseNode end)) break;
                    MovementGraph.Editor.NodeManager.CreateConnection(start, end);
                }
            }
            
            return graphViewChange;
        }
        
        private void AddManipulators() {
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new ContentZoomer());
            CameraBinder binder = new CameraBinder(_linkObject);
            this.AddManipulator(binder);
            binder.RestoreCamera();
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
            StyleSheet graphSheet = (StyleSheet) EditorGUIUtility.Load($"{ResourcePath}/MovementGraph.uss");
            styleSheets.Add(graphSheet);
            StyleSheet nodeSheet = (StyleSheet) EditorGUIUtility.Load($"{ResourcePath}/NodeStyles.uss");
            styleSheets.Add(nodeSheet);
        }
    }
}