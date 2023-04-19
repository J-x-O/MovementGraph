using System;
using JescoDev.MovementGraph.States;

namespace Entities.Movement.States {
    
    [Serializable]
    public class LimitedEventState : EventState {
        
        private Port InputPort;
        
        public override bool ValidActivation() => InputPort.HasTransition(true);
    }
}