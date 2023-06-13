using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Entities.Movement;
using Entities.Movement.States;
using Gameplay.Movement.Layer;
using JescoDev.MovementGraph.Layer;
using JescoDev.MovementGraph.States;
using JescoDev.MovementGraph.StateTransition;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Movement.States {
    
    [Serializable]
    public abstract class State {

#if UNITY_EDITOR
        [HideInInspector] [SerializeField] protected Vector2 _position;
#endif
        
        public MovementLayer Layer { get; protected set; }
        
        public string Identifier => _identifier;
        [SerializeField] protected string _identifier;
        
        public Transform Transform => Layer.System.transform;
        public GameObject GameObject => Layer.System.gameObject;

        public State(string identifier) {
            _identifier = identifier;
        }
        
        public abstract bool ValidActivation();
        
        /// <summary> This function decides if the state can be activated or not </summary>
        /// <returns> The resolved movement state for activation, or null if we cant activate </returns>
        public abstract MovementState ResolveActivation(MovementPort incomingPort = null);

        public MovementPort GetPort(string identifier) {
            return GetAllPorts().FirstOrDefault(port => port.Identifier == identifier);
        }
        
        public IEnumerable<MovementPort> GetAllPorts() {
            return GetType().ExtractFields()
                .Select(field => field.GetValue(this))
                .OfType<MovementPort>();
        }
        
        private IEnumerable<MovementPort> GetFilteredPorts(Func<FieldInfo ,bool> predicate) {
            return GetType().ExtractFields()
                .Where(predicate)
                .Select(field => field.GetValue(this))
                .OfType<MovementPort>();
        }
        
        public IEnumerable<MovementPort> GetInputPorts() => GetFilteredPorts(field => field.IsInputPort());
        public IEnumerable<MovementPort> GetOutputPorts() => GetFilteredPorts(field => field.IsOutputPort());

        private IEnumerable<FieldInfo> GetPortInfo() {
            return GetType().ExtractFields()
                .Where(field => typeof(MovementPort).IsAssignableFrom(field.FieldType));
        }
        
        public void OnBeforeSerialize() {
            foreach (MovementPort port in GetAllPorts()) {
                port.OnBeforeSerialize(this);
            }
        }

        public void OnAfterDeserialize(MovementLayer layer) {
            Layer = layer;
            foreach (FieldInfo portInfo in GetPortInfo()) {
                MovementPort port = portInfo.GetValue(this) as MovementPort;
                string identifier = portInfo.Name;
                port?.OnAfterDeserialize(this, identifier);
            }
        }

        public void OnLateDeserialize() {
            foreach (MovementPort port in GetAllPorts()) {
                port?.OnLateDeserialize(this);
            }
        }
    }
}