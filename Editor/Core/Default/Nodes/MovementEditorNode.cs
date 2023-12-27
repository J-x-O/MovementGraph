using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JescoDev.SmoothBrainStates.States;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace JescoDev.SmoothBrainStates.Editor {

    public class MovementEditorNode : Node {
        
        public DefaultGraphView View { get; protected set; }
        public SerializedPropertyState State { get; protected set; }
        public State StateObject { get; protected set; }
        
        public string Identifier => State.Identifier;
        public string Guid => State.Guid;

        private List<NodeElement> _elements;

        public MovementEditorNode(DefaultGraphView view, SerializedPropertyState state, State stateObject) {
            View = view;
            State = state;
            StateObject = stateObject;
            _elements = ReflectionUtility.GetAllInheritors<NodeElement>()
                .MoveToBack(typeof(NodeElementVariables))
                .Select(type =>
                    (NodeElement)type.GetConstructor(new[] { typeof(MovementEditorNode) })
                        ?.Invoke(new object[] { this }))
                .Where(instance => instance != null && instance.CanBeApplied())
                .ToList();
            this.AddManipulator(new OpenScriptManipulator(StateObject.GetType()));
            Rebuild();
        }

        public void Rebuild() {
            
            List<FieldInfo> fieldInfos = StateObject.GetType()
                .ExtractFields()
                .ToList();
            
            fieldInfos.RemoveAll(element => element.Name is "_position" or "_guid");

            foreach (NodeElement nodeElement in _elements) {
                nodeElement.Rebuild(fieldInfos);
            }
            
            mainContainer.AddToClassList("NodeMainContainer");
            mainContainer.AddToClassList(StateObject.GetType().Name);
            extensionContainer.AddToClassList("NodeExtensionContainer");
            RefreshExpandedState();
        }

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
                + $"in layer {View.StateProperty.Identifier}!";
            Debug.LogWarning(log);
            return null;
        }
        
    }
    
    
}