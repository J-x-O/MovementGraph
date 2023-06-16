using System;
using JescoDev.MovementGraph.Layer;
using JescoDev.MovementGraph.States;
using JescoDev.MovementGraph.StateTransition;
using UnityEngine;

namespace JescoDev.MovementGraphSample {
    
    // we extend MovementState<CustomMovementSample> so we can access our custom data
    [Serializable]
    public class MovementStateWalk : MovementState<CustomMovementSample> {
        
        // you can expose scene references, utilizing dependency injection
        [SerializeField] private GroundedManager _floorManager;
        
        // you can expose balancing variables
        [SerializeField] private float _movementSpeed;
        
        // you can use custom editors
        [SerializeField, Range(0, 1)] private float _drag;

        // you can utilize activate and deactivate to hook up event subscribtions
        public override void Activate() => _floorManager.OnUngrounded.AddListener(SwitchToAir);
        public override void Deactivate() => _floorManager.OnUngrounded.RemoveListener(SwitchToAir);

        // you can exit a state through a custom port like this
        private void SwitchToAir() => QueueRegularExit();

        public override MovementDefinition HandleMovement() {
            
            // Custom and our data is accessible since we extend MovementState<CustomMovementSample>
            float input = Custom.Input;
            Vector3 currentVelocity = Custom.CharController.velocity;
            float targetMovement = input * _movementSpeed;
            
            // calculate our preferred movement value
            currentVelocity.x = Mathf.Lerp(currentVelocity.x, targetMovement, _drag);
            
            // account for delta time
            return MovementDefinition.Local(currentVelocity * Time.deltaTime);
        }
    }
}