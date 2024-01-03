using System;
using JescoDev.SmoothBrainStates.Attributes;
using JescoDev.SmoothBrainStates.SubStates;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.States {
    [Serializable, SmoothStateMenuPath("Default/Linked Entry"), SmoothStateFixedIdentifier("Linked Entry")]
    public class LinkedStateEntry : State {

        [field:SerializeField, InputPort] public SmoothPort InputPort { get; protected set; }
        [SerializeField] private string _targetPath;
        
        public LinkedStateEntry() : base("Linked Entry") { }
        
        protected internal override bool CanBeActivated() => true;

        protected internal override ExecutableState ResolveActivation(SmoothPort incomingPort = null) {
            State target = StateMashineMachine.GetStateByPath(_targetPath);
            if (target == null) {
                Debug.LogError($"Could not find state at path {_targetPath}");
                return null;
            }
            
            return target.ResolveActivation();
        }
    }
}