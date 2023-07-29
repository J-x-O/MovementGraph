using UnityEngine;

namespace JescoDev.MovementGraph {
    public abstract class CustomMovement : MonoBehaviour {

        public MovementSystem MovementSystem { get; internal set; }

        /// <inheritdoc cref="MoveBy"/>
        internal void MoveByInternal(Vector3 movement) => MoveBy(movement);
        
        /// <summary> Moves the player along a vector </summary>
        /// <param name="movement"> the movement vector, applied relative to the current player position </param>
        protected abstract void MoveBy(Vector3 movement);

        /// <inheritdoc cref="TeleportTo"/>
        internal void TeleportToInternal(Vector3 targetWorld) => TeleportTo(targetWorld);
        
        /// <summary> Teleports the player to a specified position </summary>
        /// <param name="targetWorld"> the target position in world space </param>
        protected abstract void TeleportTo(Vector3 targetWorld);

    }
}