using JescoDev.SmoothBrainStates.Movement;
using UnityEngine;

namespace JescoDev.MovementGraph.Samples.BasicMovement {
    public class CustomMovementSample : MovementGraphCharacterController {

        public float Gravity => - _gravity;
        [SerializeField] private float _gravity = 9.81f;
        
        [field:SerializeField] public float Input { get; private set; }

        private bool _exited = false;

        public void Update() {
            Input = UnityEngine.Input.GetAxis("Horizontal");
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space)) {
                StateMashine.SetState<MovementStateJump>();
                Debug.Log("Jump");
            }
        }
    }
}