using System;
using UnityEngine;

namespace JescoDev.MovementGraph.Sample.Sample {
    public class CustomMovementSample : CustomMovementCharacterController {

        [SerializeField] private MovementSystem _movement;
        
        public float Input { get; private set; }

        private bool _exited = false;
        
        public void Update() {
            Input = UnityEngine.Input.GetAxis("Horizontal");
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space)) {
                _movement.Layers[0].SendEvent<MovementStateJump>();
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.P)) {
                if(_exited) _movement.Layers[0].Restart();
                else _movement.Layers[0].Stop();
            }
        }
    }
}