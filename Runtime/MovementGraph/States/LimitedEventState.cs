using System;
using JescoDev.MovementGraph.MovementGraph.StateTransition;
using JescoDev.MovementGraph.States;

namespace Entities.Movement.States {
    
    [Serializable]
    public class LimitedEventState : EventState {
        
        [InputPort] private Port InputPort;
        
        public override bool ValidActivation() => InputPort.HasActiveTransition(true);
    }
}