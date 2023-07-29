using System;
using UnityEngine;

namespace JescoDev.MovementGraph {
    
    public interface IOnControllerColliderHitReceiver {
        void OnControllerColliderHit(ControllerColliderHit hit);
    }
    
    public class CustomMovementCharacterController : CustomMovement {
        
        public Vector3 Center => _charController.transform.TransformPoint(_charController.center);
        
        public CharacterController CharController => _charController;
        [SerializeField] private CharacterController _charController;

        protected virtual void Awake() => CharController.enabled = false;
        protected virtual void Start() => CharController.enabled = true;

        internal override void MoveBy(Vector3 movement) => _charController.Move(movement);

        internal override void TeleportTo(Vector3 targetWorld) {
            _charController.enabled = false;
            transform.position = targetWorld;
            _charController.enabled = true;
        }

        protected virtual void OnControllerColliderHit(ControllerColliderHit hit) {
            MovementSystem.SendEvent<IOnControllerColliderHitReceiver>(receiver => receiver.OnControllerColliderHit(hit));
        }
    }
}