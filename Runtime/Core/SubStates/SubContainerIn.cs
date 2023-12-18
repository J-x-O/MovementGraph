using System;
using System.Collections.Generic;
using JescoDev.MovementGraph.Editor.Attributes;
using JescoDev.MovementGraph.MovementGraph.Attributes;
using JescoDev.SmoothBrainStates.States;
using JescoDev.SmoothBrainStates.StateTransition;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.SubStates {
    
    [Serializable, FixedStateIdentifier("Layer In"), MovementHideMenu]
    public class SubContainerIn : State {

        [SerializeField, OutputPort] private SmoothPort _in;
        [SerializeField, OutputPort] private List<SmoothPort> _extra = new List<SmoothPort>();
        
        [field:NonSerialized] public SubContainerOut Out { get; internal set; }

        internal override bool CanBeActivated() {
            return true;
        }
        
        internal override ExecutableState ResolveActivation(SmoothPort incomingPort) {
            foreach (SmoothPort port in _extra) {
                if (port.Identifier != incomingPort?.Identifier) continue;
                ExecutableState resolve = port.FindFirstValidTransition();
                if (resolve != null && resolve.CanBeActivated()) return resolve;
            }
            Debug.LogWarning("Couldn't find valid transition for incoming port {incomingPort?.Identifier}");
            return _in.FindFirstValidTransition();
        }

        public SubContainerIn() : base("Layer In") { }
        
    }
}