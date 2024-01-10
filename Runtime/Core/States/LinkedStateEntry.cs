using System;
using System.Linq;
using JescoDev.SmoothBrainStates.Attributes;
using JescoDev.SmoothBrainStates.SubStates;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.States {
    [Serializable, SmoothStateMenuPath("Default/Linked Entry"), SmoothStateFixedIdentifier("Linked Entry")]
    public class LinkedStateEntry : State, IFastForward {

        [field:SerializeField, InputPort] public SmoothPort InputPort { get; protected set; }
        [SerializeField] private string _targetPath;
        
        private State _target;
        
        public LinkedStateEntry() : base("Linked Entry") { }
        
        protected internal override bool CanBeActivated() => true;

        protected internal override ExecutableState ResolveActivation(SmoothPort incomingPort = null) {
            return _target?.ResolveActivation();
        }

        protected internal override void OnLateDeserialize() => _target = StateMashine.GetStateByPath(_targetPath);

        // todo: sus for multiple ports
        public SmoothPort GetNextPort(SmoothPort port) => _target.GetOutputPorts().FirstOrDefault();
    }
}