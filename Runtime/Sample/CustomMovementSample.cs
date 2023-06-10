using System;

namespace JescoDev.MovementGraph.Sample.Sample {
    public class CustomMovementSample : CustomMovementCharacterController {
        
        public float Input { get; private set; }

        public void Update() {
            Input = UnityEngine.Input.GetAxis("Horizontal");
        }
    }
}