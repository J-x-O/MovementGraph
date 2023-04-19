using System;

namespace JescoDev.MovementGraph.States {
    
    [Serializable]
    public class EventState : NamedState {
        public override bool ValidActivation() => true;

        public override MovementState ResolveActivation() => FindFirstValidTransition();
    }
}