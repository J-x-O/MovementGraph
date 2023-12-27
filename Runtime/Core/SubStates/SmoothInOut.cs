using System;
using JescoDev.SmoothBrainStates.States;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.SubStates {
    
    [Serializable]
    public class SmoothInOut : ISerializationCallbackReceiver {
        
        public SubContainerOut OutNode => _outNode;
        [SerializeReference] private SubContainerOut _outNode = new();
        
        public SubContainerIn InNode => _inNode;
        [SerializeReference] private SubContainerIn _inNode = new();
        
        public State[] Nodes => new State[] { _inNode, _outNode };
        
        public ExecutableState ResolveActivation(SmoothPort incomingPort = null) {
            return _inNode.ResolveActivation(incomingPort);
        }

        public void OnBeforeSerialize() { }
        public void OnAfterDeserialize() {
            // connect the layer in and out nodes, so they can do repeat logic
            _inNode.Out = OutNode;
            _outNode.In = InNode;
        }
    }
}