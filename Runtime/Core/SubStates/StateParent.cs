using System;
using System.Collections.Generic;
using System.Linq;
using JescoDev.SmoothBrainStates.Attributes;
using JescoDev.SmoothBrainStates.States;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.SubStates {
    
    [Serializable]
    [SmoothStateEnter]
    [SmoothStateMenuPath("State Parent")]
    public class StateParent : State, IStateParentWithExit, IFastForward {

        [field: SerializeField, InputPort] public SmoothPort InputPort { get; private set; }
        [field: SerializeField, OutputPort] public SmoothPort RegularExit { get; private set; }
        
        public SmoothInOut Connector => _connector;
        [SerializeField] private SmoothInOut _connector;

        public IEnumerable<State> States => _states.Concat(_connector.Nodes);
        public IEnumerable<ExecutableState> MovementStates => _states.OfType<ExecutableState>();
        [Tooltip("All possible states this character can use")]
        [SerializeReference] private List<State> _states = new List<State>();

        public StateParent() : base("StateParent") { }
        
        public SmoothBrainStateMashine StateMashineMachine => Parent.StateMashineMachine;
        protected internal override bool CanBeActivated() => true;

        protected internal override ExecutableState ResolveActivation(SmoothPort incomingPort = null) {
            return _connector.ResolveActivation(incomingPort);
        }
        
        public SmoothPort GetNextPort(SmoothPort port) {
            return port == InputPort ? _connector.InNode.In : null;
        }
        
        public string ResolvePath() => Parent.ResolvePath() + "/" + Identifier;

        protected internal override void OnBeforeSerialize() {
            base.OnBeforeSerialize();
            foreach (State state in States) state.OnBeforeSerialize();
        }
        
        protected internal override void OnAfterDeserialize(IStateParent parent) {
            base.OnAfterDeserialize(parent);
            foreach (State state in States) state.OnAfterDeserialize(this);
        }

        protected internal override void OnLateDeserialize() {
            base.OnLateDeserialize();
            foreach (State state in States) state.OnLateDeserialize();
        }
    }
}