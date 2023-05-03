using UnityEngine;

namespace JescoDev.MovementGraph {
    public interface IMovementSink {

        public void Disable();
        public void Enable();
        public void MoveTo(Vector3 worldPos);
        public void MoveBy(Vector3 worldPos);

    }
}