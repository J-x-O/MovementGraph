using System;
using System.Collections.Generic;
using JescoDev.SmoothBrainStates.Attributes;
using JescoDev.SmoothBrainStates.States;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.SubStates {
    
    [Serializable, SmoothStateFixedIdentifier("Layer In"), SmoothStateHideMenu]
    public class SubContainerIn : State {

        public SmoothPort In => _in;  
        [SerializeField, OutputPort] private SmoothPort _in;  
        [field:NonSerialized] public SubContainerOut Out { get; internal set; }

        protected internal override bool CanBeActivated() {
            return true;
        }
        
        protected internal override ExecutableState ResolveActivation(SmoothPort incomingPort = null) {
            return _in.FindFirstValidTransition();
        }

        public SubContainerIn() : base("Layer In") { }
        
    }
}