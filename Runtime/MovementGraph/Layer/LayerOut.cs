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
    public class LayerOut : State, IFastForward {
        
        public Port OutStop => _outStop;
        [SerializeField, OutputPort] private Port _outStop = new Port();
        
        public Port OutReplay => _outReplay;
        [SerializeField, OutputPort] public Port _outReplay = new Port();

        private NullState _exitState = new NullState();
        public LayerIn _in;
        public LayerOut() {
            _outStop.ConnectTo(_exitState.In);
            
            _outStop.State = this;
            _outReplay.State = this;
        }

        public Port GetNextPort(Port port) {
            // this node will forward the output to the input, so the whole system loops
            return port == OutReplay ? _in.In : null;
        }

        public override bool ValidActivation() {
            return true;
        }

        public override MovementState ResolveActivation(Port incomingPort = null) {
            if (incomingPort == _outReplay) return _in.ResolveActivation(_outReplay);
            return _exitState;
        }
    }
}