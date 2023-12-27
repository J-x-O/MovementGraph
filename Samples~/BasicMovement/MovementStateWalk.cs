using System;
using JescoDev.SmoothBrainStates;
using JescoDev.SmoothBrainStates.Movement;
using JescoDev.SmoothBrainStates.Movement.Tags;
using UnityEngine;

namespace JescoDev.MovementGraph.Samples.BasicMovement {
    
    // we extend MovementState<CustomMovementSample> so we can access our custom data
    [Serializable]
    public class MovementStateWalk : TaggedExecutableState, IMovementState {
        
        [SerializeField] private CustomMovementSample _custom;
        [SerializeField, OutputPort] private SmoothPort _onAirborne;
        
        // you can expose scene references, utilizing dependency injection
        [SerializeField] private GroundedManager _floorManager;
        
        // you can expose balancing variables
        [SerializeField] private float _movementSpeed;
        
        // you can use custom editors
        [SerializeField, Range(0, 1)] private float _drag;

        // you can define custom conditions for activation
        protected override bool CanBeActivated() => _floorManager.Grounded == true;
        
        // you can utilize activate and deactivate to hook up event subscribtions
        protected override void OnActivate() => _floorManager.OnUngrounded.AddListener(SwitchToAir);
        protected override void OnDeactivate() => _floorManager.OnUngrounded.RemoveListener(SwitchToAir);

        // you can exit a state through a custom port like this
        private void SwitchToAir() => ExitCurrentState(_onAirborne);

        public MovementDefinition HandleMovement() {
            
            // Custom and our data is accessible since we extend MovementState<CustomMovementSample>
            float input = _custom.Input;
            Vector3 currentVelocity = _custom.CharController.velocity;
            float targetMovement = input * _movementSpeed;
            
            // calculate our preferred movement value
            currentVelocity.x = Mathf.Lerp(currentVelocity.x, targetMovement, _drag);
            currentVelocity.y = _custom.Gravity;
            
            // account for delta time
            return MovementDefinition.Local(currentVelocity * Time.fixedDeltaTime);
        }
    }
}