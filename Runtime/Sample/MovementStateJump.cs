using System;
using JescoDev.MovementGraph.Layer;
using JescoDev.MovementGraph.StateTransition;
using UnityEngine;

namespace JescoDev.MovementGraph.Sample.Sample {
    
    // you can recycle variables and behaviour by just inheriting from another existing class
    [Serializable]
    public class MovementStateJump : MovementStateAirborne {
        
        // now we extend the state by another variable
        [SerializeField] private float _jumpForce;

        public override void Activate() {
            base.Activate();
            _downwardsVelocity = _jumpForce;
        }

        public override MovementDefinition HandleMovement() {
            // if we are over the arc, use regular airborne logic
            if(_downwardsVelocity < 0) QueueRegularExit();
            return base.HandleMovement();
        }
    }
}