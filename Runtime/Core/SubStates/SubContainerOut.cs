using System;
using System.Collections.Generic;
using JescoDev.SmoothBrainStates.Attributes;
using JescoDev.SmoothBrainStates.States;
using JescoDev.SmoothBrainStates.StateTransition;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.SubStates {
    
    [Serializable, SmoothStateFixedIdentifier("Layer Out"), SmoothStateHideMenu]
    public class SubContainerOut : State, IFastForward {

        public SmoothPort OutReplay => _outReplay;
        [SerializeField, InputPort] private SmoothPort _outReplay = new SmoothPort();
        
        [SerializeField, InputPort] private List<SmoothPort> _extra = new List<SmoothPort>();
        
        [field:NonSerialized] public SubContainerIn In { get; internal set; }
        
        public SubContainerOut() : base("Layer Out") { }

        public SmoothPort GetNextPort(SmoothPort port) {
            // this node will forward the output to the input, so the whole system loops
            return port == OutReplay ? In.In : null;
        }

        protected internal override bool CanBeActivated() {
            return true;
        }

        protected internal override ExecutableState ResolveActivation(SmoothPort incomingPort) {
            if (incomingPort == _outReplay) return In.ResolveActivation(_outReplay);
            return null;
        }
        
    }
}