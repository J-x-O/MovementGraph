using System.Linq;
using Entities.Movement.States;
using UnityEngine;
using UnityEngine.UIElements;
using Utility;

namespace Editor.MovementEditor {
    public partial class NodeManager {

        public ContextualMenuManipulator CreateDebugContextMenu() {
            return new ContextualMenuManipulator(menuEvent => {
                menuEvent.menu.AppendAction("Debug/Print ValidTransitions",
                    action => {
                        BaseNode node = FindSelectedNode();
                        if (node == null) {
                            Debug.Log("No node selected");
                            return;
                        }
                        
                        if(node.StateObject is NamedState namedStart) Debug.Log("Start: " + namedStart.Identifier);
                        foreach (BaseNode compare in _nodes) {
                            if (!node.StateObject.HasTransitionTo(compare.StateObject)) continue;
                            if (compare.StateObject is NamedState namedCompare)
                                Debug.Log(namedCompare.Identifier);
                        }
                    });
            });
        }

        private BaseNode FindSelectedNode() {
            return _nodes.FirstOrDefault(node => node.selected);
        }
        
    }
}