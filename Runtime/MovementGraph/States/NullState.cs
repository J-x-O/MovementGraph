using JescoDev.MovementGraph.Layer;
using JescoDev.MovementGraph.StateTransition;

namespace JescoDev.MovementGraph.States {
    
    // this class is not serializable, so it doesnt show in the movement graph,
    // its only used at runtime to indicate that a layer is paused
    public class NullState : MovementState {

        [InputPort] public readonly MovementPort In = new MovementPort();
        [OutputPort] public readonly MovementPort Restart = new MovementPort();

        public NullState() => _identifier = "Layer Exit";

        public override MovementDefinition HandleMovement() {
            return MovementDefinition.None;
        }
    }
}