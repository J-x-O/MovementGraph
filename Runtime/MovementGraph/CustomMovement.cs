using UnityEngine;

namespace JescoDev.MovementGraph {
    public abstract class CustomMovement : MonoBehaviour {

        /// <summary> Moves the player along a vector </summary>
        /// <param name="movement"> the movement vector, applied relative to the current player position </param>
        public abstract void MoveBy(Vector3 movement);

        /// <summary> Teleports the player to a specified position </summary>
        /// <param name="targetWorld"> the target position in world space </param>
        public abstract void TeleportTo(Vector3 targetWorld);

    }
}