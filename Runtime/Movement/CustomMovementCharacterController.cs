using UnityEngine;

namespace JescoDev.SmoothBrainStates.Utility {
    
    public interface IOnControllerColliderHitReceiver {
        void OnControllerColliderHit(ControllerColliderHit hit);
    }
    
    public class CustomMovementCharacterController : CustomMovement {
        
        public Vector3 Center => _charController.transform.TransformPoint(_charController.center);
        
        public CharacterController CharController => _charController;
        [SerializeField] private CharacterController _charController;

        protected virtual void Awake() => CharController.enabled = false;
        protected virtual void Start() => CharController.enabled = true;

        protected override void MoveBy(Vector3 movement) => _charController.Move(movement);

        protected override void TeleportTo(Vector3 targetWorld) {
            _charController.enabled = false;
            transform.position = targetWorld;
            _charController.enabled = true;
        }

        protected virtual void OnControllerColliderHit(ControllerColliderHit hit) {
            SmoothBrainStates.SendEvent<IOnControllerColliderHitReceiver>(receiver => receiver.OnControllerColliderHit(hit));
        }
    }
}