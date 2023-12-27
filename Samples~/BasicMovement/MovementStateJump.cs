using System;
using JescoDev.SmoothBrainStates.Movement;
using UnityEngine;

namespace JescoDev.MovementGraph.Samples.BasicMovement {
    
    // you can recycle variables and behaviour by just inheriting from another existing class
    [Serializable]
    public class MovementStateJump : MovementStateAirborne, IMovementState {
        
        // now we extend the state by another variable
        [SerializeField] private float _jumpForce;

        protected override bool CanBeActivated() {
            return true;
        }

        protected override void OnActivate() {
            _downwardsVelocity = _jumpForce;
        }

        public override MovementDefinition HandleMovement() {
            // if we are over the arc, use regular airborne logic
            if(_downwardsVelocity < 0) ExitCurrentState();
            return base.HandleMovement();
        }
    }
}