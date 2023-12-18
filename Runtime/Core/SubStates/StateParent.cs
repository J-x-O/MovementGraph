using System;
using System.Collections.Generic;
using System.Linq;
using JescoDev.MovementGraph.Layer;
using JescoDev.SmoothBrainStates.States;
using JescoDev.SmoothBrainStates.StateTransition;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.SubStates {
    
    [Serializable]
    public class StateParent : ExecutableState, IStateParent {

        [SerializeField] private MovementLayerConnector _connector;
        
        public readonly MovementLayerEvents Events = new MovementLayerEvents();

        public IEnumerable<State> States => _states;
        public IEnumerable<ExecutableState> MovementStates => _states.OfType<ExecutableState>();
        [Tooltip("All possible states this character can use")]
        [SerializeReference] private List<State> _states = new List<State>();

        public IEnumerable<SmoothPort> InputPorts => _inputPorts;
        [SerializeField] private List<SmoothPort> _inputPorts;
        public IEnumerable<SmoothPort> OutputPorts => _outputPorts;
        [SerializeField] private List<SmoothPort> _outputPorts;

        public StateParent() : base("StateParent") { }
        
        internal override void Awake() {
            _connector.Awake(this);
            foreach (ExecutableState state in MovementStates) state.Awake();
        }

        internal void OnDestroy() {
            foreach (State state in States) {
                if (state is ExecutableState movementState) movementState.Destroy();
            }
        }

        internal override void OnBeforeSerialize() {
            base.OnBeforeSerialize();
            foreach (State state in States) state?.OnBeforeSerialize();
        }

        internal override void OnAfterDeserialize(IStateParent parent) {
            base.OnAfterDeserialize(parent);
            foreach (State state in States) state?.OnAfterDeserialize(this);
        }
        
        internal override void OnLateDeserialize() {
            base.OnLateDeserialize();
            foreach (State state in States) state?.OnLateDeserialize();
        }

        public SmoothBrainStates StateMachine => Parent.StateMachine;
        internal override bool CanBeActivated() => true;

        internal override ExecutableState ResolveActivation(SmoothPort incomingPort = null) {
            return _connector.FindFirstValidTransition();
        }

        public string ResolvePath() => Parent.ResolvePath() + "/" + Identifier;
    }
}