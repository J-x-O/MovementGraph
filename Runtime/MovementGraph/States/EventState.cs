using System;
using JescoDev.MovementGraph.States;

namespace Entities.Movement.States {
    
    [Serializable]
    public class EventState : NamedState {
        
        private Port OutputPort;
        
        public override bool ValidActivation() => true;

        public override MovementState ResolveActivation() => OutputPort.FindFirstValidTransition();
    }
}