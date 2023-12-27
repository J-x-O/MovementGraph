using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace JescoDev.SmoothBrainStates.Editor {
    public partial class NodeManager {

        public ContextualMenuManipulator CreateDebugContextMenu() {
            return new ContextualMenuManipulator(menuEvent => {
                menuEvent.menu.AppendAction("Debug/Print ValidTransitions",
                    action => {
                        MovementEditorNode node = Nodes.FirstOrDefault(node => node.selected);
                        if (node == null) {
                            Debug.Log("No node selected");
                            return;
                        }
                        
                        Debug.Log("Start: " + node.StateObject.Identifier);
                        foreach (MovementEditorNode compare in Nodes) {
                            if (!node.StateObject.GetAllPorts().Any(port => port.HasTransition(compare.StateObject))) continue;
                            Debug.Log(node.StateObject.Identifier);
                                
                        }
                    });
                menuEvent.menu.AppendAction("Debug/Reload", action => View.Rebuild());
            });
        }
    }
}