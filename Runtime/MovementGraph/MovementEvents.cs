using System;
using Entities.Movement.States;
using GameProgramming.Utility.TypeBasedEventSystem;
using JescoDev.Utility.EventUtility.EventUtility;

namespace JescoDev.MovementGraph {
    public class MovementEvents {
        
        public event Action<MovementState> OnAnyStateActivated;
        public event Action<MovementState> OnAnyStateDeactivated;
        public readonly VarBasedEventsSystem<string, MovementState> ID = new();
        public readonly VarBasedEventsSystem<string, MovementState> Tags = new();
        
        public void InvokeStart(MovementState newState, bool fullPath) {
            OnAnyStateActivated.TryInvoke(newState);
            string identifier = fullPath ? newState.Layer.Identifier + "/" + newState.Identifier : newState.Identifier;
            ID.InvokeVarBasedEventStart(identifier, newState);
            foreach (string tag in newState.Tags) {
                Tags.InvokeVarBasedEventStart(tag, newState);
            }
        }
        
        public void InvokeEnd(MovementState oldState, bool fullPath) {
            OnAnyStateDeactivated.TryInvoke(oldState);
            string identifier = fullPath ? oldState.Layer.Identifier + "/" + oldState.Identifier : oldState.Identifier;
            ID.InvokeVarBasedEventEnd(identifier, oldState);
            foreach (string tag in oldState.Tags) {
                Tags.InvokeVarBasedEventEnd(tag, oldState);
            }
        }
    }
}