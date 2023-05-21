using System;
using System.Reflection;

namespace JescoDev.MovementGraph.StateTransition {

    public class PortType : Attribute {

        public bool IsOutput => !IsInput;

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


    public static class PortExtensions {

        public static bool IsInputPort(this FieldInfo field)
            => field.GetCustomAttribute<PortType>()?.IsInput ?? false;

        public static bool IsOutputPort(this FieldInfo field)
            => field.GetCustomAttribute<PortType>()?.IsOutput ?? false;
    }
}