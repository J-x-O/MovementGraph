using System;
using JescoDev.SmoothBrainStates.Attributes;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.States {
    
    [Serializable, SmoothStateHideIdentifier, SmoothStateMenuPath("Default/Redirect")]
    public class RedirectState : State, IFastForward {
        
        [field: SerializeField, OutputPort] public SmoothPort OutputPort { get; private set; }
        [field: SerializeField, InputPort] public SmoothPort InputPort { get; private set; }

        protected internal override bool CanBeActivated() => InputPort.HasActiveTransition(true);

        protected internal override ExecutableState ResolveActivation(SmoothPort incomingPort)
            => OutputPort.FindFirstValidTransition();
        
        public SmoothPort GetNextPort(SmoothPort port) {
            if (port == OutputPort) return InputPort;
            if (port == InputPort) return OutputPort;
            return null;
        }

        public RedirectState() : base("<Hidden>Redirect") { }
    }
}