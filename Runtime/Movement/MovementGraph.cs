using UnityEngine;

namespace JescoDev.SmoothBrainStates.MovementGraph {
    public class MovementGraph : SmoothBrainStates {
        
        private void FixedUpdate() {
            Vector3 localMovement = Vector3.zero;
            if(CurrentState is not IMovementState state) return;

            MovementDefinition move = state.HandleMovement();
            switch (move.Context) {
                case MovementContext.Global:
                    move.Movement = transform.InverseTransformPoint(move.Movement);
                    break;
                case MovementContext.Teleport:
                    CustomMovement.TeleportToInternal(move.Movement);
                    break;
            }

            CustomMovement.MoveByInternal(localMovement);
        }
        
    }
}