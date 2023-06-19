using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JescoDev.MovementGraph.States;
using UnityEngine;

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

        
        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance |
                                                  System.Reflection.BindingFlags.NonPublic |
                                                  System.Reflection.BindingFlags.Public;
        
        public static IEnumerable<FieldInfo> ExtractFields(this object obj) => ExtractFields(obj.GetType());
        public static IEnumerable<FieldInfo> ExtractFields(this Type target) {

            List<Type> inheritance = new() { target };
            while (target.BaseType != null) {
                target = target.BaseType;
                inheritance.Add(target);
            }
            inheritance.Reverse();
            
            List<FieldInfo> collected = new List<FieldInfo>();
            foreach (Type type in inheritance) {
                foreach (FieldInfo field in type.GetFields(BindingFlags)) {
                    if(collected.All(compare => compare.FieldHandle != field.FieldHandle)) collected.Add(field);
                }
            }
            return collected;
        }

        public static IEnumerable<FieldInfo> ExtractPorts(this object obj) => ExtractPorts(obj.GetType());
        public static IEnumerable<FieldInfo> ExtractPorts(this Type type) {
            return ExtractFields(type).Where(info => typeof(MovementPort).IsAssignableFrom(info.FieldType));
        }
        
        
    }
}