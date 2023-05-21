using System;
using Entities.Movement.States;
using Gameplay.Movement.States;
using JescoDev.MovementGraph.StateTransition;
using Movement.States;
using UnityEngine;

namespace JescoDev.MovementGraph.States {
    
    [Serializable]
    public class RedirectState : State, IFastForward {
        
        [field: SerializeField, OutputPort] public MovementPort OutputPort { get; private set; }
        [field: SerializeField, InputPort] public MovementPort InputPort { get; private set; }

        public override bool ValidActivation() => InputPort.HasActiveTransition(true);

        public override MovementState ResolveActivation(MovementPort incomingPort = null)
            => OutputPort.FindFirstValidTransition();
        
        public MovementPort GetNextPort(MovementPort port) {
            if (port == OutputPort) return InputPort;
            if (port == InputPort) return OutputPort;
            return null;
        }
    }
}