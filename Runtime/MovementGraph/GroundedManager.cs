using UnityEngine;
using UnityEngine.Events;

namespace Player.Movement {
    
    public class GroundedManager : MonoBehaviour {

        [field:SerializeField] public UnityEvent OnGrounded { get; private set; }
        [field:SerializeField] public UnityEvent OnUngrounded { get; private set; }
        
        [SerializeField] private LayerMask _targetLayer;
        
        public bool Grounded => _triggerCounter > 0;

        private int _triggerCounter;

        private void OnTriggerEnter(Collider other) {
            if (!IsMatch(other.gameObject.layer)) return;
            _triggerCounter++;
            if(_triggerCounter == 1) OnGrounded.Invoke();
        }

        private void OnTriggerExit(Collider other) {
            if (!IsMatch(other.gameObject.layer)) return;
            _triggerCounter--;
            if(!Grounded) OnUngrounded.Invoke();
        }


        private bool IsMatch(int layer) => _targetLayer.value == (_targetLayer.value | 1 << layer);

#if UNITY_EDITOR

        private BoxCollider _collider;
        
        private void OnDrawGizmos() {
            _collider ??= GetComponent<BoxCollider>();
            if(_collider == null) return;
            Gizmos.color = Grounded ? new Color(0,1,0,0.5f) : new Color(1,0,0,0.5f);
            Gizmos.DrawCube(transform.TransformPoint(_collider.center), transform.TransformVector(_collider.size));
        }
        
#endif
        
    }
}