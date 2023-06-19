using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Editor.MovementEditor.PropertyUtility;
using JescoDev.MovementGraph.States;
using JescoDev.MovementGraph.StateTransition;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


namespace Editor.MovementEditor {

    public abstract class BaseNode : Node {
        
        public MovementGraphView View { get; protected set; }
        public SerializedPropertyState State { get; protected set; }
        public State StateObject { get; protected set; }
        
        public string Identifier => State.Identifier;

        protected BaseNode(MovementGraphView view, SerializedPropertyState state, State stateObject) {
            View = view;
            State = state;
            StateObject = stateObject;
            Rebuild();
        }

        public void Rebuild(SerializedPropertyState property) {
            State = property;
            Rebuild();
        }
        
        public void Rebuild() {
            List<FieldInfo> fieldInfos = StateObject.GetType()
                .ExtractFields()
                .ToList();
            
            fieldInfos.RemoveAll(element => element.Name is "_position");
            
            List<FieldInfo> ports = fieldInfos.Where(IsMovementPort).ToList();
            List<FieldInfo> inputPorts = ports.Where(element => element.IsInputPort()).ToList();
            List<FieldInfo> outputPorts = ports.Where(element => element.IsOutputPort()).ToList();
            RebuildPorts(inputPorts, outputPorts);

            fieldInfos.RemoveAll(IsMovementPort);
            Rebuild(fieldInfos);
            
            mainContainer.AddToClassList("NodeMainContainer");
            extensionContainer.AddToClassList("NodeExtensionContainer");
            RefreshExpandedState();
        }

        private static bool IsMovementPort(FieldInfo element)
            => typeof(MovementPort).IsAssignableFrom(element.FieldType);

        private void RebuildPorts(List<FieldInfo> inputPorts, List<FieldInfo> outputPorts) {
            inputContainer.Clear();
            bool multiple = inputPorts.Count > 1;
            foreach (FieldInfo port in inputPorts) {
                inputContainer.Add(BoundPort.Create(this, port.Name, multiple, Direction.Input));
            }
            
            outputContainer.Clear();
            multiple = outputPorts.Count > 1;
            foreach (FieldInfo port in outputPorts) {
                outputContainer.Add(BoundPort.Create(this, port.Name, multiple, Direction.Output));
            }
        }

        protected abstract void Rebuild(List<FieldInfo> fieldInfos);

        public void LoadConnections() {
            foreach (BoundPort port in InputPorts) port.LoadConnection();
            foreach (BoundPort port in OutputPorts) port.LoadConnection();
        }
        
        public IEnumerable<BoundPort> Ports => InputPorts.Concat(OutputPorts);
        public IEnumerable<BoundPort> InputPorts => inputContainer.Children().OfType<BoundPort>();
        public IEnumerable<BoundPort> OutputPorts => outputContainer.Children().OfType<BoundPort>();

        public BoundPort FindPort(string portIdentifier) {

            foreach (BoundPort inputPort in InputPorts.Concat(OutputPorts))
                if (inputPort.Identifier == portIdentifier) 
                    return inputPort;

            string log = $"Could not find requested port \"{portIdentifier}\" in node \"{Identifier}\" "
                + $"in layer {View.LayerProperty.Identifier}!";
            Debug.LogWarning(log);
            return null;
        }
        
    }
    
    
}