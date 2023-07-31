﻿using System;
using JescoDev.MovementGraph.MovementGraph.Attributes;
using JescoDev.MovementGraph.StateTransition;
using UnityEngine;

namespace JescoDev.MovementGraph.States {
    
    [Serializable, HideStateIdentifier, MovementMenuPath("Default/Event")]
    public class EventState : State {
        
        [field: SerializeField, OutputPort] public MovementPort OutputPort { get; protected set; }

        public override bool ValidActivation() => true;

        public override MovementState ResolveActivation(MovementPort incomingPort = null)
            => OutputPort.FindFirstValidTransition();

        public EventState() : base("<Hidden>Event") { }
    }
}