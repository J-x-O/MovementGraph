using System;
using JescoDev.MovementGraph.States;

namespace JescoDev.MovementGraph.Customization {
    
    [Serializable]
    public abstract class MovementState<T> : MovementState where T : CustomMovement {
        
        protected T Custom => System.Custom as T;
        
    }
}