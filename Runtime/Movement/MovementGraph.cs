using UnityEngine;

namespace JescoDev.SmoothBrainStates.Movement {
    public abstract class MovementGraph : MonoBehaviour {

        [field:SerializeField] public SmoothBrainStateMashine SmoothBrainStateMashine { get; private set; }

        private void FixedUpdate() {
            Vector3 localMovement = Vector3.zero;
            if(SmoothBrainStateMashine.CurrentState is not IMovementState state) return;

            MovementDefinition move = state.HandleMovement();
            switch (move.Context) {
                case MovementContext.Global:
                    move.Movement = transform.InverseTransformPoint(move.Movement);
                    break;
                case MovementContext.Teleport:
                    TeleportToInternal(move.Movement);
                    break;
            }

            MoveByInternal(localMovement);
        }
        
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