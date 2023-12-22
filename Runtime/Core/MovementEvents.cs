using System;
using GameProgramming.Utility.TypeBasedEventSystem;
using JescoDev.SmoothBrainStates.States;
using JescoDev.Utility.EventUtility.EventUtility;

namespace JescoDev.SmoothBrainStates {
    public class MovementEvents {

        public event Action<ExecutableState> OnAnyStateActivated;
        public event Action<ExecutableState> OnAnyStateDeactivated;
        
        public IReadOnlyVarBasedEventSystem<string, ExecutableState> ID => _id;
        private readonly VarBasedEventSystem<string, ExecutableState> _id = new();
        public IReadOnlyVarBasedEventSystem<string, ExecutableState> Path => _path;
        private readonly VarBasedEventSystem<string, ExecutableState> _path = new();
        
        internal void InvokeStart(ExecutableState newState) {
            OnAnyStateActivated.TryInvoke(newState);
            string fullPath = newState.Parent.ResolvePath() + "/" + newState.Identifier;
            _id.InvokeVarBasedEventStart(newState.Identifier, newState);
            _path.InvokeVarBasedEventStart(fullPath, newState);
        }
        
        internal void InvokeEnd(ExecutableState oldState) {
            OnAnyStateDeactivated.TryInvoke(oldState);
            if(oldState == null) return;
            string fullPath = oldState.Parent.ResolvePath() + "/" + oldState.Identifier;
            _id.InvokeVarBasedEventEnd(oldState.Identifier, oldState);
            _path.InvokeVarBasedEventEnd(fullPath, oldState);
        }
    }
}