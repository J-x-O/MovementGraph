using System;
using JescoDev.SmoothBrainStates.States;
using JescoDev.SmoothBrainStates.StateTransition;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.SubStates {
    
    [Serializable]
    public class MovementLayerConnector {
        
        public SubContainerOut OutNode => _outNode;
        [SerializeReference] private SubContainerOut _outNode;
        
        public SubContainerIn InNode => _inNode;
        [SerializeReference] private SubContainerIn _inNode;
        
        public State[] Nodes => new State[] { _inNode, _outNode };
        
        private IStateParent _parent;
        
        public void Awake(IStateParent parent) {
            _parent = parent;
            
            // connect the layer in and out nodes, so they can do repeat logic
            _inNode.Out = OutNode;
            _outNode.In = InNode;
        }

        

        public ExecutableState ResolveActivation(SmoothPort incomingPort = null) {
            return _inNode.ResolveActivation(incomingPort);
        }
    }
}