using System;
using JescoDev.MovementGraph.MovementGraph.Attributes;
using JescoDev.SmoothBrainStates.StateTransition;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.States {
    
    [Serializable, MovementMenuPath("Default/Event")]
    public class EventState : State {
        
        [field: SerializeField, OutputPort] public SmoothPort OutputPort { get; protected set; }

        internal override bool CanBeActivated() => true;

        internal override ExecutableState ResolveActivation(SmoothPort incomingPort)
            => OutputPort.FindFirstValidTransition();

        public EventState() : base("Event") { }
    }
}