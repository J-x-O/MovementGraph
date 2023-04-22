using System;
using Entities.Movement.States;
using Gameplay.Movement.States;
using JescoDev.MovementGraph.States;
using UnityEngine;

namespace Gameplay.Movement.Layer {
    
    [Serializable]
    public class LayerInOut : IFastForward {
        
        public Port In => _in;
        [SerializeField] private Port _in;
        
        public Port OutStop => _outStop;
        [SerializeField] private Port _outStop;
        
        public Port OutReplay => _outReplay;
        [SerializeField] public Port _outReplay;

        public Port GetNextPort(Port port) {
            return port == OutReplay ? In : null;
        }
    }
}