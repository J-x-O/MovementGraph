using System;
using JescoDev.MovementGraph.States;
using JescoDev.MovementGraph.StateTransition;
using UnityEngine;

namespace Entities.Movement.States {
    
    [Serializable]
    public class EventState : NamedState {
        
        [field: SerializeField, OutputPort] public MovementPort OutputPort { get; private set; }

        public override bool ValidActivation() => true;

        public override MovementState ResolveActivation(MovementPort incomingPort = null)
            => OutputPort.FindFirstValidTransition();
    }
}