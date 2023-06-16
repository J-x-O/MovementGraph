using System;
using JescoDev.MovementGraph.StateTransition;
using UnityEngine;

namespace JescoDev.MovementGraph.States {
    
    [Serializable]
    public class LimitedEventState : EventState {

        [field: SerializeField, InputPort] public MovementPort InputPort { get; private set; }

        public override bool ValidActivation() => InputPort.HasActiveTransition(true);
    }
}