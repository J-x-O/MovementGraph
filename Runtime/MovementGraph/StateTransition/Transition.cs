using System;
using Entities.Movement.States;
using JescoDev.MovementGraph.Layer;
using JescoDev.MovementGraph.States;
using Movement.States;
using UnityEngine;

namespace JescoDev.MovementGraph.StateTransition {

    [Serializable]
    public class Transition {
        
        public MovementPort Target { get; private set; }
        
        [SerializeField] private int _stateIndex;
        [SerializeField] private string _stateIdentifier;
        [SerializeField] private string _portIdentifier;

        public Transition() { }

        public Transition(MovementPort target) {
            Target = target;
        }

        public void OnBeforeSerialize(State state) {
            _stateIdentifier = Target.State is NamedState namedState ? namedState.Identifier : "";
            _stateIndex = state.Layer.GetStateID(Target.State);
            _portIdentifier = Target.Identifier;
        }
        
        public void OnLateDeserialize(State state) {
            State otherState = ByIdentifier(state.Layer) ?? ByIndex(state.Layer);
            if(otherState == null) return;
            Target = otherState.GetPort(_portIdentifier);
        }

        private State ByIndex(MovementLayer layer) {
            if(_stateIndex < 0 || _stateIndex >= layer.States.Count) return null;
            return layer.States[_stateIndex];
        }
        
        private State ByIdentifier(MovementLayer layer) {
            return layer.GetState(_stateIdentifier);
        }
        
    }
}