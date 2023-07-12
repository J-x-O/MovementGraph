using System;
using Entities.Movement.States;
using GameProgramming.Utility.TypeBasedEventSystem;
using JescoDev.MovementGraph.States;
using JescoDev.Utility.EventUtility.EventUtility;

namespace JescoDev.MovementGraph {
    public class MovementEvents {

        public event Action<MovementState> OnAnyStateActivated;
        public event Action<MovementState> OnAnyStateDeactivated;
        
        public IReadOnlyVarBasedEventSystem<string, MovementState> ID => _id;
        private readonly VarBasedEventSystem<string, MovementState> _id = new();
        public IReadOnlyVarBasedEventSystem<string, MovementState> Tags => _tags;
        private readonly VarBasedEventSystem<string, MovementState> _tags = new();
        
        internal void InvokeStart(MovementState newState) {
            OnAnyStateActivated.TryInvoke(newState);
            string fullPath = newState.Layer.Identifier + "/" + newState.Identifier;
            _id.InvokeVarBasedEventStart(fullPath, newState);
            _id.InvokeVarBasedEventStart(newState.Identifier, newState);
            foreach (string tag in newState.Tags) {
                _tags.InvokeVarBasedEventStart(tag, newState);
            }
        }
        
        internal void InvokeEnd(MovementState oldState) {
            OnAnyStateDeactivated.TryInvoke(oldState);
            if(oldState == null) return;
            string fullPath = oldState.Layer.Identifier + "/" + oldState.Identifier;
            _id.InvokeVarBasedEventEnd(fullPath, oldState);
            _id.InvokeVarBasedEventEnd(oldState.Identifier, oldState);
            foreach (string tag in oldState.Tags) {
                _tags.InvokeVarBasedEventEnd(tag, oldState);
            }
        }
    }
}