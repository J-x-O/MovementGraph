using System;
using Entities.Movement.States;
using Gameplay.Movement.States;
using JescoDev.MovementGraph.MovementGraph.States;
using JescoDev.MovementGraph.MovementGraph.StateTransition;
using JescoDev.MovementGraph.States;
using Movement.States;
using UnityEngine;

namespace Gameplay.Movement.Layer {
    
    [Serializable]
    public class LayerInOut : MovementState, IFastForward {
        
        public Port In => _in;
        [SerializeField, InputPort] private Port _in;
        
        public Port OutStop => _outStop;
        [SerializeField, OutputPort] private Port _outStop;
        
        public Port OutReplay => _outReplay;
        [SerializeField, OutputPort] public Port _outReplay;

        private NullState _exitState;
        
        public LayerInOut() {
            _exitState = new NullState();
            _exitState.
            
            _in.State = this;
            _outStop.State = this;
            _outReplay.State = this;
        }
        
        public Port GetNextPort(Port port) {
            // this node will forward the output to the input, so the whole system loops
            return port == OutReplay ? In : null;
        }

        public override bool ValidActivation() {
            return false;
        }

        public override MovementState ResolveActivation() {
            throw new NotImplementedException();
        }
    }
}