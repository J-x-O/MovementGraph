using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JescoDev.MovementGraph.States;
using JescoDev.MovementGraph.StateTransition;
using Movement.States;
using UnityEditor;
using UnityEditor.Experimental.GraphView;


namespace Editor.MovementEditor {

    public abstract class BaseNode : Node {
        
        public GraphView View { get; protected set; }
        public SerializedProperty State { get; protected set; }
        
        public State StateObject { get; protected set; }

        protected BaseNode(GraphView view, SerializedProperty state, State stateObject) {
            View = view;
            State = state;
            StateObject = stateObject;

            Rebuild();
        }

        public void Rebuild(SerializedProperty property) {
            State = property;
            Rebuild();
        }
        
        public void Rebuild() {
            List<FieldInfo> fieldInfos = StateObject.GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .OrderBy(field => field.MetadataToken)
                .ToList();
            
            fieldInfos.RemoveAll(element => element.Name is "_position");
            List<FieldInfo> ports = fieldInfos.Where(element => typeof(MovementPort).IsAssignableFrom(element.FieldType)).ToList();
            List<FieldInfo> inputPorts = ports.Where(element => element.IsInputPort()).ToList();
            List<FieldInfo> outputPorts = ports.Where(element => element.IsOutputPort()).ToList();
            RebuildPorts(inputPorts, outputPorts);

            Rebuild(fieldInfos);
            
            mainContainer.AddToClassList("NodeMainContainer");
            extensionContainer.AddToClassList("NodeExtensionContainer");
            RefreshExpandedState();
        }

        private void RebuildPorts(List<FieldInfo> inputPorts, List<FieldInfo> outputPorts) {
            inputContainer.Clear();
            bool multiple = inputPorts.Count > 1;
            foreach (FieldInfo port in inputPorts) {
                inputContainer.Add(new BoundPort(this, port.Name, multiple, Direction.Input));
            }
            
            outputContainer.Clear();
            multiple = outputPorts.Count > 1;
            foreach (FieldInfo port in outputPorts) {
                outputContainer.Add(new BoundPort(this, port.Name, multiple, Direction.Output));
            }
        }

        protected abstract void Rebuild(List<FieldInfo> fieldInfos);

        public void LoadConnections() {
            foreach (BoundPort port in InputPorts) port.LoadConnection();
            foreach (BoundPort port in OutputPorts) port.LoadConnection();
        }
        
        public IEnumerable<BoundPort> InputPorts => inputContainer.Children().OfType<BoundPort>();
        public IEnumerable<BoundPort> OutputPorts => outputContainer.Children().OfType<BoundPort>();
    }
    
    
}