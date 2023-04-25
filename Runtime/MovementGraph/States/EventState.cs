using System;
using JescoDev.MovementGraph.MovementGraph.StateTransition;
using JescoDev.MovementGraph.States;

namespace Entities.Movement.States {
    
    [Serializable]
    public class EventState : NamedState {
        
        [OutputPort] private Port OutputPort;
        
        public override bool ValidActivation() => true;

        public override MovementState ResolveActivation() => OutputPort.FindFirstValidTransition();
    }
}