using System;

namespace JescoDev.MovementGraph {
    public interface IMovementSource {

        public event Action OnValueFlipped;
        
        public float MovementValue { get; }
        public float ControlledDirection { get; }
    }
}