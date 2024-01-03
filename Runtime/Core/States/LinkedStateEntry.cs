using System;
using JescoDev.SmoothBrainStates.Attributes;
using JescoDev.SmoothBrainStates.SubStates;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.States {
    [Serializable, SmoothStateMenuPath("Default/Linked Entry"), SmoothStateFixedIdentifier("Linked Entry")]
    public class LinkedStateEntry : State {

        [field:SerializeField, InputPort] public SmoothPort InputPort { get; protected set; }
        [SerializeField] private string _targetPath;
        
        private State _target;
        
        public LinkedStateEntry() : base("Linked Entry") { }
        
        protected internal override bool CanBeActivated() => true;

        protected internal override ExecutableState ResolveActivation(SmoothPort incomingPort = null) {
            return _target?.ResolveActivation();
        }

        protected internal override void OnLateDeserialize() {
            _target = StateMashineMachine.GetStateByPath(_targetPath);
            if (_target == null) Debug.LogError($"Could not find state at path {_targetPath}");
        }
    }
}