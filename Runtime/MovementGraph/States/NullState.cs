using Entities.Movement.States;
using JescoDev.MovementGraph.MovementGraph.StateTransition;
using JescoDev.MovementGraph.States;
using UnityEngine;

namespace JescoDev.MovementGraph.MovementGraph.States {
    
    // this class is not serializable, so it doesnt show in the movement graph,
    // its only used at runtime to indicate that a layer is paused
    public class NullState : MovementState {

        [InputPort] public readonly Port In = new Port();

        public override Vector3 HandleMovement(float input) {
            return Vector3.zero;
        }
    }
}