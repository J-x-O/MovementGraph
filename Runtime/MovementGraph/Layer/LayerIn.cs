using System;
using Entities.Movement.States;
using JescoDev.MovementGraph.States;
using JescoDev.MovementGraph.StateTransition;
using Movement.States;
using UnityEngine;

namespace Gameplay.Movement.Layer {
    
    [Serializable]
    public class LayerIn : State {
        
        public MovementPort In => _in;
        [SerializeField, OutputPort] private MovementPort _in = new MovementPort();

        public override bool ValidActivation() {
            return true;
        }

        public override MovementState ResolveActivation(MovementPort incomingPort = null) {
            return _in.FindFirstValidTransition();
        }

        public LayerIn() : base("LayerOut") { }
    }
}