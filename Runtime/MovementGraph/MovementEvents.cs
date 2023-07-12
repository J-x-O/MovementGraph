﻿using System;
using Entities.Movement.States;
using GameProgramming.Utility.TypeBasedEventSystem;
using JescoDev.MovementGraph.States;
using JescoDev.Utility.EventUtility.EventUtility;

namespace JescoDev.MovementGraph {
    public class MovementEvents {

        public event Action<MovementState> OnAnyStateActivated;
        public event Action<MovementState> OnAnyStateDeactivated;
        
        public readonly VarBasedEventsSystem<string, MovementState> ID = new();
        public readonly VarBasedEventsSystem<string, MovementState> Tags = new();
        
        internal void InvokeStart(MovementState newState) {
            OnAnyStateActivated.TryInvoke(newState);
            string fullPath = newState.Layer.Identifier + "/" + newState.Identifier;
            ID.InvokeVarBasedEventStart(fullPath, newState);
            ID.InvokeVarBasedEventStart(newState.Identifier, newState);
            foreach (string tag in newState.Tags) {
                Tags.InvokeVarBasedEventStart(tag, newState);
            }
        }
        
        internal void InvokeEnd(MovementState oldState) {
            OnAnyStateDeactivated.TryInvoke(oldState);
            if(oldState == null) return;
            string fullPath = oldState.Layer.Identifier + "/" + oldState.Identifier;
            ID.InvokeVarBasedEventEnd(fullPath, oldState);
            ID.InvokeVarBasedEventEnd(oldState.Identifier, oldState);
            foreach (string tag in oldState.Tags) {
                Tags.InvokeVarBasedEventEnd(tag, oldState);
            }
        }
    }
}