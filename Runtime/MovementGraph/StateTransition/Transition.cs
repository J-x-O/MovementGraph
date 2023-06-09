﻿using System;
using Entities.Movement.States;
using JescoDev.MovementGraph.Layer;
using JescoDev.MovementGraph.States;
using UnityEngine;

namespace JescoDev.MovementGraph.StateTransition {

    [Serializable]
    public class Transition {
        
        [field:NonSerialized] public MovementPort Target { get; private set; }
        
        [SerializeField] private string _stateIdentifier;
        [SerializeField] private string _portIdentifier;

        public Transition() { }

        public Transition(MovementPort target) {
            Target = target;
        }

        public void OnBeforeSerialize(State state) {
            if(Target?.State == null) return;
            _stateIdentifier = Target.State.Identifier;
            _portIdentifier = Target.Identifier;
        }
        
        public void OnLateDeserialize(State state) {
            State otherState = state.Layer.GetState(_stateIdentifier);
            if (otherState == null) {
                Debug.LogWarning($"Couldn't find State {_stateIdentifier} in Transition on state {state.Identifier}");
                return;
            }
            Target = otherState.GetPort(_portIdentifier);
        }

    }
}