using UnityEngine;

namespace JescoDev.MovementGraph.Customization {
    public abstract class CustomMovement : MonoBehaviour {

        public abstract void MoveTo(Vector3 worldPos);

        public abstract void MoveBy(Vector3 worldPos);
    }
}