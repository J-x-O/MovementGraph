using System;
using Entities.Movement.States;
using Gameplay.Movement.States;
using JescoDev.MovementGraph.MovementGraph.States;
using JescoDev.MovementGraph.States;
using JescoDev.MovementGraph.StateTransition;
using Movement.States;
using UnityEngine;

namespace Gameplay.Movement.Layer {
    
    [Serializable]
    public class LayerOut : State, IFastForward {
        
        public MovementPort OutStop => _outStop;
        [SerializeField, OutputPort] private MovementPort _outStop = new MovementPort();
        
        public MovementPort OutReplay => _outReplay;
        [SerializeField, OutputPort] public MovementPort _outReplay = new MovementPort();

        private NullState _exitState = new NullState();
        public LayerIn _in;
        public LayerOut() {
            _outStop.ConnectTo(_exitState.In);
        }

        public MovementPort GetNextPort(MovementPort port) {
            // this node will forward the output to the input, so the whole system loops
            return port == OutReplay ? _in.In : null;
        }

        public override bool ValidActivation() {
            return true;
        }

        public override MovementState ResolveActivation(MovementPort incomingPort = null) {
            if (incomingPort == _outReplay) return _in.ResolveActivation(_outReplay);
            return _exitState;
        }
    }
}