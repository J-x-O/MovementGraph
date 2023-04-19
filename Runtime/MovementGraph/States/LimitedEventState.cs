using System;

namespace JescoDev.MovementGraph.States {
    
    [Serializable]
    public class LimitedEventState : EventState {
        public override bool ValidActivation() => HasTransition(true);
    }
}