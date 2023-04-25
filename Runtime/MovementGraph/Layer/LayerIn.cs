using System;
using Entities.Movement.States;
using JescoDev.MovementGraph.MovementGraph.StateTransition;
using JescoDev.MovementGraph.States;
using Movement.States;
using UnityEngine;

namespace Gameplay.Movement.Layer {
    
    [Serializable]
    public class LayerIn : State {
        
        public Port In => _in;
        [SerializeField, InputPort] private Port _in = new Port();
        
        LayerIn() {
            _in.State = this;
        }

        public override bool ValidActivation() {
            return true;
        }

        public override MovementState ResolveActivation(Port incomingPort = null) {
            return _in.FindFirstValidTransition();
        }
    }
}