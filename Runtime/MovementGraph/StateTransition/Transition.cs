using System;
using JescoDev.MovementGraph.States;
using Movement.States;
using UnityEngine;

namespace JescoDev.MovementGraph.StateTransition {

    [Serializable]
    public class Transition {
        
        public MovementPort Target { get; private set; }
        
        [SerializeField] private int _stateIndex;
        [SerializeField] private string _portIdentifier;

        public Transition() { }

        public Transition(MovementPort target) {
            Target = target;
        }

        public void OnBeforeSerialize(State state) {
            _stateIndex = state.Layer.GetStateID(Target.State);
            _portIdentifier = Target.Identifier;
        }
        
        public void OnLateDeserialize(State state) {
            if(_stateIndex < 0 || _stateIndex >= state.Layer.States.Count) return;
            State otherState = state.Layer.States[_stateIndex];
            Target = otherState?.GetPort(_portIdentifier);
        }

        
    }
}