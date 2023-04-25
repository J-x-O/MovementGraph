using System;
using JescoDev.MovementGraph.MovementGraph.StateTransition;
using JescoDev.MovementGraph.States;

namespace Entities.Movement.States {
    
    [Serializable]
    public class EventState : NamedState {
        
        [OutputPort] private Port OutputPort;

        public EventState() {
            OutputPort = new Port { State = this };
        }
        
        public override bool ValidActivation() => true;

        public override MovementState ResolveActivation(Port incomingPort = null)
            => OutputPort.FindFirstValidTransition();
    }
}