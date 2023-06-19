using JescoDev.MovementGraph;
using JescoDev.MovementGraph.Layer;
using UnityEngine;

namespace JescoDev.MovementGraphSample {
    public class CustomMovementSample : CustomMovementCharacterController {

        [SerializeField] private MovementSystem _movement;
        
        public float Input { get; private set; }

        private bool _exited = false;

        private MovementLayer _target;

        private void Awake() => _target = _movement.Layers[0];

        public void Update() {
            Input = UnityEngine.Input.GetAxis("Horizontal");
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space)) {
                _target.SendEvent<MovementStateJump>();
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.P)) {
                if(!_target.IsActive) _target.Restart();
                else _target.Stop();
            }
        }
    }
}