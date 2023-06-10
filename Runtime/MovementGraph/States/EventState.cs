using System;
using JescoDev.MovementGraph.States;
using JescoDev.MovementGraph.StateTransition;
using Movement.States;
using UnityEngine;

namespace Entities.Movement.States {
    
    [Serializable]
    public class EventState : State {
        
        [field: SerializeField, OutputPort] public MovementPort OutputPort { get; protected set; }

        public override bool ValidActivation() => true;

        public override MovementState ResolveActivation(MovementPort incomingPort = null)
            => OutputPort.FindFirstValidTransition();

        public EventState() : base("Event") { }
    }
}