using JescoDev.MovementGraph.Layer;
using JescoDev.MovementGraph.StateTransition;

namespace JescoDev.MovementGraph.States {
    
    // this class is not serializable, so it doesnt show in the movement graph,
    // its only used at runtime to indicate that a layer is paused
    public class NullState : MovementState {

        public NullState() {
            _identifier = "Layer Exit";
            InputPort = new MovementPort();
            RegularExit = new MovementPort();
        }

        public override MovementDefinition HandleMovement() {
            return MovementDefinition.None;
        }
    }
}