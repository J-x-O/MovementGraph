using System;
using System.Collections.Generic;
using System.Linq;
using JescoDev.SmoothBrainStates.Attributes;
using JescoDev.SmoothBrainStates.States;
using JescoDev.SmoothBrainStates.StateTransition;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.SubStates {
    
    [Serializable]
    [SmoothStateEnter]
    [SmoothStateMenuPath("State Parent")]
    public class StateParent : State, IStateParent {

        [SerializeField] private MovementLayerConnector _connector;

        public IEnumerable<State> States => _states.Concat(_connector.Nodes);
        public IEnumerable<ExecutableState> MovementStates => _states.OfType<ExecutableState>();
        [Tooltip("All possible states this character can use")]
        [SerializeReference] private List<State> _states = new List<State>();

        public IEnumerable<SmoothPort> InputPorts => _inputPorts;
        [SerializeField, InputPort] private List<SmoothPort> _inputPorts;
        public IEnumerable<SmoothPort> OutputPorts => _outputPorts;
        [SerializeField, OutputPort] private List<SmoothPort> _outputPorts;

        public StateParent() : base("StateParent") { }
        
        public SmoothBrainStateMashine StateMashineMachine => Parent.StateMashineMachine;
        protected internal override bool CanBeActivated() => true;

        protected internal override ExecutableState ResolveActivation(SmoothPort incomingPort = null) {
            return _connector.ResolveActivation(incomingPort);
        }

        public string ResolvePath() => Parent.ResolvePath() + "/" + Identifier;
    }
}