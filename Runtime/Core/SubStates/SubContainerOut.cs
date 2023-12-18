using System;
using JescoDev.MovementGraph.Editor.Attributes;
using JescoDev.MovementGraph.MovementGraph.Attributes;
using JescoDev.SmoothBrainStates.States;
using JescoDev.SmoothBrainStates.StateTransition;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.SubStates {
    
    [Serializable, FixedStateIdentifier("Layer Out"), MovementHideMenu]
    public class SubContainerOut : State, IFastForward {

        public SmoothPort OutReplay => _outReplay;
        [SerializeField, InputPort] private SmoothPort _outReplay = new SmoothPort();
        
        [field:NonSerialized] public SubContainerIn In { get; internal set; }
        [field:NonSerialized] public IPortEnumerable Connections { get; internal set; }
        
        public SubContainerOut() : base("Layer Out") { }

        public SmoothPort GetNextPort(SmoothPort port) {
            // this node will forward the output to the input, so the whole system loops
            return port == OutReplay ? In.In : null;
        }

        internal override bool CanBeActivated() {
            return true;
        }

        internal override ExecutableState ResolveActivation(SmoothPort incomingPort) {
            if (incomingPort == _outReplay) return In.ResolveActivation(_outReplay);
            return null;
        }
        
    }
}