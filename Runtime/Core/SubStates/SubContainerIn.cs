using System;
using System.Collections.Generic;
using JescoDev.SmoothBrainStates.Attributes;
using JescoDev.SmoothBrainStates.States;
using JescoDev.SmoothBrainStates.StateTransition;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.SubStates {
    
    [Serializable, SmoothStateFixedIdentifier("Layer In"), SmoothStateHideMenu]
    public class SubContainerIn : State {

        public SmoothPort In => _in;  
        [SerializeField, OutputPort] private SmoothPort _in;  
        [SerializeField, InputPort] private List<SmoothPort> _extra = new List<SmoothPort>();
        [field:NonSerialized] public SubContainerOut Out { get; internal set; }

        protected internal override bool CanBeActivated() {
            return true;
        }
        
        protected internal override ExecutableState ResolveActivation(SmoothPort incomingPort) {
            if (incomingPort == null) return _in.FindFirstValidTransition();
            foreach (SmoothPort port in _extra) {
                if (port.Identifier != incomingPort.Identifier) continue;
                ExecutableState resolve = port.FindFirstValidTransition();
                if (resolve != null && resolve.CanBeActivated()) return resolve;
            }
            Debug.LogWarning("Couldn't find valid transition for incoming port {incomingPort?.Identifier}");
            return _in.FindFirstValidTransition();
        }

        public SubContainerIn() : base("Layer In") { }
        
    }
}