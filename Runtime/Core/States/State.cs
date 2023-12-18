using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JescoDev.SmoothBrainStates.StateTransition;
using JescoDev.SmoothBrainStates.SubStates;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.States {
    
    [Serializable]
    public abstract class State {

#if UNITY_EDITOR
        [HideInInspector] [SerializeField] protected Vector2 _position;
#endif
        
        [field:NonSerialized] public IStateParent Parent { get; protected set; }
        
        public string Identifier => _identifier;
        [SerializeField] protected string _identifier;

        internal string Guid => _guid;
        [SerializeField] private string _guid;
        
        public SmoothBrainStates StateMachine => Parent.StateMachine;
        public Transform Transform => StateMachine.transform;
        public GameObject GameObject => StateMachine.gameObject;

        public State(string identifier) {
            _identifier = identifier;
            _guid = System.Guid.NewGuid().ToString();
        }
        
        internal abstract bool CanBeActivated();
        
        /// <summary> This function decides if the state can be activated or not </summary>
        /// <returns> The resolved movement state for activation, or null if we cant activate </returns>
        internal abstract ExecutableState ResolveActivation(SmoothPort incomingPort = null);

        public SmoothPort GetPort(string identifier) {
            return GetAllPorts().FirstOrDefault(port => port.Identifier == identifier);
        }
        
        public IEnumerable<SmoothPort> GetAllPorts() {
            return GetType().ExtractFields()
                .Select(field => field.GetValue(this))
                .OfType<SmoothPort>();
        }
        
        private IEnumerable<SmoothPort> GetFilteredPorts(Func<FieldInfo ,bool> predicate) {
            return GetType().ExtractFields()
                .Where(predicate)
                .Select(field => field.GetValue(this))
                .OfType<SmoothPort>();
        }
        
        public IEnumerable<SmoothPort> GetInputPorts() => GetFilteredPorts(field => field.IsInputPort());
        public IEnumerable<SmoothPort> GetOutputPorts() => GetFilteredPorts(field => field.IsOutputPort());

        private IEnumerable<FieldInfo> GetPortInfo() {
            return GetType().ExtractFields()
                .Where(field => typeof(SmoothPort).IsAssignableFrom(field.FieldType));
        }
        
        internal virtual void OnBeforeSerialize() {
            foreach (SmoothPort port in GetAllPorts()) {
                port.OnBeforeSerialize(this);
            }
        }

        internal virtual void OnAfterDeserialize(IStateParent parent) {
            Parent = parent;
            foreach (FieldInfo portInfo in GetPortInfo()) {
                SmoothPort port = portInfo.GetValue(this) as SmoothPort;
                string identifier = portInfo.Name;
                port?.OnAfterDeserialize(this, identifier);
            }
        }

        internal virtual void OnLateDeserialize() {
            foreach (SmoothPort port in GetAllPorts()) {
                port?.OnLateDeserialize(this);
            }
        }
    }
}