using System;
using Entities.Movement.States;
using Gameplay.Movement.States;
using JescoDev.MovementGraph.States;
using JescoDev.MovementGraph.StateTransition;
using Movement.States;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Movement.Layer {
    
    [Serializable]
    public class LayerIn : State, IFastForward {

        public MovementPort In => _in;
        [SerializeField, OutputPort] private MovementPort _in = new MovementPort();

        public LayerOut Out { get; internal set; }

        public override bool ValidActivation() {
            return true;
        }
        
        public override MovementState ResolveActivation(MovementPort incomingPort = null) {
            return _in.FindFirstValidTransition();
        }

        public LayerIn() : base("Layer In") { }
        
        
        public MovementPort GetNextPort(MovementPort port) => Out.OutReplay;
    }
}