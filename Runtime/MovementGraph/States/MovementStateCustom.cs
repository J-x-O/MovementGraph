using Entities.Movement.States;

namespace JescoDev.MovementGraph.States {
    public abstract class MovementState<T> : MovementState where T : CustomMovement {

        public T Custom => Layer.System.CustomMovement as T;

    }
}