using JescoDev.MovementGraph.States;

namespace Gameplay.Movement.States {
    public interface IFastForward {

        public MovementPort GetNextPort(MovementPort port);

        public static MovementPort Connect(MovementPort from, MovementPort to, MovementPort compare) {
            if (from == compare) return to;
            if (to == compare) return from;
            return null;
        }

    }
}