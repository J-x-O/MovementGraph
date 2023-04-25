using System;
using Entities.Movement.States;
using Gameplay.Movement.States;
using JescoDev.MovementGraph.MovementGraph.StateTransition;
using Movement.States;

namespace JescoDev.MovementGraph.States {
    
    [Serializable]
    public class RedirectState : State, IFastForward {
        
        [OutputPort] public Port OutputPort;
        [InputPort] public Port InputPort;

        public override bool ValidActivation() => InputPort.HasActiveTransition(true);

        public override MovementState ResolveActivation(Port incomingPort = null)
            => OutputPort.FindFirstValidTransition();
        
        public Port GetNextPort(Port port) {
            if (port == OutputPort) return InputPort;
            if (port == InputPort) return OutputPort;
            return null;
        }
    }
}