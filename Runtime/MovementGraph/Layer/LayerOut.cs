using System;
using Entities.Movement.States;
using Gameplay.Movement.States;
using JescoDev.MovementGraph.States;
using JescoDev.MovementGraph.StateTransition;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Movement.Layer {
    
    [Serializable]
    public class LayerOut : State, IFastForward {

        public MovementPort OutReplay => _outReplay;
        [SerializeField, InputPort] public MovementPort _outReplay = new MovementPort();
        
        public MovementPort OutStop => _outStop;
        [SerializeField, InputPort] private MovementPort _outStop = new MovementPort();

        [field:NonSerialized] public NullState NullState { get; internal set; }
        [field:NonSerialized] public LayerIn In { get; internal set; }
        
        public LayerOut() : base("Layer Out") { }

        public MovementPort GetNextPort(MovementPort port) {
            // this node will forward the output to the input, so the whole system loops
            return port == OutReplay ? In.In : null;
        }

        public override bool ValidActivation() {
            return true;
        }

        public override MovementState ResolveActivation(MovementPort incomingPort = null) {
            if (incomingPort == _outReplay) return In.ResolveActivation(_outReplay);
            return NullState;
        }
        
    }
}