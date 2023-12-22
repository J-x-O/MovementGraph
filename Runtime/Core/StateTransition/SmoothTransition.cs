using System;
using JescoDev.SmoothBrainStates.States;
using JescoDev.SmoothBrainStates.SubStates;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.StateTransition {

    [Serializable]
    public class SmoothTransition {
        
        [field:NonSerialized] public SmoothPort Target { get; private set; }
        
        [SerializeField] private string _stateGuid;
        [SerializeField] private string _portIdentifier;

        public SmoothTransition() { }

        public SmoothTransition(SmoothPort target) {
            Target = target;
        }

        internal void OnBeforeSerialize(State state) {
            if(Target?.State == null) return;
            _stateGuid = Target.State.Guid;
            _portIdentifier = Target.Identifier;
        }
        
        internal void OnLateDeserialize(State state) {
            State otherState = state.Parent.GetStateByGuid(_stateGuid);
            if (otherState == null) {
                Debug.LogWarning($"Couldn't find State with GUID {_stateGuid} in Transition on state {state.Identifier}");
                return;
            }
            Target = otherState.GetPort(_portIdentifier);
        }

    }
}