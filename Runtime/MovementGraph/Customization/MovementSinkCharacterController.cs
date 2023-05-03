using System;
using UnityEngine;

namespace JescoDev.MovementGraph.DefaultTargets {
    
    [Serializable]
    public class MovementSinkCharacterController : IMovementSink {
        
        [SerializeField] private CharacterController _target;

        public void Disable() {
            _target.enabled = false;
        }

        public void Enable() {
            _target.enabled = true;
        }

        public void MoveTo(Vector3 worldPos) => MoveBy(worldPos - _target.transform.position);

        public void MoveBy(Vector3 direction) => _target.Move(direction);
    }
}