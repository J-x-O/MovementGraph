using System;
using Entities.Movement.States;
using Gameplay.Movement.States;
using Movement.States;

namespace JescoDev.MovementGraph.States {
    
    [Serializable]
    public class RedirectState : State, IFastForward {
        
        public Port OutputPort;
        public Port InputPort;

        public override bool ValidActivation() => InputPort.HasTransition(true);

        public override MovementState ResolveActivation() => OutputPort.FindFirstValidTransition();
        public Port GetNextPort(bool reverse) {
            return reverse ? InputPort : OutputPort;
        }
    }
}