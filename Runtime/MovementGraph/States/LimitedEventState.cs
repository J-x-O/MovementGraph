using System;
using JescoDev.MovementGraph.MovementGraph.Attributes;
using JescoDev.MovementGraph.StateTransition;
using UnityEngine;

namespace JescoDev.MovementGraph.States {
    
    [Serializable, MovementMenuPath("Default/Limited Event")]
    public class LimitedEventState : EventState {

        [field: SerializeField, InputPort] public MovementPort InputPort { get; private set; }

        public override bool ValidActivation() => InputPort.HasActiveTransition(true);
    }
}