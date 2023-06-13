﻿using System;
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

        public MovementPort OutReplay => _outReplay;
        [SerializeField, InputPort] public MovementPort _outReplay = new MovementPort();
        
        public MovementPort OutStop => _outStop;
        [SerializeField, InputPort] private MovementPort _outStop = new MovementPort();

        public NullState ExitState { get; private set; }

        public LayerIn _in;
        public LayerOut() : base("Layer Out") {
            ExitState = new NullState(Layer);
            _outStop.ConnectTo(ExitState.In);
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
            return ExitState;
        }
        
    }
}