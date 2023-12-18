using System;
using System.Collections.Generic;
using JescoDev.SmoothBrainStates.StateTransition;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.States {
    
    [Serializable]
    public abstract class ExecutableState : State {

        [field: SerializeField, InputPort] public SmoothPort InputPort { get; protected set; }
        [field: SerializeField, OutputPort] public SmoothPort RegularExit { get; protected set; }

        public IReadOnlyList<string> Tags => _tags;
        [HideInInspector] [SerializeField] private List<string> _tags = new List<string>();
        
        public ExecutableState(string identifier) : base(identifier) { }
        
        public virtual bool CanBeReactivated => false;
        
        internal virtual void Awake() { }
        
        internal virtual void Destroy() {}

        internal virtual void OnActivate() {}
        
        internal virtual void OnDeactivate() {}
        
        public bool HasTag(string tag) => _tags.Contains(tag);

        internal override bool CanBeActivated() => true;
        
        /// <summary> It is important that only an state which can be activated results in a valid return </summary>
        internal override ExecutableState ResolveActivation(SmoothPort incomingPort = null) {
            return this;
        }
        
        public void ExitCurrentState(SmoothPort port = null)
            => Parent.StateMachine.ExitCurrentState(port ?? RegularExit);
    }
}