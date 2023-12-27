using System;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.Movement {
    public abstract class MovementGraph : MonoBehaviour {

        [field:SerializeField] public SmoothBrainStateMashine StateMashine { get; private set; }

        private void FixedUpdate() {
            if(StateMashine.CurrentState is not IMovementState state) return;

            MovementDefinition move = state.HandleMovement();
            switch (move.Context) {
                case MovementContext.Global:
                    MoveBy(transform.InverseTransformPoint(move.Movement));
                    break;
                case MovementContext.Teleport:
                    TeleportTo(move.Movement);
                    break;
                case MovementContext.Local:
                    MoveBy(move.Movement);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        /// <summary> Moves the player along a vector </summary>
        /// <param name="movement"> the movement vector, applied relative to the current player position </param>
        protected abstract void MoveBy(Vector3 movement);
        
        /// <summary> Teleports the player to a specified position </summary>
        /// <param name="targetWorld"> the target position in world space </param>
        protected abstract void TeleportTo(Vector3 targetWorld);

    }
}