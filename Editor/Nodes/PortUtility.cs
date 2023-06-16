using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JescoDev.MovementGraph.States;
using JescoDev.MovementGraph.StateTransition;

namespace Editor.MovementEditor {
    public static class PortUtility {
        
        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance |
                                                  System.Reflection.BindingFlags.NonPublic |
                                                  System.Reflection.BindingFlags.Public;
        
        public static IEnumerable<string> GetAllPorts(this State state) {
            return state.GetType()
                .GetFields(BindingFlags)
                .Where(field => field.GetValue(state) is MovementPort)
                .Select(field => field.Name);
        }
        
        private static IEnumerable<string> GetFilteredPorts(this State state, Func<FieldInfo ,bool> predicate) {
            return state.GetType()
                .GetFields(BindingFlags)
                .Where(predicate)
                .Where(field => field.GetValue(state) is MovementPort)
                .Select(field => field.Name);
        }
        
        public static IEnumerable<string> GetInputPorts(this State state) {
            return state.GetFilteredPorts(field => field.GetCustomAttribute<PortType>()?.IsInput ?? false);
        }
        
        public static IEnumerable<string> GetOutputPorts(this State state) {
            return state.GetFilteredPorts(field => field.GetCustomAttribute<PortType>()?.IsOutput ?? false);
        }
    }
}