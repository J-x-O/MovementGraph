using System;
using JescoDev.MovementGraph.MovementGraph.Attributes;
using JescoDev.SmoothBrainStates.StateTransition;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.States {
    
    [Serializable, HideStateIdentifier, MovementMenuPath("Default/Redirect")]
    public class RedirectState : State, IFastForward {
        
        [field: SerializeField, OutputPort] public SmoothPort OutputPort { get; private set; }
        [field: SerializeField, InputPort] public SmoothPort InputPort { get; private set; }

        internal override bool CanBeActivated() => InputPort.HasActiveTransition(true);

        internal override ExecutableState ResolveActivation(SmoothPort incomingPort)
            => OutputPort.FindFirstValidTransition();
        
        public SmoothPort GetNextPort(SmoothPort port) {
            if (port == OutputPort) return InputPort;
            if (port == InputPort) return OutputPort;
            return null;
        }

        public RedirectState() : base("<Hidden>Redirect") { }
    }
}