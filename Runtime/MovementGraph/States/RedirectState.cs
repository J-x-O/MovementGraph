using System;

namespace JescoDev.MovementGraph.States {
    
    [Serializable]
    public class RedirectState : State {

        public override bool ValidActivation() => HasTransition(true);

        public override MovementState ResolveActivation() => FindFirstValidTransition();
    }
}