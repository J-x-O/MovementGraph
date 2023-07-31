using System;
using Entities.Movement.States;
using JescoDev.MovementGraph.Layer;
using JescoDev.MovementGraph.States;
using UnityEngine;

namespace JescoDev.MovementGraph.StateTransition {

    [Serializable]
    public class Transition {
        
        [field:NonSerialized] public MovementPort Target { get; private set; }
        
        [SerializeField] private string _stateGuid;
        [SerializeField] private string _portIdentifier;

        public Transition() { }

        public Transition(MovementPort target) {
            Target = target;
        }

        internal void OnBeforeSerialize(State state) {
            if(Target?.State == null) return;
            _stateGuid = Target.State.Guid;
            _portIdentifier = Target.Identifier;
        }
        
        internal void OnLateDeserialize(State state) {
            State otherState = state.Layer.GetStateGuid(_stateGuid);
            if (otherState == null) {
                Debug.LogWarning($"Couldn't find State with GUID {_stateGuid} in Transition on state {state.Identifier}");
                return;
            }
            Target = otherState.GetPort(_portIdentifier);
        }

    }
}