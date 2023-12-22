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
        
        public SmoothBrainStateMashine StateMashineMachine => Parent.StateMashineMachine;
        public Transform Transform => StateMashineMachine.transform;
        public GameObject GameObject => StateMashineMachine.gameObject;

        public State(string identifier) {
            _identifier = identifier;
            _guid = System.Guid.NewGuid().ToString();
        }
        
        protected internal abstract bool CanBeActivated();
        
        /// <summary> This function decides if the state can be activated or not </summary>
        /// <returns> The resolved movement state for activation, or null if we cant activate </returns>
        protected internal abstract ExecutableState ResolveActivation(SmoothPort incomingPort = null);

        public SmoothPort GetPort(string identifier) {
            return GetAllPorts().FirstOrDefault(port => port.Identifier == identifier);
        }
        
        public IEnumerable<SmoothPort> GetAllPorts() {
            IEnumerable<object> all = GetType().ExtractFields()
                .Select(field => field.GetValue(this));
            return GetPorts(all);
        }
        
        private IEnumerable<SmoothPort> GetFilteredPorts(Func<FieldInfo ,bool> predicate) {
            IEnumerable<object> all = GetType().ExtractFields()
                .Where(predicate)
                .Select(field => field.GetValue(this));
            return GetPorts(all);
        }
        
        private IEnumerable<SmoothPort> GetPorts(IEnumerable<object> objects) {
            objects = objects.ToList();
            return objects.OfType<SmoothPort>()
                .Concat(objects
                    .OfType<IEnumerable<SmoothPort>>()
                    .SelectMany(ports => ports));
        }
        
        public IEnumerable<SmoothPort> GetInputPorts() => GetFilteredPorts(field => field.IsInputPort());
        public IEnumerable<SmoothPort> GetOutputPorts() => GetFilteredPorts(field => field.IsOutputPort());

        private IEnumerable<FieldInfo> GetPortInfo() {
            return GetType().ExtractFields()
                .Where(field => typeof(SmoothPort).IsAssignableFrom(field.FieldType));
        }
        
        protected internal virtual void OnBeforeSerialize() {
            foreach (SmoothPort port in GetAllPorts()) {
                port.OnBeforeSerialize(this);
            }
        }

        protected internal virtual void OnAfterDeserialize(IStateParent parent) {
            Parent = parent;
            foreach (FieldInfo portInfo in GetPortInfo()) {
                SmoothPort port = portInfo.GetValue(this) as SmoothPort;
                string identifier = portInfo.Name;
                port?.OnAfterDeserialize(this, identifier);
            }
        }

        protected internal virtual void OnLateDeserialize() {
            foreach (SmoothPort port in GetAllPorts()) {
                port?.OnLateDeserialize(this);
            }
        }
    }
}