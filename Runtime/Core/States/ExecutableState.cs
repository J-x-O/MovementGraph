using System;
using System.Collections.Generic;
using JescoDev.SmoothBrainStates.StateTransition;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.States {
    
    [Serializable]
    public abstract class ExecutableState : State {

        [field: SerializeField, InputPort] public SmoothPort InputPort { get; protected set; }
        [field: SerializeField, OutputPort] public SmoothPort RegularExit { get; protected set; }
        
        public ExecutableState(string identifier) : base(identifier) { }
        
        public virtual bool CanBeReactivated => false;
        
        protected internal virtual void Awake() { }
        
        protected internal virtual void Destroy() {}

        protected internal virtual void OnActivate() {}
        
        protected internal virtual void OnDeactivate() {}

        protected internal override bool CanBeActivated() => true;
        
        /// <summary> It is important that only an state which can be activated results in a valid return </summary>
        protected internal override ExecutableState ResolveActivation(SmoothPort incomingPort = null) {
            return this;
        }
        
        public void ExitCurrentState(SmoothPort port = null)
            => Parent.StateMashineMachine.ExitCurrentState(port ?? RegularExit);
    }
}