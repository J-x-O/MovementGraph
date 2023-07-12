using UnityEngine;

namespace JescoDev.MovementGraph.Samples.BasicMovement {
    public class CustomMovementSample : CustomMovementCharacterController {

        [SerializeField] private MovementSystem _movement;

        public float Gravity => - _gravity;
        [SerializeField] private float _gravity = 9.81f;
        
        public float Input { get; private set; }

        private bool _exited = false;

        public void Update() {
            Input = UnityEngine.Input.GetAxis("Horizontal");
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space)) {
                _movement.Layers[0].SetState<MovementStateJump>();
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.P)) {
                if(!_movement.Layers[1].IsActive) _movement.Layers[1].Restart();
                else _movement.Layers[1].Stop();
            }
        }
    }
}