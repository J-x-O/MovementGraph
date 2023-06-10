using System;
using UnityEngine;

namespace JescoDev.MovementGraph {
    public class CustomMovementCharacterController : CustomMovement {
        
        public CharacterController CharController => _charController;
        [SerializeField] private CharacterController _charController;

        private void Awake() => CharController.enabled = false;
        private void Start() => CharController.enabled = true;

        public override void MoveBy(Vector3 movement) => _charController.Move(movement);

        public override void TeleportTo(Vector3 targetWorld) {
            _charController.enabled = false;
            transform.position = targetWorld;
            _charController.enabled = true;
        }
    }
}