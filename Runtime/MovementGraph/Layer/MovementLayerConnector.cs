using System;
using Gameplay.Movement.Layer;
using JescoDev.MovementGraph.States;
using UnityEngine;

namespace JescoDev.MovementGraph.Layer {
    
    [Serializable]
    public class MovementLayerConnector {
        
        public LayerOut OutNode => _outNode;
        [SerializeReference] private LayerOut _outNode;
        
        public LayerIn InNode => _inNode;
        [SerializeReference] private LayerIn _inNode;

        public NullState NullState { get; private set; } = new();

        public State[] Nodes => new State[] { _inNode, _outNode, NullState };
        
        public void Awake() {

            // connect the layer in and out nodes
            _inNode.Out = OutNode;
            _outNode.In = InNode;
            _outNode.NullState = NullState;
            
            // connect the null state to the layer in and out nodes, since this node only exists at runtime
            NullState.RegularExit.ConnectTo(InNode.In);
            OutNode.OutStop.ConnectTo(NullState.InputPort);
        }
    }
}