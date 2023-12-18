using System;
using JescoDev.MovementGraph.MovementGraph.Attributes;
using JescoDev.SmoothBrainStates.StateTransition;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.States {
    
    [Serializable, MovementMenuPath("Default/Limited Event")]
    public class LimitedEventState : EventState {

        [field: SerializeField, InputPort] public SmoothPort InputPort { get; private set; }

        internal override bool CanBeActivated() => InputPort.HasActiveTransition(true);
    }
}