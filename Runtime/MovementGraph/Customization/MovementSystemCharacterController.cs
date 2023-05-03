using System;
using UnityEngine;

namespace JescoDev.MovementGraph.DefaultTargets {
    
    /// <summary> Default Implementation using a character controller </summary>
    public class MovementSystemCharacterController : MovementSystemCustomization {
        
        public CharacterController Target => _target;
        [SerializeField] private CharacterController _target;

        public void Awake() => _target.enabled = true;
        public void Start() => _target.enabled = false;

        public override void MoveTo(Vector3 worldPos) => MoveBy(worldPos - _target.transform.position);

        public override void MoveBy(Vector3 direction) => _target.Move(direction);
    }
}