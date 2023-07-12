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

        public MovementStateNull MovementStateNull { get; private set; } = new();

        public State[] Nodes => new State[] { _inNode, _outNode, MovementStateNull };
        
        public void Awake() {

            // connect the layer in and out nodes
            _inNode.Out = OutNode;
            _outNode.In = InNode;
            _outNode.MovementStateNull = MovementStateNull;
            
            // connect the null state to the layer in and out nodes, since this node only exists at runtime
            MovementStateNull.RegularExit.ConnectTo(InNode.In);
            OutNode.OutStop.ConnectTo(MovementStateNull.InputPort);
        }
    }
}