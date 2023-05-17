using UnityEngine;

namespace JescoDev.MovementGraph.Customization {
    
    /// <summary> Default Implementation using a character controller </summary>
    public class CustomMovementCharacterController : CustomMovement {
        
        public CharacterController Target => _target;
        [SerializeField] private CharacterController _target;

        public void Awake() => _target.enabled = false;
        public void Start() => _target.enabled = true;

        public override void MoveTo(Vector3 worldPos) => MoveBy(worldPos - _target.transform.position);

        public override void MoveBy(Vector3 direction) => _target.Move(direction);
    }
}