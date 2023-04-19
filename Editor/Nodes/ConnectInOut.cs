using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor.MovementEditor {

    public static class ConnectExtension {

        public static bool HasInput(this Node node, out Port port) {
            port = node.inputContainer.Query<Port>().First();
            return port != null;
        }
        
        public static bool HasOutput(this Node node, out Port port) {
            port = node.outputContainer.Query<Port>().First();
            return port != null;
        }

        public static void RebuildInput(this Node baseNode, string portName = "Input") {
            baseNode.inputContainer.Clear();
            BuildInput(baseNode, portName);
        }

        public static void RebuildInputs(this Node baseNode, params string[] portNames) {
            baseNode.inputContainer.Clear();
            foreach (string name in portNames) {
                BuildInput(baseNode, name);
            }
        }
        
        public static void BuildInput(this Node baseNode, string portName = "Input") {
            Port port = baseNode.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            port.portName = portName;
            baseNode.inputContainer.Add(port);
        }

        public static void RebuildOutput(this Node baseNode, string portName = "Output") {
            baseNode.outputContainer.Clear();
            BuildOutput(baseNode, portName);
        }
        
        public static void RebuildOutputs(this Node baseNode, params string[] portNames) {
            baseNode.outputContainer.Clear();
            foreach (string name in portNames) {
                BuildOutput(baseNode, name);
            }
        }

        public static void BuildOutput(this Node baseNode, string portName = "Output") {
            Port port = baseNode.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            port.portName = portName;
            baseNode.outputContainer.Add(port);
        }


    }
}