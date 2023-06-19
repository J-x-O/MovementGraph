using System;
using JescoDev.MovementGraph.StateTransition;
using UnityEngine;

namespace JescoDev.MovementGraph.States {
    
    [Serializable]
    public class EventState : State {
        
        [field: SerializeField, OutputPort] public MovementPort OutputPort { get; protected set; }

        public override bool ValidActivation() => true;

        public override MovementState ResolveActivation(MovementPort incomingPort = null)
            => OutputPort.FindFirstValidTransition();

        public EventState() : base("Event") { }
    }
}