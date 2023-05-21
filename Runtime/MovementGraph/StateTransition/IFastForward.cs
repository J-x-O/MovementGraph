using JescoDev.MovementGraph.States;

namespace Gameplay.Movement.States {
    public interface IFastForward {

        public MovementPort GetNextPort(MovementPort port);

    }
}