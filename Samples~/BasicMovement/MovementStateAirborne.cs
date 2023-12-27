using System;
using JescoDev.SmoothBrainStates;
using JescoDev.SmoothBrainStates.Movement;
using JescoDev.SmoothBrainStates.Movement.Tags;
using UnityEngine;

namespace JescoDev.MovementGraph.Samples.BasicMovement {
    
    // very similar to walk
    [Serializable]
    public class MovementStateAirborne : TaggedExecutableState, IMovementState {

        [SerializeField] private CustomMovementSample _custom;
        [SerializeField, OutputPort] private SmoothPort _onGrounded;
        
        [SerializeField] private GroundedManager _floorManager;
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _maxFallSpeed;
        [SerializeField, Range(0, 1)] private float _drag;

        // you can utilize runtime variables like this
        protected float _downwardsVelocity = 0;

        protected override bool CanBeActivated() => _floorManager.Grounded == false;

        protected override void OnActivate() {
            // we use a different event here, compared to walking
            _floorManager.OnGrounded.AddListener(SwitchToAir);
            
            // dont forget to reset them
            _downwardsVelocity = _custom.CharController.velocity.y;
        }

        protected override void OnDeactivate() => _floorManager.OnGrounded.RemoveListener(SwitchToAir);
        private void SwitchToAir() => ExitCurrentState(_onGrounded);

        public virtual MovementDefinition HandleMovement() {
            
            // apply gravity, respective to the passed time
            _downwardsVelocity += _custom.Gravity * Time.fixedDeltaTime;
            _downwardsVelocity = Mathf.Max(_downwardsVelocity, -_maxFallSpeed);
            
            // Custom and our data is accessible since we extend MovementState<CustomMovementSample>
            float input = _custom.Input;
            Vector3 currentVelocity = _custom.CharController.velocity;
            float targetMovement = input * _movementSpeed;
            
            // calculate our preferred movement value
            currentVelocity.x = Mathf.Lerp(currentVelocity.x, targetMovement, _drag);
            currentVelocity.y = _downwardsVelocity;
            
            // account for delta time
            return MovementDefinition.Local(currentVelocity * Time.fixedDeltaTime);
        }
    }
}