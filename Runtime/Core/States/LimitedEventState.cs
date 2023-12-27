using System;
using JescoDev.SmoothBrainStates.Attributes;
using UnityEngine;

namespace JescoDev.SmoothBrainStates.States {
    
    [Serializable, SmoothStateMenuPath("Default/Limited Event")]
    public class LimitedEventState : EventState {

        [field: SerializeField, InputPort] public SmoothPort InputPort { get; private set; }

        protected internal override bool CanBeActivated() => InputPort.HasActiveTransition(true);
    }
}