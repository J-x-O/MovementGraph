using System;
using System.Collections.Generic;
using JescoDev.SmoothBrainStates.Attributes;
using JescoDev.SmoothBrainStates.States;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.SubStates {
    
    [Serializable, SmoothStateFixedIdentifier("Layer Out"), SmoothStateHideMenu]
    public class SubContainerOut : State, IFastForward {

        public SmoothPort OutReplay => _outReplay;
        [SerializeField, InputPort] private SmoothPort _outReplay = new SmoothPort();
        
        public SmoothPort OutExit => _outExit;
        [SerializeField, InputPort] private SmoothPort _outExit = new SmoothPort();
        
        [field:NonSerialized] public SubContainerIn In { get; internal set; }

        public SubContainerOut() : base("Layer Out") {
            #if UNITY_EDITOR
            _position = new Vector2(200, 0);
            #endif
        }

        public SmoothPort GetNextPort(SmoothPort port) {
            // this node will forward the output to the input, so the whole system loops
            if (port == OutReplay) return In.In;
            
            // if the parent is a state parent with exit, we will forward the output to the exit
            if (port != OutExit) return null;
            return Parent is IStateParentWithExit exit ? exit.RegularExit : null;
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