using System;
using JescoDev.SmoothBrainStates.Attributes;
using JescoDev.SmoothBrainStates.StateTransition;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.States {
    
    [Serializable, SmoothStateMenuPath("Default/Event")]
    public class EventState : State {
        
        [field: SerializeField, OutputPort] public SmoothPort OutputPort { get; protected set; }

        protected internal override bool CanBeActivated() => true;

        protected internal override ExecutableState ResolveActivation(SmoothPort incomingPort)
            => OutputPort.FindFirstValidTransition();

        public EventState() : base("Event") { }
    }
}