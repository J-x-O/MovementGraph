using System;
using GameProgramming.Utility.TypeBasedEventSystem;
using JescoDev.MovementGraph.States;
using JescoDev.Utility.EventUtility.EventUtility;

namespace JescoDev.MovementGraph {
    public class MovementEvents {
        
        public event Action<MovementState> OnAnyStateActivated;
        public event Action<MovementState> OnAnyStateDeactivated;
        public readonly VarBasedEventsSystem<string, MovementState> ID = new();
        public readonly VarBasedEventsSystem<string, MovementState> Tags = new();
        
        public void InvokeStart(MovementState newState) {
            OnAnyStateActivated.TryInvoke(newState);
            ID.InvokeVarBasedEventStart(newState.Identifier, newState);
            foreach (string tag in newState.Tags) {
                Tags.InvokeVarBasedEventStart(tag, newState);
            }
        }
        
        public void InvokeEnd(MovementState oldState) {
            OnAnyStateDeactivated.TryInvoke(oldState);
            ID.InvokeVarBasedEventEnd(oldState.Identifier, oldState);
            foreach (string tag in oldState.Tags) {
                Tags.InvokeVarBasedEventEnd(tag, oldState);
            }
        }
    }
}