using System;
using JescoDev.MovementGraph.Layer;
using JescoDev.MovementGraph.States;
using JescoDev.MovementGraph.StateTransition;
using UnityEngine;

namespace JescoDev.MovementGraph.Samples.BasicMovement {
    
    // very similar to walk
    [Serializable]
    public class MovementStateAirborne : MovementState<CustomMovementSample> {

        [SerializeField, OutputPort] private MovementPort _onGrounded;
        
        [SerializeField] private GroundedManager _floorManager;
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _maxFallSpeed;
        [SerializeField, Range(0, 1)] private float _drag;

        // you can utilize runtime variables like this
        protected float _downwardsVelocity = 0;
        
        public override void Activate() {
            // we use a different event here, compared to walking
            _floorManager.OnGrounded.AddListener(SwitchToAir);
            
            // dont forget to reset them
            _downwardsVelocity = Custom.CharController.velocity.y;
        }

        public override void Deactivate() => _floorManager.OnGrounded.RemoveListener(SwitchToAir);
        private void SwitchToAir() => Layer.QueueExit(_onGrounded);

        public override MovementDefinition HandleMovement() {
            
            // apply gravity, respective to the passed time
            _downwardsVelocity += Custom.Gravity * Time.deltaTime;
            _downwardsVelocity = Mathf.Max(_downwardsVelocity, -_maxFallSpeed);
            
            // Custom and our data is accessible since we extend MovementState<CustomMovementSample>
            float input = Custom.Input;
            Vector3 currentVelocity = Custom.CharController.velocity;
            float targetMovement = input * _movementSpeed;
            
            // calculate our preferred movement value
            currentVelocity.x = Mathf.Lerp(currentVelocity.x, targetMovement, _drag);
            currentVelocity.y = _downwardsVelocity;
            
            // account for delta time
            return MovementDefinition.Local(currentVelocity * Time.deltaTime);
        }
        
    }
}