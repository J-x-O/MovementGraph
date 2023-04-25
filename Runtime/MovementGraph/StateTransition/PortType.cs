using System;

namespace JescoDev.MovementGraph.MovementGraph.StateTransition {
    
    public class PortType : Attribute {
        
        public readonly bool IsInput;
        
        public PortType(bool isInput) {
            IsInput = isInput;
        }
    }
    
    [AttributeUsage(AttributeTargets.Field)]
    public class InputPort : PortType {
        public InputPort() : base(true) { }
    }
    
    [AttributeUsage(AttributeTargets.Field)]
    public class OutputPort : PortType {
        public OutputPort() : base(false) { }
    }

    
}