using UnityEditor.Experimental.GraphView;

namespace JescoDev.MovementGraph.Editor.Nodes {
    public interface IConnectIn {
        public Port InputPort { get; set; }
    }

    public interface IConnectOut {
        public Port OutputPort { get; set; }
    }
    
    public static class ConnectExtension {
        public static void RebuildInput(this IConnectIn connectIn, BaseNode baseNode, string portName = "Input", Orientation orientation = Orientation.Horizontal) {
            baseNode.inputContainer.Clear();
            connectIn.InputPort = baseNode.InstantiatePort(orientation, Direction.Input, Port.Capacity.Multi, typeof(bool));
            connectIn.InputPort.portName = portName;
            baseNode.inputContainer.Add(connectIn.InputPort);
        }
        
        public static void RebuildOutput(this IConnectOut connectIn, BaseNode baseNode, string portName = "Output", Orientation orientation = Orientation.Horizontal) {
            baseNode.outputContainer.Clear();
            connectIn.OutputPort = baseNode.InstantiatePort(orientation, Direction.Output, Port.Capacity.Multi, typeof(bool));
            connectIn.OutputPort.portName = portName;
            baseNode.outputContainer.Add(connectIn.OutputPort);
        }
    }
}