using System;
using Gameplay.Movement.Layer;
using JescoDev.MovementGraph.MovementGraph.States;
using Movement.States;
using UnityEngine;

namespace JescoDev.MovementGraph.Layer {
    
    [Serializable]
    public class MovementLayerConnector {
        
        public LayerOut OutNode => _outNode;
        [SerializeReference] private LayerOut _outNode = new LayerOut();
        
        public LayerIn InNode => _inNode;
        [SerializeReference] private LayerIn _inNode = new LayerIn();

        public NullState NullState { get; private set; } = new NullState();

        public State[] Nodes => new State[] { _inNode, _outNode, NullState };
        
        public void Awake() {

            // connect the layer in and out nodes
            _inNode.Out = OutNode;
            _outNode.In = InNode;
            _outNode.NullState = NullState;
            
            // connect the null state to the layer in and out nodes, since this node only exists at runtime
            NullState.Restart.ConnectTo(InNode.In);
            OutNode.OutStop.ConnectTo(NullState.In);
        }
    }
}