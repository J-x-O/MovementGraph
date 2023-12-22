using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JescoDev.MovementGraph.Editor;
using JescoDev.SmoothBrainStates.StateTransition;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace JescoDev.SmoothBrainStates.Editor {
    public class NodeElementPorts : NodeElement {

        public NodeElementPorts(MovementEditorNode node) : base(node) { }
        
        public override bool CanBeApplied() {
            return true;
        }

        public override void Rebuild(List<FieldInfo> fieldInfos) {
            List<FieldInfo> ports = fieldInfos.Where(IsMovementPort).ToList();
            List<FieldInfo> inputPorts = ports.Where(element => element.IsInputPort()).ToList();
            List<FieldInfo> outputPorts = ports.Where(element => element.IsOutputPort()).ToList();
            RebuildPorts(Node.inputContainer, inputPorts, Direction.Input);
            RebuildPorts(Node.outputContainer, outputPorts, Direction.Output);

            fieldInfos.RemoveAll(IsMovementPort);
        }
        
        private static bool IsMovementPort(FieldInfo element)
            => typeof(SmoothPort).IsAssignableFrom(element.FieldType);
        
        private void RebuildPorts(VisualElement target, List<FieldInfo> ports, Direction direction) {
            bool multiple = ports.Count > 1;
            foreach (FieldInfo port in ports) {
                target.Add(BoundPort.Create(Node, port.Name, multiple, direction));
            }
        }
    }
}